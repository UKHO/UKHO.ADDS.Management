using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace UKHO.ADDS.Management.Shell.Configuration
{
    public class ModuleConfigurationProvider : IModuleConfigurationProvider
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<ModuleConfigurationProvider> logger;

        public ModuleConfigurationProvider(IConfiguration configuration, ILogger<ModuleConfigurationProvider> logger)
        {
            this.configuration = configuration;
            this.logger = logger;
        }

        public IConfigurationSection GetSection(string deploymentId, string moduleId)
        {
            if (string.IsNullOrWhiteSpace(deploymentId) || string.IsNullOrWhiteSpace(moduleId))
            {
                logger.LogWarning("GetSection called with empty deploymentId or moduleId (deploymentId={DeploymentId}, moduleId={ModuleId})", deploymentId, moduleId);
                return configuration.GetSection("");
            }

            var path = $"Deployments:{deploymentId}:Modules:{moduleId}";
            var section = configuration.GetSection(path);
            if (!section.Exists())
            {
                logger.LogWarning("Configuration section not found for path {Path}", path);
            }
            return section;
        }

        public TOptions GetOptions<TOptions>(string deploymentId, string moduleId) where TOptions : class, new()
        {
            var section = GetSection(deploymentId, moduleId);
            if (!section.Exists())
            {
                logger.LogWarning("Returning default options for missing section Deployments:{DeploymentId}:Modules:{ModuleId}", deploymentId, moduleId);
                return new TOptions();
            }

            try
            {
                var options = section.Get<TOptions>();
                if (options == null)
                {
                    logger.LogWarning("Binding returned null for options type {Type} at path Deployments:{DeploymentId}:Modules:{ModuleId}", typeof(TOptions).FullName, deploymentId, moduleId);
                    return new TOptions();
                }
                return options;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error binding options of type {Type} for deployment {DeploymentId}, module {ModuleId}", typeof(TOptions).FullName, deploymentId, moduleId);
                return new TOptions();
            }
        }
    }
}
