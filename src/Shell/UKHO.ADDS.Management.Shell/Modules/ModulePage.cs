namespace UKHO.ADDS.Management.Shell.Modules
{
    public class ModulePage
    {
        public bool New { get; set; }
        public bool Updated { get; set; }
        public bool Pro { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Path { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Expanded { get; set; }
        public IEnumerable<ModulePage> Children { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public IEnumerable<ModulePageSection> Toc { get; set; }
    }
}
