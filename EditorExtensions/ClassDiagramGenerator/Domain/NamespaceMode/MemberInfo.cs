
using System;
using System.Reflection;

namespace HvcNeoria.Unity.Utils
{
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
}
