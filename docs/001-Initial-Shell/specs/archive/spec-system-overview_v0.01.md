Title: ADDS Management System Overview
Version: v0.01 (Draft)
Status: Draft / Baseline Extraction
Supersedes: None
Change Log:
- Initial draft creation

1. Scope / Purpose
- Provide a high-level baseline of projects, runtime architecture, domains, and deployed resources for ADDS Management & associated Mocks.
- Describes only what exists (no future design).

2. Context & Overview
- Solution targets .NET 9.
- Composed of Blazor Server host (management shell) plus mock service set and Aspire AppHost orchestration.
- Key domains: Management Shell (UI), Mocks (Mocked external services & test harness), Samples Module (extensible feature module), ServiceDefaults (cross-cutting infra), AppHost (Aspire orchestrator).
- Authentication: Keycloak OIDC via OpenIdConnect + Cookies.
- UI Framework: Blazor Server + Radzen Components.
- Observability: OpenTelemetry metrics/tracing (ServiceDefaults) + Serilog (Mocks).

3. Components / Modules (Summary)
- Shell Host: src/Shell/UKHO.ADDS.Management.Host/ (Blazor Server app, routing, auth, pages)
- Shell Core: src/Shell/UKHO.ADDS.Management.Shell/ (Module abstractions & page catalog service)
- Samples Module: src/Modules/UKHO.ADDS.Management.Modules.Samples/ (Example module + SamplePage component & CSS)
- Service Defaults: src/Shell/UKHO.ADDS.Management.ServiceDefaults/ (Common OpenTelemetry, health checks, discovery, resilience)
- Configuration (Service Names): src/Shell/UKHO.ADDS.Management/Configuration/ (Constant service identifiers)
- AppHost (Aspire): src/Shell/UKHO.ADDS.Management.AppHost/ (DistributedApplication builder, resource provisioning: Keycloak, KeyVault emulator, Storage emulators, project references)
- Mocks Runtime: mock/repo/src/UKHO.ADDS.Mocks/ (MockServer, endpoint registration, markdown docs, traffic capture middleware)
- Mocks Management Overrides: mock/UKHO.ADDS.Mocks.Management/ (Program, override endpoints with response generators)

4. Detailed Elements (High-Level)
Projects (.csproj paths):
- UKHO.ADDS.Management: src/Shell/UKHO.ADDS.Management/UKHO.ADDS.Management.csproj (configuration constants)
- UKHO.ADDS.Management.Shell: src/Shell/UKHO.ADDS.Management.Shell/UKHO.ADDS.Management.Shell.csproj (module abstractions, services)
- UKHO.ADDS.Management.Host: src/Shell/UKHO.ADDS.Management.Host/UKHO.ADDS.Management.Host.csproj (Blazor host program/auth/routing)
- UKHO.ADDS.Management.ServiceDefaults: src/Shell/UKHO.ADDS.Management.ServiceDefaults/UKHO.ADDS.Management.ServiceDefaults.csproj (cross-cutting infra)
- UKHO.ADDS.Management.AppHost: src/Shell/UKHO.ADDS.Management.AppHost/UKHO.ADDS.Management.AppHost.csproj (Aspire resources)
- UKHO.ADDS.Management.Modules.Samples: src/Modules/UKHO.ADDS.Management.Modules.Samples/UKHO.ADDS.Management.Modules.Samples.csproj (sample module)
- UKHO.ADDS.Mocks: mock/repo/src/UKHO.ADDS.Mocks/UKHO.ADDS.Mocks.csproj (mock endpoints)
- UKHO.ADDS.Mocks.Management: mock/UKHO.ADDS.Mocks.Management/UKHO.ADDS.Mocks.Management.csproj (override / management mock logic)

Architecture Layers:
- UI: Management.Host + Modules (Blazor components, Radzen UI, routing).
- API (internal): Minimal API endpoints for authentication and mocks (Login/Logout in Host; mock endpoints under various mock paths).
- Shared/Core: ServiceDefaults, Shell abstractions (IModule, ModulePageService).
- Infra Orchestration: AppHost (Aspire) defines resources & dependencies.
- Mocks: Provides stubbed external system behaviors for development/validation.

Technology Stack Components:
- Blazor Server (interactive server components).
- Radzen UI library.
- OpenIdConnect (Keycloak) authentication.
- Output Caching (AddOutputCache in Host) – usage details (Unverified).
- OpenAPI (AddOpenApi in Host & Mocks) + Scalar UI in mocks.
- OpenTelemetry (metrics/tracing) via ServiceDefaults; Serilog console + OpenTelemetry exporter in MockServer.
- Azure resource emulators (KeyVault emulator, Storage emulator) managed via Aspire.

