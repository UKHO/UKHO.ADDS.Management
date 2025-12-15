using Microsoft.Extensions.Configuration;

namespace UKHO.ADDS.Management.Shell.Configuration
{
    public interface IModuleConfigurationProvider
    {
        IConfigurationSection GetSection(string deploymentId, string moduleId);
        TOptions GetOptions<TOptions>(string deploymentId, string moduleId) where TOptions : class, new();
    }
}
