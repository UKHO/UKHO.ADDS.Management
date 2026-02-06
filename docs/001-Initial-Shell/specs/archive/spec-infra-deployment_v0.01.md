Title: ADDS Management Infrastructure & Deployment Specification
Version: v0.01 (Draft)
Status: Draft / Baseline Extraction
Supersedes: None
Change Log:
- Initial draft creation

1. Scope / Purpose
- Document current infrastructure orchestration (Aspire AppHost) and resource composition for local development.

2. Context & Overview
- Utilizes .NET Aspire (DistributedApplication builder) via AppHost project.
- Local emulation of Azure services (KeyVault emulator, Azure Storage emulator) and embedded Keycloak identity provider.
- Adds project references with shell & mock dashboards through custom resource commands.

3. Components / Modules (Infra)
Aspire Resources (Program.cs):
- Parameters: username, password (Keycloak initialization credentials).
- Keycloak resource with data volume & realm import from ./Realms path.
- AzureKeyVaultEmulator resource (ServiceNames.KeyVault) with persistence enabled.
- Azure Storage emulator (ServiceNames.Storage) -> sub-resources: Queues, Tables, Blobs (ServiceNames constants).
- Mock service project resource (ServiceNames.Mocks) with Mock UI command.
- Management shell project resource ("management-shell") with external HTTP endpoints, references to Keycloak & storage resources & shell UI command.

4. Detailed Elements
Resource Command Extensions:
- WithKeycloakUi: Opens admin UI in browser when resource is healthy.
- WithShell: Opens shell site via HTTP endpoint.
- WithMockUi: Opens mock dashboard via HTTPS endpoint.

Dependencies & Ordering:
- Shell waits for keyVault, queues, tables, blobs (explicit WaitFor chaining).
- Keycloak realm import ensures identity realm pre-configured before shell usage.

Configuration & Naming:
- Service names centrally defined in ServiceNames.cs for consistency.

Health & Status:
- Resource commands enabled only when HealthStatus == Healthy.
- Health instrumentation for services provided via ServiceDefaults (OpenTelemetry + health checks) (development environment).

Observability & Telemetry:
- OpenTelemetry instrumentation in ServiceDefaults; OTLP exporter conditional on environment variable for services referencing ServiceDefaults.
- Serilog for mocks (console + OTLP export).

Security & Identity:
- Keycloak provides OIDC server; shell references its endpoint for authentication flows.
- No explicit mention of secrets management besides KeyVault emulator (Unverified secret usage).

Deployment Model:
- Focus appears to be local development orchestration (emulators + identity provider) rather than production deployment scripts.
- No Bicep/AZD/Terraform files present (Unverified presence externally).

5. Cross-Cutting Infra Concerns
- Service discovery configured (ServiceDefaults) enabling HttpClient service name resolution.
- Resilience defaults applied via AddStandardResilienceHandler.
- Health endpoints only enabled in Development environment (MapDefaultEndpoints logic).

6. Non-Functional Characteristics
- Local developer productivity: one AppHost to start all dependent services.
- Scalability: Emulators not representative of production scaling (Unverified production approach).
- Reliability: WaitFor ensures dependency readiness ordering.
- Security: Local credentials for Keycloak; KeyVault emulator for secret placeholders.

7. Gaps & Unknowns
- No production IaC artifacts (Bicep/Terraform) in repository (Unverified if external).
- No containerization definitions (Dockerfiles) shown (Unverified).
- No environment promotion strategy documented.
- Secrets management strategy beyond emulator unclear.
- Monitoring/alerting configuration absent.

8. Future Indicators
- No TODO markers referencing infra.

9. Traceability
- AppHost orchestration: src/Shell/UKHO.ADDS.Management.AppHost/Program.cs
- Resource command helpers: src/Shell/UKHO.ADDS.Management.AppHost/Extensions/ResourceBuilderExtensions.cs
- Service name constants: src/Shell/UKHO.ADDS.Management/Configuration/ServiceNames.cs
- Service defaults (telemetry, health, resilience): src/Shell/UKHO.ADDS.Management.ServiceDefaults/Extensions.cs
- Mock server runtime: mock/repo/src/UKHO.ADDS.Mocks/MockServer.cs

10. Cross-References
- System overview spec for holistic architecture.
- Architecture components spec for cross-cutting services.

11. Completion Checklist
- Infra resources enumerated.
- Gaps highlighted.
- No future design invented.

End of Document.
