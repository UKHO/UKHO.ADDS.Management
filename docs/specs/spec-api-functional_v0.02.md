Title: ADDS Management API Functional Specification
Version: v0.02 (Draft)
Status: Draft / Baseline Extraction (Adjusted Scope)
Supersedes: spec-api-functional_v0.01.md
Change Log:
- v0.02: Removed detailed mock endpoint catalog; retained only in-scope host endpoints.

1. Scope / Purpose
- Enumerate host (management shell) API endpoints and high-level infra endpoints; exclude mock endpoints.

2. Context & Overview
- Host implements authentication and health endpoints.
- OpenAPI added (development environment); exposes host endpoints only.

3. Components / Modules (API Exposure In-Scope)
- Authentication group: /authentication/login, /authentication/logout.
- Health checks (development): /health, /alive.

4. Detailed Elements (Endpoints)
Authentication:
- GET /authentication/login -> OIDC challenge (RedirectUri "/").
- GET /authentication/logout -> Cookie + OIDC sign-out (RedirectUri "/").
- POST /authentication/logout -> Same sign-out sequence.

Health:
- GET /health -> Aggregated readiness checks (development only).
- GET /alive -> Liveness check tagged with "live" (development only).

Supporting Structures:
- AuthorizationHandler: Injects bearer token into outgoing HttpClient requests for authenticated users.

5. Cross-Cutting API Concerns
- Authentication: OIDC Code flow with Keycloak; scope addsmanagement:all.
- Authorization: Registered but no explicit policies (role/claim enforcement gap).
- Versioning: None (gap).
- Validation: Not applicable (endpoints are infrastructural/auth).
- Error Handling: Relies on default pipeline.
- Documentation: OpenAPI available in development; no custom descriptions applied to auth routes.

6. Non-Functional Characteristics
- Security: Standard OIDC challenge/SignOut patterns.
- Reliability: Health endpoints provide readiness & liveness (development only).
- Observability: Telemetry instrumentation via ServiceDefaults.

7. Gaps & Unknowns
- Lack of versioning strategy.
- Missing authorization policies / role mapping.
- No standardized error model.
- No explicit rate limiting.

8. Future Indicators
- No TODO markers related to host API.

9. Traceability
- Auth endpoints: src/Shell/UKHO.ADDS.Management.Host/Extensions/LoginLogoutEndpointRouteBuilderExtensions.cs
- Auth configuration: src/Shell/UKHO.ADDS.Management.Host/Program.cs
- AuthorizationHandler: src/Shell/UKHO.ADDS.Management.Host/Extensions/AuthorizationHandler.cs
- Health mapping: src/Shell/UKHO.ADDS.Management.ServiceDefaults/Extensions.cs

10. Cross-References
- System overview v0.02.
- Architecture components v0.02.

11. Completion Checklist
- Host endpoints enumerated.
- Mock endpoints excluded per scope change.
- Gaps identified.

End of Document.
