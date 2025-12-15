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

        public async Task<IEnumerable<DeploymentRef>> LoadForModuleAsync(string moduleName)
        {
            try
            {
                // Assume deployments.json is at modules folder under project root for simplicity
                var basePath = env.ContentRootPath;
                var path = Path.Combine(basePath, "src", "Modules", moduleName, "deployments.json");
                if (!File.Exists(path))
                {
                    logger.LogWarning("deployments.json not found for module {Module} at {Path}", moduleName, path);
                    return Enumerable.Empty<DeploymentRef>();
                }

                using var stream = File.OpenRead(path);
                var items = await JsonSerializer.DeserializeAsync<List<DeploymentRef>>(stream) ?? new List<DeploymentRef>();
                return items;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error loading deployments.json for module {Module}", moduleName);
                return Enumerable.Empty<DeploymentRef>();
            }
        }
    }
}
