namespace UKHO.ADDS.Management.Shell.Modules
{
    public interface IModule
    {
        public string Id { get; }

        public IEnumerable<ModulePage> Pages { get; }
    }
}
