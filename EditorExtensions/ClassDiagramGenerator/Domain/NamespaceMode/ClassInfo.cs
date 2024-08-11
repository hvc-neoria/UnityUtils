using System.Collections.Generic;

namespace HvcNeoria.Unity.Utils
{
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

}
