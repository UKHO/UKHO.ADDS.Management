using Microsoft.Extensions.DependencyInjection;
using UKHO.ADDS.Management.Shell.Modules;

namespace UKHO.ADDS.Management.Modules.Samples.Registration;

public static class ModuleRegistration
{
    public static IServiceCollection AddSampleModule(this IServiceCollection collection)
    {
        collection.AddSingleton<IModule>(new SampleModule());

        return collection;
    }
}
