using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using UKHO.ADDS.Management.Shell.Configuration;
using Xunit;

namespace UKHO.ADDS.Management.Shell.Tests;

public class ModuleConfigurationProviderTests
{
    [Fact]
    public void GetSection_ReturnsSectionWhenPresent()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["Deployments:dev:Modules:Samples:BaseUrl"] = "https://dev.example.com",
                ["Deployments:dev:Modules:Samples:TimeoutSeconds"] = "30"
            })
            .Build();

        var provider = BuildProvider(configuration);

        var section = provider.GetSection("dev", "Samples");

        Assert.True(section.Exists());
        Assert.Equal("https://dev.example.com", section["BaseUrl"]);
        Assert.Equal("30", section["TimeoutSeconds"]);
    }

    [Fact]
    public void GetOptions_BindsConfiguredValues()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["Deployments:prod:Modules:Samples:BaseUrl"] = "https://prod.example.com",
                ["Deployments:prod:Modules:Samples:TimeoutSeconds"] = "45"
            })
            .Build();

        var provider = BuildProvider(configuration);

        var options = provider.GetOptions<TestOptions>("prod", "Samples");

        Assert.Equal("https://prod.example.com", options.BaseUrl);
        Assert.Equal(45, options.TimeoutSeconds);
    }

    [Fact]
    public void GetOptions_ReturnsDefaultWhenSectionMissing()
    {
        var configuration = new ConfigurationBuilder().Build();

        var provider = BuildProvider(configuration);

        var options = provider.GetOptions<TestOptions>("dev", "Samples");

        Assert.Null(options.BaseUrl);
        Assert.Equal(default, options.TimeoutSeconds);
    }

    [Fact]
    public void GetOptions_ReturnsDefaultWhenBindingThrows()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["Deployments:dev:Modules:Samples:TimeoutSeconds"] = "not-an-int"
            })
            .Build();

        var provider = BuildProvider(configuration);

        var options = provider.GetOptions<TestOptions>("dev", "Samples");

        Assert.Null(options.BaseUrl);
        Assert.Equal(default, options.TimeoutSeconds);
    }

    private static ModuleConfigurationProvider BuildProvider(IConfiguration configuration)
    {
        return new ModuleConfigurationProvider(configuration, NullLogger<ModuleConfigurationProvider>.Instance);
    }

    private class TestOptions
    {
        public string? BaseUrl { get; set; }
        public int TimeoutSeconds { get; set; }
    }
}
