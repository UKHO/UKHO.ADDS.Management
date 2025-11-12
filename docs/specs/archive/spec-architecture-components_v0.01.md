Title: ADDS Management Architecture Components
Version: v0.01 (Draft)
Status: Draft / Baseline Extraction
Supersedes: None
Change Log:
- Initial draft creation

1. Scope / Purpose
- Enumerate core architectural components, their roles, and cross-cutting integrations.

2. Context & Overview
- Mixed solution combining Blazor Server shell, mock service infrastructure, and Aspire resource orchestration.
- Emphasis on extensibility via modules (IModule) and mock endpoint composition.

3. Components / Modules
UI / Shell:
- Program (Host): configures Blazor, auth, OpenAPI, Radzen, output cache.
- Module abstractions: IModule, ModulePage, ModulePageSection, KeyboardShortcut.
- ModulePageService: aggregates pages from modules + built-in shell pages.

Modules:
- SampleModule registered as singleton IModule instance (AddSampleModule).

Mocks:
- MockServer: orchestrates building & applying endpoint definitions.
- MappingService: builds definitions from service files/state for MockServer.
- ServiceEndpointMock derivatives define minimal API handlers (MapGet/MapPost/etc.).
- Override endpoints in UKHO.ADDS.Mocks.Management add alternative behavior (response generator approach).

Infra / Cross-Cutting:
- ServiceDefaults Extensions: OpenTelemetry configuration (metrics/tracing), health checks, service discovery, HTTP resilience.
- AppHost Program: DistributedApplication builder instantiates resources (Keycloak, KeyVault emulator, Azure Storage emulator components) and adds project references with custom commands (WithShell, WithMockUi, WithKeycloakUi).

4. Detailed Elements
Dependency Injection Patterns:
- AddSingleton<IModule> (SampleModule).
- AddScoped<ModulePageService>.
- AddTransient<AuthorizationHandler>.
- Serilog configuration via builder.Services.AddSerilog in MockServer.
- Output caching (AddOutputCache) registration (policy definitions not present) (Unverified).

Routing & Composition:
- Authentication endpoints grouped under /authentication (login/logout).
- Blazor router includes additional assemblies for module pages (AppRouter.razor referencing SamplePage assembly).

Resilience & Discovery:
- HttpClient defaults configured with AddStandardResilienceHandler + AddServiceDiscovery.
- AuthorizationHandler adds bearer token to outgoing requests.

State & Configuration:
- ServiceNames constants supply resource identifiers for AppHost (KeyVault/Storage/Mocks).
- StateDefinition & WellKnownState drive conditional responses in mocks.

Observability:
- OpenTelemetry instrumentation: AspNetCore, HttpClient, Runtime (metrics); AspNetCore + HttpClient + source for tracing.
- OTLP exporter conditional on OTEL_EXPORTER_OTLP_ENDPOINT.
- Serilog sinks: Console + OpenTelemetry sink (Mocks) with several Microsoft namespace level overrides.

Health / Liveness:
- Health checks /alive and /health mapped only in Development via MapDefaultEndpoints.
- Mocks include specific /health endpoint (fss HealthEndpoint).

5. Cross-Cutting Concerns
Logging:
- Serilog structured logging (Mocks) with environment-configurability.
- Console bootstrap logger early initialization.

Error Handling:
- No global exception middleware beyond standard pipeline.
- Blazor error boundary styling via site.css (.blazor-error-boundary).
- MockTrafficCaptureMiddleware in mocks captures request/response for internal logging views.

Caching:
- OutputCache enabled (Host) (actual usage or policies not surfaced) (Unverified).

Configuration Management:
- AppHost uses parameters (username/password) for Keycloak initialization.
- Keycloak realm import from ./Realms (folder existence not verified).
- OTEL endpoint configured via environment variable.

Security:
- OIDC configured: ClientId ADDSManagementShell, ResponseType Code, Scope adds addsmanagement:all, tokens saved, SignInScheme Cookie.
- Cookies for session persistence.
- No granular authorization policies or role checks listed (Unverified).

Serialization:
- Default ASP.NET Core model binding & JSON serialization; no System.Text.Json source-gen contexts found (Unverified).

Accessibility:
- Not explicitly addressed (Unverified).

6. Non-Functional Characteristics
- Reliability aided by health checks.
- Observability via OpenTelemetry + Serilog.
- Scalability patterns via Aspire composition (local orchestrated environment).
- Resilience default handler for HttpClient (policies internal to .NET standard library).

7. Gaps & Unknowns
- Missing explicit error handling strategy (no custom middleware for exceptions).
- No tests present validating modules or endpoints.
- No explicit configuration layering documented (appsettings inspection skipped).
- Absence of authorization policies (role/claims) beyond scope request.
- No custom telemetry enrichment (Unverified).
- OutputCache policies absent.

8. Future Indicators
- No TODO markers present in scanned code.

9. Traceability
- DI & Program: src/Shell/UKHO.ADDS.Management.Host/Program.cs
- Auth endpoints: src/Shell/UKHO.ADDS.Management.Host/Extensions/LoginLogoutEndpointRouteBuilderExtensions.cs
- OIDC config: Program.cs (Host)
- Module system: src/Shell/UKHO.ADDS.Management.Shell/Modules/*.cs
- Module registration: src/Modules/UKHO.ADDS.Management.Modules.Samples/Registration/ModuleRegistration.cs
- Service defaults: src/Shell/UKHO.ADDS.Management.ServiceDefaults/Extensions.cs
- AppHost orchestration: src/Shell/UKHO.ADDS.Management.AppHost/Program.cs
- Mock server: mock/repo/src/UKHO.ADDS.Mocks/MockServer.cs
- Mock overrides: mock/UKHO.ADDS.Mocks.Management/Override/**

10. Cross-References
- System overview spec for high-level description.
- API functional spec for endpoint catalog.
- Frontend functional spec for component list.

11. Completion Checklist
- Components enumerated.
- Cross-cutting concerns listed.
- Gaps highlighted.

End of Document.
