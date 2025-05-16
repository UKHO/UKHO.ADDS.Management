using AzureKeyVaultEmulator.Aspire.Hosting;
using Projects;
using UKHO.ADDS.Management.Configuration;
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

        var cosmos = builder.AddAzureCosmosDB(ServiceNames.CosmosDb).RunAsEmulator(x => x.WithDataVolume());
        cosmos.AddCosmosDatabase("adds-management");

        var keyVault = builder.AddAzureKeyVaultEmulator(ServiceNames.KeyVault,
            new KeyVaultEmulatorOptions
            {
                Persist = true
            });

        var storage = builder.AddAzureStorage(ServiceNames.Storage).RunAsEmulator(e => { e.WithDataVolume(); });

        var storageQueue = storage.AddQueues(ServiceNames.Queues);
        var storageTable = storage.AddTables(ServiceNames.Tables);
        var storageBlob = storage.AddBlobs(ServiceNames.Blobs);

        // ADDS Mock
        var mockService = builder.AddProject<UKHO_ADDS_Mocks_Management>(ServiceNames.Mocks)
            .WithMockUi("Mock UI");

        builder.AddProject<Projects.UKHO_ADDS_Management_Host>("management-shell")
            .WithExternalHttpEndpoints()
            .WithReference(keycloak)
            .WithReference(cosmos)
            .WaitFor(cosmos)
            .WithReference(keyVault)
            .WaitFor(keyVault)
            .WithReference(storageQueue)
            .WaitFor(storageQueue)
            .WithReference(storageTable)
            .WaitFor(storageTable)
            .WithReference(storageBlob)
            .WaitFor(storageBlob)
            .WithShell("ADDS Management")
            .WithKeycloakUi(keycloak.GetEndpoint("http"), "Keycloak UI");

        builder.Build().Run();
    }
}