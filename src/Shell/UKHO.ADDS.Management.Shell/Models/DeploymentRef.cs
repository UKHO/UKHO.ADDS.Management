namespace UKHO.ADDS.Management.Shell.Models
{
    public class DeploymentRef
    {
        public string Id { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class DeploymentsLoadResult
    {
        public IReadOnlyList<DeploymentRef> Items { get; init; } = Array.Empty<DeploymentRef>();
        public string? ErrorMessage { get; init; }
        public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);
        public static DeploymentsLoadResult Success(IEnumerable<DeploymentRef> items) => new() { Items = items.ToList() };
        public static DeploymentsLoadResult Error(string message) => new() { ErrorMessage = message };
    }
}
