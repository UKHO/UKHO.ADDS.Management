using UKHO.ADDS.Management.Shell.Modules;

namespace UKHO.ADDS.Management.Shell.Services
{
    public class ModulePageService
    {
        public ModulePageService(IEnumerable<IModule> modules)
        {
            _allPages = new List<ModulePage>();

            foreach (var module in modules)
            {
                _allPages.AddRange(module.Pages);
            }

            _allPages.Add(new ModulePage { Name = "Services", Path = "/", Icon = "\ue88a" });
            _allPages.Add(new ModulePage
            {
                Name = "Explorer",
                Path = "/_dashboard/explorer",
                Title = "",
                Description = "",
                Icon = "\ue0c6"
            });
        }

        private readonly List<ModulePage> _allPages;

        public IEnumerable<ModulePage> Pages => _allPages;

        public ModulePage? FindCurrent(Uri uri)
        {
            IEnumerable<ModulePage> Flatten(IEnumerable<ModulePage> e)
            {
                return e.SelectMany(c => c.Children != null ? Flatten(c.Children) : new[] { c });
            }

            return Flatten(Pages)
                .FirstOrDefault(example => example.Path == uri.AbsolutePath || $"/{example.Path}" == uri.AbsolutePath);
        }

        public string TitleFor(ModulePage? example)
        {
            if (example != null && example.Name != "Overview")
            {
                return example.Title ?? "";
            }

            return "";
        }

        public string DescriptionFor(ModulePage? example) => example?.Description ?? "";
    }
}
