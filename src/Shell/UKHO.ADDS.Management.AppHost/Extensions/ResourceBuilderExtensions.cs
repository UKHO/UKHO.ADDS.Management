using System.Diagnostics;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace UKHO.ADDS.Management.AppHost.Extensions
{
    internal static class ResourceBuilderExtensions
    {
        internal static IResourceBuilder<T> WithKeycloakUi<T>(this IResourceBuilder<T> builder, EndpointReference endpoint, string displayName) where T : IResourceWithEndpoints => builder.WithKeycloakUi(endpoint, displayName, "keycloak-admin-ui");

        internal static IResourceBuilder<T> WithKeycloakUi<T>(this IResourceBuilder<T> builder, EndpointReference endpoint, string displayName, string name)
            where T : IResourceWithEndpoints =>
            builder.WithCommand(
                name,
                displayName,
                executeCommand: async (ExecuteCommandContext context) =>
                {
                    try
                    {
                        var urlBuilder = new UriBuilder(endpoint.Url) { Path = "/" };

                        Process.Start(new ProcessStartInfo(urlBuilder.ToString()) { UseShellExecute = true });

                        return await Task.FromResult(new ExecuteCommandResult { Success = true });
                    }
                    catch (Exception e)
                    {
                        return new ExecuteCommandResult { Success = false, ErrorMessage = e.ToString() };
                    }
                },
                commandOptions: new CommandOptions
                {
                    UpdateState = (UpdateCommandStateContext context) =>
                    {
                        return context.ResourceSnapshot.HealthStatus == HealthStatus.Healthy ? ResourceCommandState.Enabled : ResourceCommandState.Disabled;
                    },
                    IconName = "Document",
                    IconVariant = IconVariant.Filled
                });

        internal static IResourceBuilder<T> WithShell<T>(this IResourceBuilder<T> builder, string displayName) where T : IResourceWithEndpoints => builder.WithShell(displayName, "adds-management-shell");

        internal static IResourceBuilder<T> WithShell<T>(this IResourceBuilder<T> builder, string displayName, string name)
            where T : IResourceWithEndpoints =>
            builder.WithCommand(
                name,
                displayName,
                async context =>
                {
                    try
                    {
                        var endpoint = builder.GetEndpoint("http");
                        var url = $"{endpoint.Url}";

                        Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });

                        return await Task.FromResult(new ExecuteCommandResult { Success = true });
                    }
                    catch (Exception e)
                    {
                        return new ExecuteCommandResult { Success = false, ErrorMessage = e.ToString() };
                    }
                },
                new CommandOptions { UpdateState = context => { return context.ResourceSnapshot.HealthStatus == HealthStatus.Healthy ? ResourceCommandState.Enabled : ResourceCommandState.Disabled; }, IconName = "Document", IconVariant = IconVariant.Filled });

        internal static IResourceBuilder<T> WithMockUi<T>(this IResourceBuilder<T> builder, string displayName) where T : IResourceWithEndpoints => builder.WithMockUi(displayName, "adds-mock-dashboard");

        internal static IResourceBuilder<T> WithMockUi<T>(this IResourceBuilder<T> builder, string displayName, string name)
            where T : IResourceWithEndpoints =>
            builder.WithCommand(
                name,
                displayName,
                async context =>
                {
                    try
                    {
                        var endpoint = builder.GetEndpoint("https");
                        var url = $"{endpoint.Url}";

                        Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });

                        return await Task.FromResult(new ExecuteCommandResult { Success = true });
                    }
                    catch (Exception e)
                    {
                        return new ExecuteCommandResult { Success = false, ErrorMessage = e.ToString() };
                    }
                },
                new CommandOptions
                {
                    UpdateState = context => context.ResourceSnapshot.HealthStatus == HealthStatus.Healthy ? ResourceCommandState.Enabled : ResourceCommandState.Disabled,
                    IconName = "Document",
                    IconVariant = IconVariant.Filled
                });
    }
}
