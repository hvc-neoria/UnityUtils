using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class ClassDiagramGenerator : EditorWindow
{
    private readonly string[] packageingModes = new string[] { "Namespace Mode", "Directory Mode" };

    private string assemblyPath;
    private string outputPath = "Assets/classDiagram.pu";
    private bool isGeneratingPrivate = true;
    private int packagingModeIndex;
    // パッケージングモードがディレクトリモードのときに参照するパス
    private string rootPathOnDirectoryMode;

    [MenuItem("Tools/Class Diagram Generator")]
    public static void ShowWindow()
    {
        var window = GetWindow<ClassDiagramGenerator>("Class Diagram Generator");
        window.minSize = new Vector2(400, 200);
    }

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
        }
    }

    private void GenerateClassDiagram()
    {
        // Hoge(rootPathOnDirectoryMode);
        // rootPathOnDirectoryMode;

        Namespace rootNamespace = AnalyzeAssembly(assemblyPath);
        GeneratePlantUml(rootNamespace, outputPath);
        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("Class Diagram Generator", "Class diagram generated at " + outputPath, "OK");
    }

    // private DirectoryController Hoge(string path)
    // {
    //     var directories = Directory.GetDirectories(path);
    //     foreach (var d in directories)
    //     {
    //         Hoge(d);
    //     }

    //     var directoryController = new DirectoryController(Path.GetFileName(path));
    //     var files = Directory.GetFiles(path, "*.cs", SearchOption.AllDirectories);
    //     foreach (var file in files)
    //     {

    //         new ClassName()
    //     }
    //     directoryController.Add();
    //     return;
    // }

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

public interface IItem
{
}

public class Namespace : IItem
{
    public Namespace(string name)
    {
        Name = name;
    }

    public string Name { get; }
    public HashSet<IItem> Items { get; private set; } = new HashSet<IItem>();

    public void Add(IItem item)
    {
        Items.Add(item);
    }

    public IEnumerable<ClassInfo> GetClasses()
    {
        var namespaceClasses = Items.OfType<Namespace>().SelectMany(v => v.GetClasses());
        var classes = Items.OfType<ClassInfo>();
        return namespaceClasses.Concat(classes);
    }
}

public class ClassInfo : IItem
{
    public ClassInfo(string name)
    {
        Name = name;
        BaseClasses = new List<string>();
        Interfaces = new List<string>();
        Fields = new List<MemberInfo>();
        Properties = new List<Property>();
        Methods = new List<MemberInfo>();
        Dependencies = new HashSet<string>();
    }

    public string Name { get; }
    public List<string> BaseClasses { get; }
    public List<string> Interfaces { get; }
    public List<MemberInfo> Fields { get; }
    public List<Property> Properties { get; }
    public List<MemberInfo> Methods { get; }
    public HashSet<string> Dependencies { get; }
}

public interface IDirectoryModeItem
{

}

public class DirectoryController : IDirectoryModeItem
{
    public DirectoryController(string name)
    {
        Name = name;
    }

    public string Name { get; }
    public HashSet<IDirectoryModeItem> Items { get; private set; } = new HashSet<IDirectoryModeItem>();

    public void Add(IDirectoryModeItem item)
    {
        Items.Add(item);
    }

    // public IEnumerable<ClassInfo> GetClasses()
    // {
    //     var namespaceClasses = Items.OfType<Namespace>().SelectMany(v => v.GetClasses());
    //     var classes = Items.OfType<ClassInfo>();
    //     return namespaceClasses.Concat(classes);
    // }
}


public class ClassName : IDirectoryModeItem
{
    public ClassName(string name, string namespaceName)
    {
        Name = name;
        Namespace = namespaceName;
    }

    public string Name { get; }
    public string Namespace { get; }
}

public class MemberInfo
{
    public MemberInfo(FieldInfo fieldInfo)
    {
        Name = fieldInfo.Name;
        Type = CleanClassName(fieldInfo.FieldType.Name);

        if (fieldInfo.IsPublic)
        {
            Accessibility = new Accessibility(Accessibility.Types.Public);
        }
        else if (fieldInfo.IsAssembly)
        {
            Accessibility = new Accessibility(Accessibility.Types.Internal);
        }
        else if (fieldInfo.IsFamily)
        {
            Accessibility = new Accessibility(Accessibility.Types.Protected);
        }
        else if (fieldInfo.IsPrivate)
        {
            Accessibility = new Accessibility(Accessibility.Types.Private);
        }
        else
        {
            throw new Exception("アクセシビリティが例外です。");
        }
    }

