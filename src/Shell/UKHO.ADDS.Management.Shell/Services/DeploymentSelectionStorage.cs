using Microsoft.JSInterop;

namespace UKHO.ADDS.Management.Shell.Services.Storage
{
    public class DeploymentSelectionStorage
    {
        private readonly IJSRuntime js;

        public DeploymentSelectionStorage(IJSRuntime js)
        {
            this.js = js;
        }

        public ValueTask SetAsync(string moduleId, string deploymentId)
        {
            return js.InvokeVoidAsync("adds.deploymentSelection.set", moduleId, deploymentId);
        }

        public ValueTask<string?> GetAsync(string moduleId)
        {
            return js.InvokeAsync<string?>("adds.deploymentSelection.get", moduleId);
        }

        private static string StorageKey(string moduleId) => $"adds:module:{moduleId}:deployment";
    }
}
