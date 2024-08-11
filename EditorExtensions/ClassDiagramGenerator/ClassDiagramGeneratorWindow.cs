using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace HvcNeoria.Unity.Utils
{
    public class ClassDiagramGeneratorWindow : EditorWindow
    {
        private readonly string[] packageingModes = new string[] { "Namespace Mode", "Directory Mode" };

        private string assemblyPath;
        private string outputPath = "Assets/classDiagram.pu";
        private bool isGeneratingPrivate = true;
        private int packagingModeIndex;
        // パッケージングモードがディレクトリモードのときに参照するパス
        private string rootPathOnDirectoryMode;

        /// <summary>
        /// Unity上部メニューに追加された項目を押下時にウインドウを表示する。
        /// </summary>
        [MenuItem("Tools/Class Diagram Generator")]
        public static void ShowWindow()
        {
            var window = GetWindow<ClassDiagramGeneratorWindow>("Class Diagram Generator");
            window.minSize = new Vector2(400, 200);
        }

        /// <summary>
        /// GUI更新時に呼ばれる。
        /// </summary>
        private void OnGUI()
        {
            GUILayout.Label("Class Diagram Generator", EditorStyles.boldLabel);

            assemblyPath = Path.Combine(Application.dataPath, "..", "Library", "ScriptAssemblies", "Assembly-CSharp.dll");
            assemblyPath = EditorGUILayout.TextField("Assembly Path", assemblyPath);
            outputPath = EditorGUILayout.TextField("Output Path", outputPath);
            isGeneratingPrivate = EditorGUILayout.Toggle("Is Generating Private", isGeneratingPrivate);
            packagingModeIndex = EditorGUILayout.Popup("Packaging Type", packagingModeIndex, packageingModes);
            rootPathOnDirectoryMode = EditorGUILayout.TextField("Root Path On Directory Mode", rootPathOnDirectoryMode);

            if (GUILayout.Button("Generate Class Diagram"))
            {
                GenerateClassDiagram();
                AssetDatabase.Refresh();
                EditorUtility.DisplayDialog("Class Diagram Generator", "Class diagram generated at " + outputPath, "OK");
            }
        }

        /// <summary>
        /// クラス図を生成する。
        /// </summary>
        private void GenerateClassDiagram()
        {
            // var hoge = Hoge(rootPathOnDirectoryMode);

            Namespace rootNamespace = AnalyzeAssembly(assemblyPath);
            GeneratePlantUml(rootNamespace, outputPath);
        }

        private DirectoryController Hoge(string path)
        {
            var directories = Directory.GetDirectories(path);
            foreach (var d in directories)
            {
                Hoge(d);
            }

            var directoryController = new DirectoryController(Path.GetFileName(path));
            var csFiles = Directory.GetFiles(path, "*.cs", SearchOption.AllDirectories);
            foreach (var file in csFiles)
            {
                // ファイル内容を読み込む
                string code = File.ReadAllText(file);

                // クラス名を検索する正規表現
                // 正規表現は、`class`キーワードに続く単語を探します
                Regex regex = new Regex(@"\bclass\s+(\w+)");

                // マッチしたクラス名をすべて取得
                var matches = regex.Matches(code);

                // 各クラス名を出力
                foreach (Match match in matches)
                {
                    directoryController.Add(new ClassName(match.Groups[1].Value, ""));
                }
            }

            return directoryController;
        }

        /// <summary>
        /// アセンブリを解析する。
        /// </summary>
        /// <param name="assemblyPath"></param>
        /// <returns></returns>
        private Namespace AnalyzeAssembly(string assemblyPath)
        {
            var assembly = Assembly.LoadFrom(assemblyPath);
            var items = new Namespace("");

            foreach (var type in assembly.GetTypes())
            {
                if (!(type.Namespace == null || type.Namespace == "HvcNeoria.Unity.Utils"))
                {
                    continue;
                }

                if (type.IsClass)
                {
                    if (type.Name.StartsWith("<")) continue;
                    if (type.Name.StartsWith("k__")) continue;

                    var className = CleanClassName(type.Name);
                    var classInfo = new ClassInfo(className);

                    // 継承関係を解析
                    if (type.BaseType != null && type.BaseType != typeof(object))
                    {
                        classInfo.BaseClasses.Add(CleanClassName(type.BaseType.Name));
                    }

                    // インターフェースの実装を解析
                    foreach (var iface in type.GetInterfaces())
                    {
                        classInfo.Interfaces.Add(CleanClassName(iface.Name));
                    }

                    var bindingFlags = BindingFlags.Public | BindingFlags.Instance;

                    if (isGeneratingPrivate)
                    {
                        bindingFlags |= BindingFlags.NonPublic;
                    }

                    // フィールドを解析
                    foreach (var field in type.GetFields(bindingFlags))
                    {
                        if (field.Name.Contains("k__BackingField")) continue;
                        if (field.Name.StartsWith("m_")) continue;
                        classInfo.Fields.Add(new MemberInfo(field));
                        classInfo.Dependencies.Add(CleanClassName(field.FieldType.Name));
                    }

                    // プロパティを解析
                    foreach (var prop in type.GetProperties(bindingFlags))
                    {
                        // プロパティが自身で宣言されたものであり、バックフィールドでないことを確認
                        if (prop.Name.Contains("k__BackingField")) continue;

                        if (prop.DeclaringType == type)
                        {
                            classInfo.Properties.Add(new Property(prop));
                            classInfo.Dependencies.Add(CleanClassName(prop.PropertyType.Name));
                        }
                    }

                    // メソッドを解析
                    foreach (var method in type.GetMethods(bindingFlags | BindingFlags.DeclaredOnly))
                    {
                        if (method == null)
                        {
                            Debug.Log("W");
                        }
                        // プロパティのget/setメソッドを除外
                        if (method.IsSpecialName) continue;
                        if (method.Name.StartsWith("<")) continue;
                        classInfo.Methods.Add(new MemberInfo(method));
                        classInfo.Dependencies.Add(CleanClassName(method.ReturnType.Name));

                        foreach (var param in method.GetParameters())
                        {
                            classInfo.Dependencies.Add(CleanClassName(param.ParameterType.Name));
                        }
                    }

                    if (type.Namespace == null)
                    {
                        items.Add(classInfo);
                    }
                    else
                    {
                        var splitedNamespace = type.Namespace.Split('.');
                        AddClassInfoTo(items.Items, splitedNamespace, classInfo);
                    }
                }
            }

            return items;
        }

        private static void AddClassInfoTo(HashSet<IItem> items, string[] splitedNamespace, ClassInfo classInfo, int currentIndex = 0)
        {
            Debug.Log(string.Join(", ", splitedNamespace));
            Debug.Log(classInfo.Name);
            // if (classInfo.Name == "AxisToButtons")
            // {
            //     Debug.Log(classInfo.Name);

            // }
            var n = items.OfType<Namespace>().FirstOrDefault(v => v.Name == splitedNamespace[currentIndex]);

            if (n == null)
            {
                var newNameSpace = new Namespace(splitedNamespace[currentIndex]);
                items.Add(newNameSpace);

                if (currentIndex == splitedNamespace.Length - 1)
                {
                    newNameSpace.Add(classInfo);
                    return;
                }

                AddClassInfoTo(newNameSpace.Items, splitedNamespace, classInfo, currentIndex + 1);
            }
            else
            {
                if (currentIndex == splitedNamespace.Length - 1)
                {
                    n.Add(classInfo);
                    return;
                }

                AddClassInfoTo(n.Items, splitedNamespace, classInfo, currentIndex + 1);
            }
        }

        private void GeneratePlantUml(Namespace rootNamespace, string outputPath)
        {
            var allClasses = rootNamespace.GetClasses();

            using (var writer = new StreamWriter(outputPath))
            {
                writer.WriteLine("@startuml");
                WriteClassesOf(rootNamespace, allClasses, writer);

                writer.WriteLine("@enduml");
            }
        }

        private static void WriteClassesOf(Namespace rootNamespace, IEnumerable<ClassInfo> allClasses, StreamWriter writer)
        {
            foreach (var item in rootNamespace.Items)
            {
                var n = item as Namespace;
                if (n != null)
                {
                    writer.WriteLine($"namespace {n.Name} {{");
                    WriteClassesOf(n, allClasses, writer);
                    writer.WriteLine("}");
                    continue;
                }

                var c = item as ClassInfo;
                if (c != null)
                {
                    // itemがclassなら、描画
                    writer.WriteLine($"class {c.Name} {{");

                    foreach (var field in c.Fields)
                    {
                        writer.WriteLine($"  {field.Accessibility.ToSymbol()} {field.Type} {field.Name}");
                    }

                    foreach (var prop in c.Properties)
                    {
                        var getter = prop.CanGet ? $"{prop.GetterAccessibility} get; " : "";
                        var setter = prop.CanSet ? $"{prop.SetterAccessibility} set; " : "";
                        writer.WriteLine($"  {{method}}{prop.Accessibility.ToSymbol()} {prop.Type} {prop.Name} {{{getter}{setter}}}");
                    }

                    foreach (var method in c.Methods)
                    {
                        writer.WriteLine($"  {method.Accessibility.ToSymbol()} {method.Type} {method.Name}()");
                    }

                    writer.WriteLine("}");

                    foreach (var baseClass in c.BaseClasses)
                    {
                        writer.WriteLine($"{c.Name} --|> {baseClass}");
                    }

                    foreach (var iface in c.Interfaces)
                    {
                        writer.WriteLine($"{c.Name} ..|> {iface}");
                    }

                    foreach (var dependency in c.Dependencies)
                    {
                        if (allClasses.Any(ci => ci.Name == dependency))
                        {
                            var index = dependency.IndexOf("[");
                            var unitDependency = index == -1 ? dependency : dependency.Substring(0, index);
                            writer.WriteLine($"{c.Name} --> {unitDependency}");
                        }
                        else
                        {
                            // var index = dependency.IndexOf("[");
                            // var unitDependency = index == -1 ? dependency : dependency.Substring(0, index);
                            // writer.WriteLine($"{classInfo.Name} ..> {unitDependency}");

                        }
                    }
                    continue;
                }

                throw new Exception("エラー");
            }
        }

        private string CleanClassName(string className)
        {
            int backtickIndex = className.IndexOf('`');
            return backtickIndex == -1 ? className : className.Substring(0, backtickIndex);
        }
    }
}
