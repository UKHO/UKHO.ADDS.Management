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
            var key = StorageKey(moduleId);
            return js.InvokeVoidAsync("localStorage.setItem", key, deploymentId);
        }

        public ValueTask<string?> GetAsync(string moduleId)
        {
            var key = StorageKey(moduleId);
            return js.InvokeAsync<string?>("localStorage.getItem", key);
        }

        private static string StorageKey(string moduleId) => $"adds:module:{moduleId}:deployment";
    }
}
