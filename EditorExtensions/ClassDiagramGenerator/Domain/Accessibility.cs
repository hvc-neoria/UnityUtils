using System;

namespace HvcNeoria.Unity.Utils
{
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
}
