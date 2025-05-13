using UKHO.ADDS.Management.LocalHost.Extensions;

namespace UKHO.ADDS.Management.LocalHost;

internal static class Program
{
    private static void Main(string[] args)
    {
        var builder = DistributedApplication.CreateBuilder(args);

        var username = builder.AddParameter("username");
        var password = builder.AddParameter("password", secret: true);

        var keycloak = builder.AddKeycloak("keycloak", 8080, username, password)
            .WithDataVolume()
            .WithRealmImport("./Realms");

        var sqlserver = builder.AddSqlServer("sql-server")
            .WithVolume("adds-mgmt-sqlserver-data", "/var/opt/mssql")
            .AddDatabase("adds-management");

        builder.AddProject<Projects.UKHO_ADDS_Management_Host>("management-shell")
            .WithExternalHttpEndpoints()
            .WithReference(keycloak)
            .WithReference(sqlserver)
            .WaitFor(sqlserver)
            .WithShell("ADDS Management")
            .WithKeycloakUi(keycloak.GetEndpoint("http"), "Keycloak UI");

        builder.Build().Run();
    }
}