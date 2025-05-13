using UKHO.ADDS.Management.Shell.Models;

namespace UKHO.ADDS.Management.Shell.Services
{
    public class ModulePageService
    {
        private readonly ModulePage[] _allPages =
        {
            new ModulePage { Name = "Services", Path = "/", Icon = "\ue88a" }, new ModulePage
            {
                Name = "Explorer",
                Path = "/_dashboard/explorer",
                Title = "",
                Description = "",
                Icon = "\ue0c6"
            }
        };

        public IEnumerable<ModulePage> Pages => _allPages;

        public ModulePage FindCurrent(Uri uri)
        {
            IEnumerable<ModulePage> Flatten(IEnumerable<ModulePage> e)
            {
                return e.SelectMany(c => c.Children != null ? Flatten(c.Children) : new[] { c });
            }

            return Flatten(Pages)
                .FirstOrDefault(example => example.Path == uri.AbsolutePath || $"/{example.Path}" == uri.AbsolutePath);
        }

        public string TitleFor(ModulePage example)
        {
            if (example != null && example.Name != "Overview")
            {
                return example.Title ?? "";
            }

            return "";
        }

        public string DescriptionFor(ModulePage example) => example?.Description ?? "";
    }
}
