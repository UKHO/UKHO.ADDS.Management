using System.Threading.Tasks;
using System.Threading;
using Microsoft.JSInterop;
using UKHO.ADDS.Management.Shell.Services.Storage;
using Xunit;

namespace UKHO.ADDS.Management.Shell.Tests;

public class DeploymentSelectionStorageTests
{
    [Fact]
    public async Task GetAsync_InvokesExpectedJsFunction()
    {
        var js = new FakeJsRuntime();
        var storage = new DeploymentSelectionStorage(js);

        await storage.GetAsync("Samples");

        Assert.Equal("adds.deploymentSelection.get", js.LastIdentifier);
        Assert.Equal(new object[] { "Samples" }, js.LastArgs);
    }

    [Fact]
    public async Task SetAsync_InvokesExpectedJsFunction()
    {
        var js = new FakeJsRuntime();
        var storage = new DeploymentSelectionStorage(js);

        await storage.SetAsync("Samples", "dev");

        Assert.Equal("adds.deploymentSelection.set", js.LastIdentifier);
        Assert.Equal(new object[] { "Samples", "dev" }, js.LastArgs);
    }

    private sealed class FakeJsRuntime : IJSRuntime
    {
        public string? LastIdentifier { get; private set; }
        public object[]? LastArgs { get; private set; }

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args)
        {
            LastIdentifier = identifier;
            LastArgs = args is null ? null : System.Array.ConvertAll(args, x => x!);
            return default;
        }

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[]? args)
        {
            LastIdentifier = identifier;
            LastArgs = args is null ? null : System.Array.ConvertAll(args, x => x!);
            return default;
        }
    }
}
