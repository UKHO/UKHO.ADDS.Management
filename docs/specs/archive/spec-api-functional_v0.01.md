Title: ADDS Management API Functional Specification
Version: v0.01 (Draft)
Status: Draft / Baseline Extraction
Supersedes: None
Change Log:
- Initial draft creation

1. Scope / Purpose
- Enumerate currently implemented minimal API endpoints & behaviors in Host and Mocks.

2. Context & Overview
- Host provides authentication endpoints and OpenAPI exposure (development conditionally).
- Mock services expose numerous file/batch/sample endpoints simulating external systems (fss, fssmsi, sap, sample).

3. Components / Modules (API Exposure)
- Host Authentication Group (/authentication).
- Mocks endpoint sets grouped by service configuration folder names: fss, fssmsi, sap, sample.
- Management overrides extend fss behavior (response generator logic).

4. Detailed Elements (Endpoints Catalog)
Authentication (Host):
- GET /authentication/login -> OIDC challenge (RedirectUri "/").
- GET /authentication/logout -> SignOut (cookie + OIDC) redirect "/".
- POST /authentication/logout -> Same sign-out sequence.

Health / Diagnostics:
- (Development only) /health, /alive (ServiceDefaults) -> HealthCheck results.
- Mocks: GET /health (fss HealthEndpoint) -> 200 "Healthy".

FSS Mock Endpoints (Representative):
- GET /batch -> Returns file batchsearchresult.json or NotFound.
- POST /batch -> Creates batch (201 with batchId).
- PUT /batch/{batchId} -> Commit batch (202 Accepted or validation error).
- PUT /batch/{batchId}/expiry -> Sets expiry (204 or validation error).
- POST /batch/{batchId}/files/{fileName} -> Add file (201).
- PUT /batch/{batchId}/files/{fileName}/{blockId} -> Upload block (201 or BadRequest).
- PUT /batch/{batchId}/files/{fileName} -> Write block (204 or error object).
- GET /batch/{batchId}/files/{fileName} -> Download file (readme.txt) or 404.
- GET /attributes/search -> Returns attributes.json.

FSS MSI Mock Endpoints:
- GET /batch -> Returns annualfiles.json.
- GET /attributes/search -> Returns attributes.json.

SAP Mock Endpoints:
- GET /z_adds_mat_info.asmx -> 200 OK.
- POST /z_adds_ros.asmx -> Returns response.xml file or 404.

Sample Mock Endpoints:
- GET /files -> Returns string result OR (state "get-file") returns file readme.txt.
- POST /files -> Returns success string (does not persist).
- PUT /files -> Returns success (UpdateFilesEndpoint).
- DELETE /files -> State-dependent response via WellKnownStateHandler.

Overrides (Management Mocks):
- GET /batch (override) -> ResponseGenerator (search filter logic) replaces file-driven result. Adds markdown metadata describing sample query attributes.

Endpoint Metadata & Documentation:
- Each ServiceEndpointMock attaches markdown elements describing purpose, parameters, or sample queries.

5. Cross-Cutting API Concerns
- Authentication: Bearer tokens attached to HttpClient outbound via AuthorizationHandler (Host).
- Authorization: AddAuthorization without explicit policies (Unverified for fine-grained access).
- Versioning: No API versioning present (Unverified).
- Validation: Basic request body presence checks; Guard classes exist but not visibly wired to endpoints (Unverified usage in API pipeline).
- Error Handling: Manual conditional returns (BadRequest, NotFound); no global exception filter.
- Documentation: OpenAPI via AddOpenApi (Host & Mocks); Scalar UI (Mocks). Markdown metadata added to operations by custom transformer (MockServer.ConfigureOpenApi).

6. Non-Functional Characteristics
- Observability: Operations traced via OpenTelemetry instrumentation automatically (AspNetCore + HttpClient).
- Performance: Minimal logic endpoints (mock/demonstration oriented).
- Reliability: Health endpoints supply readiness status (development only for main host; explicit health for fss mock).
- Security: OIDC for host, mocks appear unsecured (no auth checks) (Unverified security boundary between host and mocks).

7. Gaps & Unknowns
- No explicit rate limiting or throttling.
- No API versioning strategy.
- Limited validation (only body presence checks).
- Absence of comprehensive error model (no standardized error envelope).
- Authorization policy usage not evident.
- No correlation ID propagation logic (Unverified).

8. Future Indicators
- No TODO markers in endpoint sources.

9. Traceability (Representative Source Files)
- Host auth endpoints: src/Shell/UKHO.ADDS.Management.Host/Extensions/LoginLogoutEndpointRouteBuilderExtensions.cs
- Authorization handler: src/Shell/UKHO.ADDS.Management.Host/Extensions/AuthorizationHandler.cs
- FSS endpoints: mock/repo/src/UKHO.ADDS.Mocks/Configuration/Mocks/fss/*.cs
- FSS MSI endpoints: mock/repo/src/UKHO.ADDS.Mocks/Configuration/Mocks/fssmsi/*.cs
- SAP endpoints: mock/repo/src/UKHO.ADDS.Mocks/Configuration/Mocks/sap/*.cs
- Sample endpoints: mock/repo/src/UKHO.ADDS.Mocks/Configuration/Mocks/sample/*.cs
- Overrides: mock/UKHO.ADDS.Mocks.Management/Override/Mocks/fss/GetFssBatchesEndpoint.cs
- OpenAPI transformer: mock/repo/src/UKHO.ADDS.Mocks/MockServer.cs (ConfigureOpenApi).

10. Cross-References
- See spec-architecture-components_v0.01.md for DI & observability.
- See spec-domain-mocks_v0.01.md for state & markdown domain objects.

11. Completion Checklist
- Endpoints enumerated.
- Behaviors summarized.
- Gaps identified.

End of Document.
