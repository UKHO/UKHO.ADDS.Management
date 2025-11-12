using UKHO.ADDS.Management.Shell.Modules;

namespace UKHO.ADDS.Management.Modules.Samples
{
    public class SampleModule : IModule
    {
        public string Id => "sample-module";

        public IEnumerable<ModulePage> Pages => [new()
        {
            Name = "Sample",
            Path = "/sample/main",
            Icon = "\ue88a",
            Children = [new(){Name = "sub page", Path="/sample/sub"}]
        }];
    }
}
