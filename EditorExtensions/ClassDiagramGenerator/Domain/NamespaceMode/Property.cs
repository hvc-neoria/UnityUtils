using System.Reflection;

namespace HvcNeoria.Unity.Utils
{
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
}
