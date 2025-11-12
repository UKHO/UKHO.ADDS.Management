using UKHO.ADDS.Management.Shell.Models;

namespace UKHO.ADDS.Management.Shell.Modules
{
    public interface IModule
    {
        public IEnumerable<ModulePage> Pages { get; }
    }
}