Domain Entities & Relationships (High-level summary):
- Shell domain: ModulePage, ModulePageSection, KeyboardShortcut represent navigable pages & metadata.
- Mocks domain: ServiceEndpointMock (derivatives for each mocked endpoint), StateDefinition / WellKnownState handle variable responses, Guard* classes for validation, Markdown* classes for doc generation, MimeType classes for response content types.
- Relationship: MappingService builds definitions -> applies to MockServer -> endpoints exposed.

Public API Surface (Summary – detailed list in api spec):
- Authentication group: /authentication/login (GET) /authentication/logout (GET/POST)
- Mock endpoints: Various under configured service roots (fss, fssmsi, sample, sap) – CRUD style minimal APIs.
- Health endpoints: /health, /alive (development only), mocks /health (fss HealthEndpoint).

Blazor UI Components (Summary – detailed list in frontend spec):
- Layout: ShellLayout, DashboardLayout (mocks) (generated .ide.g.cs indicates existence)
- Pages: ServicesPage, ExplorerPage, SamplePage, TrafficPage (mocks), etc.

Cross-Cutting Concerns (Overview):
- DI: AddScoped/AddSingleton patterns; Module registration via AddSampleModule.
- Logging: Serilog (Mocks), Microsoft.Extensions.Logging + OpenTelemetry (ServiceDefaults).
- Error Handling: No global exception middleware; Blazor error boundary styling in site.css; mock traffic capture middleware (MockTrafficCaptureMiddleware).
- Caching: Output cache registered (Host) – no explicit policy definitions found (Unverified).
- Configuration: Service names constants; environment/service parameters in AppHost (DistributedApplication builder AddParameter).
- Serialization: No source-generated context present (defaults) (Unverified).
- Security: OIDC + cookies; AuthorizationHandler attaches bearer token to outgoing HttpClient requests.
- Accessibility: Not explicitly addressed (Unverified).

5. Non-Functional Characteristics (Observed / Implied)
- Performance: No explicit perf settings (Unverified).
- Scalability: Aspire orchestration enables multi-resource composition (Unverified). AppHost for local dev.
- Reliability: Health checks (ServiceDefaults) for liveness/readiness in dev.
- Security: Keycloak realm integration; no role/claim enforcement specifics beyond scopes (addsmanagement:all) (Unverified detail).
- Observability: OpenTelemetry metrics/traces + Serilog; custom enrichment minimal.
- Accessibility: CSS present; no ARIA or a11y patterns identified (Unverified).

6. Gaps & Unknowns
- No test projects detected (coverage absent).
- No documented error handling strategy beyond default pipeline.
- Output cache usage details unclear.
- Serialization customization absent.
- No explicit authorization policies beyond AddAuthorization() (policy details unverified).
- No resilience custom settings besides AddStandardResilienceHandler.
- Accessibility considerations not evident.
- No explicit configuration layering (appsettings.* inspection not performed) (Unverified).

7. Future Indicators (Existing TODOs / Placeholders)
- No TODO markers found via search.

8. Traceability
- Service Names: src/Shell/UKHO.ADDS.Management/Configuration/ServiceNames.cs
- OIDC/Auth Pipeline: src/Shell/UKHO.ADDS.Management.Host/Program.cs
- AuthorizationHandler: src/Shell/UKHO.ADDS.Management.Host/Extensions/AuthorizationHandler.cs
- Login/Logout endpoints: src/Shell/UKHO.ADDS.Management.Host/Extensions/LoginLogoutEndpointRouteBuilderExtensions.cs
- Module abstractions/service: src/Shell/UKHO.ADDS.Management.Shell/Modules/*.cs, Services/ModulePageService.cs
- Sample module registration: src/Modules/UKHO.ADDS.Management.Modules.Samples/Registration/ModuleRegistration.cs
- AppHost resource build: src/Shell/UKHO.ADDS.Management.AppHost/Program.cs
- MockServer + endpoints: mock/repo/src/UKHO.ADDS.Mocks/* and overrides mock/UKHO.ADDS.Mocks.Management/*
- Cross-cutting infra: src/Shell/UKHO.ADDS.Management.ServiceDefaults/Extensions.cs
- Styles: src/Shell/UKHO.ADDS.Management.Host/wwwroot/css/site.css

9. Cross-References
- See spec-architecture-components_v0.01.md for detailed cross-cutting & DI.
- See spec-api-functional_v0.01.md for endpoint catalog.
- See spec-frontend-functional_v0.01.md for Blazor component inventory.
- See spec-infra-deployment_v0.01.md for Aspire & resource deployment details.
- See spec-domain-mocks_v0.01.md for mocks domain details.

10. Completion Checklist
- Projects enumerated.
- Layers outlined.
- Gaps identified.
- No invention of future features.

End of Document.
