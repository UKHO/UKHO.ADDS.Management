using UKHO.ADDS.Management.Shell.Models;
using UKHO.ADDS.Management.Shell.Modules;

namespace UKHO.ADDS.Management.Modules.Samples
{
    public class SampleModule : IModule
    {
        public IEnumerable<ModulePage> Pages => [new ModulePage { Name = "Sample", Path = "/sample/main", Icon = "\ue88a" }];
    }
}
