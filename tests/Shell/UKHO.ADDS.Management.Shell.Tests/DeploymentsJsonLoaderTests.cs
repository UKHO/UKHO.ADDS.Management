using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging.Abstractions;
using UKHO.ADDS.Management.Shell.Services;
using Xunit;

namespace UKHO.ADDS.Management.Shell.Tests;

public class DeploymentsJsonLoaderTests
{
    [Fact]
    public async Task LoadForModuleAsync_WhenFileMissing_ShouldReturnError()
    {
        var tempRoot = CreateTempRoot();
        var env = new StubEnv(tempRoot);
        var loader = new DeploymentsJsonLoader(env, NullLogger<DeploymentsJsonLoader>.Instance);

        var result = await loader.LoadForModuleAsync("UKHO.ADDS.Management.Modules.Samples");

        Assert.True(result.HasError);
    }

    [Fact]
    public async Task LoadForModuleAsync_WhenJsonInvalid_ShouldReturnError()
    {
        var tempRoot = CreateTempRoot();
        var env = new StubEnv(tempRoot);

        var moduleFolder = Path.Combine(tempRoot, "src", "Modules", "UKHO.ADDS.Management.Modules.Samples");
        Directory.CreateDirectory(moduleFolder);
        await File.WriteAllTextAsync(Path.Combine(moduleFolder, "deployments.json"), "{ not-json");

        var loader = new DeploymentsJsonLoader(env, NullLogger<DeploymentsJsonLoader>.Instance);

        var result = await loader.LoadForModuleAsync("UKHO.ADDS.Management.Modules.Samples");

        Assert.True(result.HasError);
    }

    [Fact]
    public async Task LoadForModuleAsync_WhenFileEmpty_ShouldReturnError()
    {
        var tempRoot = CreateTempRoot();
        var env = new StubEnv(tempRoot);

        var moduleFolder = Path.Combine(tempRoot, "src", "Modules", "UKHO.ADDS.Management.Modules.Samples");
        Directory.CreateDirectory(moduleFolder);
        await File.WriteAllTextAsync(Path.Combine(moduleFolder, "deployments.json"), "[]");

        var loader = new DeploymentsJsonLoader(env, NullLogger<DeploymentsJsonLoader>.Instance);

        var result = await loader.LoadForModuleAsync("UKHO.ADDS.Management.Modules.Samples");

        Assert.True(result.HasError);
    }

    private static string CreateTempRoot()
    {
        var path = Path.Combine(Path.GetTempPath(), "adds-mgmt-tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(path);
        return path;
    }

    private sealed class StubEnv : IWebHostEnvironment
    {
        public StubEnv(string contentRootPath) => ContentRootPath = contentRootPath;

        public string ApplicationName { get; set; } = "test";
        public IFileProvider WebRootFileProvider { get; set; } = null!;
        public string WebRootPath { get; set; } = string.Empty;
        public string EnvironmentName { get; set; } = "Development";
        public string ContentRootPath { get; set; }
        public IFileProvider ContentRootFileProvider { get; set; } = null!;
    }
}