    public MemberInfo(MethodInfo methodInfo)
    {
        Name = methodInfo.Name;
        Type = CleanClassName(methodInfo.ReturnType.Name);

        if (methodInfo.IsPublic)
        {
            Accessibility = new Accessibility(Accessibility.Types.Public);
        }
        else if (methodInfo.IsAssembly)
        {
            Accessibility = new Accessibility(Accessibility.Types.Internal);
        }
        else if (methodInfo.IsFamily)
        {
            Accessibility = new Accessibility(Accessibility.Types.Protected);
        }
        else if (methodInfo.IsPrivate)
        {
            Accessibility = new Accessibility(Accessibility.Types.Private);
        }
        else
        {
            throw new Exception("アクセシビリティが例外です。");
        }
    }

    public string Name { get; }
    public string Type { get; }
    public Accessibility Accessibility { get; }

    private string CleanClassName(string className)
    {
        int backtickIndex = className.IndexOf('`');
        return backtickIndex == -1 ? className : className.Substring(0, backtickIndex);
    }
}

public class Property
{
    public Property(PropertyInfo propertyInfo)
    {
        Name = propertyInfo.Name;
        Type = CleanClassName(propertyInfo.PropertyType.Name);

        CanGet = propertyInfo.CanRead;
        if (CanGet)
        {
            Getter = new MemberInfo(propertyInfo.GetMethod);
        }

        CanSet = propertyInfo.CanWrite;
        if (CanSet)
        {
            Setter = new MemberInfo(propertyInfo.SetMethod);
        }
    }

    public string Name { get; }
    public string Type { get; }
    public bool CanGet { get; }
    public bool CanSet { get; }
    public MemberInfo Getter { get; }
    public MemberInfo Setter { get; }

    public Accessibility Accessibility
    {
        get
        {
            if (CanGet && Getter.Accessibility.Type == Accessibility.Types.Public) return new Accessibility(Accessibility.Types.Public);
            if (CanSet && Setter.Accessibility.Type == Accessibility.Types.Public) return new Accessibility(Accessibility.Types.Public);
            if (CanGet && Getter.Accessibility.Type == Accessibility.Types.Internal) return new Accessibility(Accessibility.Types.Internal);
            if (CanSet && Setter.Accessibility.Type == Accessibility.Types.Internal) return new Accessibility(Accessibility.Types.Internal);
            if (CanGet && Getter.Accessibility.Type == Accessibility.Types.Protected) return new Accessibility(Accessibility.Types.Protected);
            if (CanSet && Setter.Accessibility.Type == Accessibility.Types.Protected) return new Accessibility(Accessibility.Types.Protected);
            return new Accessibility(Accessibility.Types.Private);
        }
    }

    public string GetterAccessibility
    {
        get
        {
            return Getter.Accessibility.Type == Accessibility.Type ? "" : Getter.Accessibility.ToKeyword();
        }
    }

    public string SetterAccessibility
    {
        get
        {
            return Setter.Accessibility.Type == Accessibility.Type ? "" : Setter.Accessibility.ToKeyword();
        }
    }

    private string CleanClassName(string className)
    {
        int backtickIndex = className.IndexOf('`');
        return backtickIndex == -1 ? className : className.Substring(0, backtickIndex);
    }
}

public class Accessibility
{
    public enum Types
    {
        Public,
        Internal,
        Protected,
        Private
    }

    public Types Type { get; }

    public Accessibility(Types type)
    {
        Type = type;
    }

    public string ToSymbol()
    {
        switch (Type)
        {
            case Types.Public:
                return "+";
            case Types.Internal:
                return "~";
            case Types.Protected:
                return "#";
            case Types.Private:
                return "-";
            default:
                throw new ArgumentException("非対応のアクセシビリティです。");
        }
    }

    public string ToKeyword()
    {
        switch (Type)
        {
            case Types.Public:
                return "public";
            case Types.Internal:
                return "internal";
            case Types.Protected:
                return "protected";
            case Types.Private:
                return "private";
            default:
                throw new ArgumentException("非対応のアクセシビリティです。");
        }
    }
}
