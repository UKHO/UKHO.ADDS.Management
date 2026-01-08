# Configuration JSON guidance

The central `configuration.json` file in the shell host and every module's `deployments.json` are intended to store openly shared topology and deployment metadata. These files should never contain secrets such as credentials, connection strings, or API tokens. Use secure stores (for example Azure Key Vault or environment-specific secrets solutions) for any sensitive configuration values instead.
