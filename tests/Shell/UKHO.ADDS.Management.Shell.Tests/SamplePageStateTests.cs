using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using UKHO.ADDS.Management.Modules.Samples.Pages;
using UKHO.ADDS.Management.Shell.Configuration;
using Xunit;

namespace UKHO.ADDS.Management.Shell.Tests;

public class SamplePageStateTests
{
    [Fact]
    public void BindOptions_ReturnsErrorWhenSectionMissing()
    {
        var configuration = new ConfigurationBuilder().Build();
        var provider = new ModuleConfigurationProvider(configuration, NullLogger<ModuleConfigurationProvider>.Instance);

        var (_, error) = SamplePageState.BindOptions(provider, "dev", "Samples");

        Assert.Equal("Missing configuration for deployment 'dev' and module 'Samples'.", error);
    }

    [Fact]
    public void BindOptions_ReturnsErrorWhenBaseUrlMissing()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["Deployments:dev:Modules:Samples:DisplayName"] = "Samples (Dev)",
                ["Deployments:dev:Modules:Samples:TimeoutSeconds"] = "30"
            })
            .Build();

        var provider = new ModuleConfigurationProvider(configuration, NullLogger<ModuleConfigurationProvider>.Instance);

        var (_, error) = SamplePageState.BindOptions(provider, "dev", "Samples");

        Assert.Equal("Invalid configuration for deployment 'dev' and module 'Samples': BaseUrl is required.", error);
    }

    [Fact]
    public void BindOptions_ReturnsNoErrorWhenBaseUrlPresent()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["Deployments:dev:Modules:Samples:BaseUrl"] = "https://dev.api.example.com/",
                ["Deployments:dev:Modules:Samples:DisplayName"] = "Samples (Dev)",
                ["Deployments:dev:Modules:Samples:TimeoutSeconds"] = "30"
            })
            .Build();

        var provider = new ModuleConfigurationProvider(configuration, NullLogger<ModuleConfigurationProvider>.Instance);

        var (options, error) = SamplePageState.BindOptions(provider, "dev", "Samples");

        Assert.Null(error);
        Assert.Equal("https://dev.api.example.com/", options.BaseUrl);
    }
}
