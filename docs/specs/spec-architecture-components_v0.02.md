Title: ADDS Management Architecture Components
Version: v0.02 (Draft)
Status: Draft / Baseline Extraction (Adjusted Scope)
Supersedes: spec-architecture-components_v0.01.md
Change Log:
- v0.02: Removed internal ADDS Mock domain/component detail per updated scope.

1. Scope / Purpose
- Enumerate core architectural components excluding mock internals.

2. Context & Overview
- Architecture centers on Blazor Server shell, modular extensibility, and Aspire-enabled local infra.
- Mock services exist but excluded from component internals.

3. Components (In-Scope)
UI Shell:
- Program (Host): Auth, OpenAPI, Radzen UI, OutputCache, Blazor server components.
- Module system: IModule + ModulePage (+ section, keyboard shortcut metadata classes).
- ModulePageService: Aggregates module pages + static pages (Services, Explorer).

Extensibility:
- SampleModule: Example implementation of IModule (singleton) providing a Sample page.

Cross-Cutting / Shared:
- ServiceDefaults (Extensions.cs): OpenTelemetry setup (metrics/tracing), health checks, service discovery, HTTP resilience.
- ServiceNames: Central service identifiers consumed by AppHost.

Infrastructure Orchestration:
- AppHost Program: Defines resources (Keycloak, KeyVault emulator, Storage emulator) and wires dependencies.
- ResourceBuilderExtensions: Provides convenience commands (WithShell, WithKeycloakUi, WithMockUi) — mock UI command retained only as reference (behavior not detailed).

4. Detailed Elements
Dependency Injection Patterns:
- Singleton: SampleModule (IModule).
- Scoped: ModulePageService.
- Transient: AuthorizationHandler.
- Scoped HttpClient (per navigation base address) for Host.

Routing & Composition:
- AppRouter includes AdditionalAssemblies for module discovery.
- Authentication endpoints grouped under /authentication.

Resilience & Telemetry:
- Standard resilience handler automatically applied to HttpClient.
- OpenTelemetry instrumentation: AspNetCore, HttpClient, Runtime metrics; traces for AspNetCore + HttpClient + source.
- Conditional OTLP exporter via environment variable.

Security:
- OIDC (Keycloak): ClientId ADDSManagementShell; code flow; scope addsmanagement:all; tokens saved; cookie sign-in scheme.
- Authorization registration without custom policies (gap).
- AuthorizationHandler adds bearer token to outgoing HttpClient requests.

Configuration & Parameters:
- AppHost uses builder.AddParameter for Keycloak credentials.
- ServiceNames constants unify resource naming.

Health:
- Health checks mapped in development (/health, /alive) via MapDefaultEndpoints.

5. Cross-Cutting Concerns
Logging:
- Standard Microsoft logging + OpenTelemetry exporter; no explicit Serilog in in-scope components.
Error Handling:
- No centralized exception middleware (gap).
Caching:
- OutputCache enabled; no policies enumerated (gap).
Serialization:
- Default JSON (no custom source generation) (gap).
Security:
- Authentication configured; fine-grained authorization unspecified.
Accessibility:
- Not addressed (gap).
Configuration Hygiene:
- Central constants; deeper environment layering unverified.

6. Non-Functional Characteristics
- Observability: Telemetry + metrics baseline.
- Reliability: Health checks for readiness/liveness in dev.
- Maintainability: Modular page injection via IModule promotes separation.
- Scalability: Aspire orchestrates resources; production scale strategy unspecified.

7. Gaps & Unknowns
- Lack of tests (unit/integration) for module system or auth pipeline.
- Absent error handling and authorization policies.
- OutputCache & resilience configuration not tuned/documented.
- No accessibility or localization strategy in shell.
- No configuration validation pattern (e.g., options validation) observed.

8. Future Indicators
- No TODO markers in in-scope files.

9. Traceability (Representative)
- Auth & Program: src/Shell/UKHO.ADDS.Management.Host/Program.cs
- Auth endpoints: src/Shell/UKHO.ADDS.Management.Host/Extensions/LoginLogoutEndpointRouteBuilderExtensions.cs
- AuthorizationHandler: src/Shell/UKHO.ADDS.Management.Host/Extensions/AuthorizationHandler.cs
- Module abstractions: src/Shell/UKHO.ADDS.Management.Shell/Modules/*.cs
- Module service: src/Shell/UKHO.ADDS.Management.Shell/Services/ModulePageService.cs
- Sample module: src/Modules/UKHO.ADDS.Management.Modules.Samples/*.cs
- Service defaults: src/Shell/UKHO.ADDS.Management.ServiceDefaults/Extensions.cs
- AppHost orchestration: src/Shell/UKHO.ADDS.Management.AppHost/Program.cs
- Resource commands: src/Shell/UKHO.ADDS.Management.AppHost/Extensions/ResourceBuilderExtensions.cs

10. Cross-References
- System overview spec v0.02 for holistic context.
- API functional spec v0.02 for endpoints.
- Frontend functional spec v0.02 for component inventory.
- Infra deployment spec v0.02 for resource details.

11. Completion Checklist
- Mock internals excluded.
- Gaps recorded.

End of Document.
