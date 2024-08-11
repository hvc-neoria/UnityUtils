using System.Collections.Generic;

namespace HvcNeoria.Unity.Utils
{
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


}
