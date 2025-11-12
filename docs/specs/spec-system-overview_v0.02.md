Title: ADDS Management System Overview
Version: v0.02 (Draft)
Status: Draft / Baseline Extraction (Adjusted Scope)
Supersedes: spec-system-overview_v0.01.md
Change Log:
- v0.02: Removed detailed ADDS Mock specifications per updated scope; retained high-level references only.

1. Scope / Purpose
- Provide high-level baseline of projects excluding detailed mock (mock/*) internals.

2. Context & Overview
- Solution targets .NET 9.
- Core areas in scope: Management Shell (Blazor Server), Service Defaults (cross-cutting infra), AppHost (Aspire orchestration), Sample Module.
- ADDS Mock services exist for local development but are out-of-scope for specification detail (only referenced).
- Authentication: Keycloak OIDC (OpenIdConnect + Cookies).
- UI: Blazor Server + Radzen.

3. Components / Modules (Summary)
- Shell Host: src/Shell/UKHO.ADDS.Management.Host/ (Blazor host, auth, routing).
- Shell Core: src/Shell/UKHO.ADDS.Management.Shell/ (Module abstractions, page catalog service).
- Samples Module: src/Modules/UKHO.ADDS.Management.Modules.Samples/ (Example extensibility entry point).
- Service Defaults: src/Shell/UKHO.ADDS.Management.ServiceDefaults/ (Telemetry, resilience, health checks, service discovery).
- Configuration: src/Shell/UKHO.ADDS.Management/Configuration/ServiceNames.cs.
- AppHost: src/Shell/UKHO.ADDS.Management.AppHost/ (Aspire resource orchestration).
- Mocks (Out-of-scope details): mock/ (Referenced only for existence).

4. Detailed Elements (High-Level)
Projects (.csproj paths):
- In-scope project list unchanged from v0.01; mock projects retained only as names without internals.
Architecture Layers (In-scope):
- UI Layer: Blazor Server host + modules.
- Cross-Cutting Layer: ServiceDefaults (telemetry/resilience), configuration constants.
- Infra Layer: AppHost orchestrated resources (Keycloak, emulated Azure services).
- Extensibility Layer: Modules via IModule.
(Mocks layer excluded from detail).

Technology Stack Components (In-scope):
- Blazor Server, Radzen components.
- OpenIdConnect (Keycloak) auth.
- OpenTelemetry + Health Checks (ServiceDefaults).
- Service Discovery + HTTP resilience handlers.
- Output Caching (configured; policies unverified).

Domain Entities & Relationships (In-scope):
- ModulePage / ModulePageSection / KeyboardShortcut (navigation metadata).
- IModule (extensibility contract) -> SampleModule implementation.
- ServiceNames constants consumed by AppHost resource declarations.
(Mocks domain entities excluded.)

Public API Surface (In-scope):
- Authentication endpoints: /authentication/login, /authentication/logout (GET/POST).
- Health endpoints (development): /health, /alive.
(Mocks endpoints excluded; see v0.01 for historical reference.)

Blazor UI Components (Summary):
- Layout: ShellLayout.
- Pages: ServicesPage, ExplorerPage, SamplePage (module).
- Shared: NavigationItem, EventConsole, RenderOnceComponent.
(Mock dashboard pages excluded.)

Cross-Cutting Concerns:
- DI: Module registration, scoped ModulePageService, transient AuthorizationHandler.
- Logging: Standard ASP.NET Core logging + OpenTelemetry (ServiceDefaults). (Serilog usage confined to mocks out-of-scope.)
- Error Handling: Default pipeline; Blazor error boundary styles.
- Caching: OutputCache registered (configuration not detailed).
- Configuration: Parameters via AppHost; service names constant class.
- Serialization: Default JSON (no source-gen) (Unverified enhancements).
- Security: OIDC + cookie auth; bearer propagation via AuthorizationHandler.
- Accessibility: Not explicitly addressed (Unverified).

5. Non-Functional Characteristics (Observed / In-scope)
- Reliability: Health checks in development.
- Observability: OpenTelemetry instrumentation; conditional OTLP exporter.
- Scalability: Aspire aids local multi-resource composition (production scaling not documented).
- Performance: No specific tuning (Unverified).
- Accessibility: Undocumented (gap).

6. Gaps & Unknowns
- No test projects for in-scope areas.
- No explicit error handling middleware.
- OutputCache policies undefined.
- Authorization policies & role usage absent (scope claim only).
- Accessibility strategy absent.
- Serialization customization absent.

7. Future Indicators
- No TODO markers (search performed in v0.01 baseline).

8. Traceability (Representative In-scope Paths)
- Auth & Program: src/Shell/UKHO.ADDS.Management.Host/Program.cs
- Auth endpoints: src/Shell/UKHO.ADDS.Management.Host/Extensions/LoginLogoutEndpointRouteBuilderExtensions.cs
- Module abstractions: src/Shell/UKHO.ADDS.Management.Shell/Modules/*.cs
- Module registration: src/Modules/UKHO.ADDS.Management.Modules.Samples/Registration/ModuleRegistration.cs
- Navigation service: src/Shell/UKHO.ADDS.Management.Shell/Services/ModulePageService.cs
- Telemetry & resilience: src/Shell/UKHO.ADDS.Management.ServiceDefaults/Extensions.cs
- AppHost orchestration: src/Shell/UKHO.ADDS.Management.AppHost/Program.cs

9. Cross-References
- spec-architecture-components_v0.02.md (updated components).
- spec-api-functional_v0.02.md (updated API scope).
- spec-frontend-functional_v0.02.md (UI components).
- spec-infra-deployment_v0.02.md (infra details).

10. Completion Checklist
- Mocks detail removed; high-level references retained.
- Gaps reaffirmed.
- No invention beyond observed implementation.

End of Document.
