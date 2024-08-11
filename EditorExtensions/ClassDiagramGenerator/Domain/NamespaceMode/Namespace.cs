using System.Collections.Generic;
using System.Linq;

namespace HvcNeoria.Unity.Utils
{
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

}
