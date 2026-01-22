using System.Text.Json;
using UKHO.ADDS.Management.Shell.Models;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace UKHO.ADDS.Management.Shell.Services
{
    public class DeploymentsJsonLoader
    {
        private readonly IWebHostEnvironment env;
        private readonly ILogger<DeploymentsJsonLoader> logger;

        public DeploymentsJsonLoader(IWebHostEnvironment env, ILogger<DeploymentsJsonLoader> logger)
        {
            this.env = env;
            this.logger = logger;
        }

        public async Task<DeploymentsLoadResult> LoadForModuleAsync(string moduleProjectFolder)
        {
            try
            {
                var basePath = env.ContentRootPath;
                var path = Path.Combine(basePath, "src", "Modules", moduleProjectFolder, "deployments.json");
                if (!File.Exists(path))
                {
                    var msg = $"Required file 'deployments.json' not found for module '{moduleProjectFolder}' at '{path}'.";
                    logger.LogWarning(msg);
                    return DeploymentsLoadResult.Error(msg);
                }

                using var stream = File.OpenRead(path);
                var items = await JsonSerializer.DeserializeAsync<List<DeploymentRef>>(stream) ?? new List<DeploymentRef>();
                if (items.Count == 0)
                {
                    var msg = $"Required file 'deployments.json' is empty for module '{moduleProjectFolder}'.";
                    logger.LogWarning(msg);
                    return DeploymentsLoadResult.Error(msg);
                }
                return DeploymentsLoadResult.Success(items);
            }
            catch (JsonException jex)
            {
                var msg = $"Invalid JSON in required file 'deployments.json' for module '{moduleProjectFolder}': {jex.Message}";
                logger.LogError(jex, msg);
                return DeploymentsLoadResult.Error(msg);
            }
            catch (Exception ex)
            {
                var msg = $"Error loading deployments.json for module {moduleProjectFolder}: {ex.Message}";
                logger.LogError(ex, msg);
                return DeploymentsLoadResult.Error(msg);
            }
        }
    }
}
