namespace HvcNeoria.Unity.Utils
{
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
}
