Title: ADDS Management Infrastructure & Deployment Specification
Version: v0.02 (Draft)
Status: Draft / Baseline Extraction (Adjusted Scope)
Supersedes: spec-infra-deployment_v0.01.md
Change Log:
- v0.02: Removed internal mock runtime details; retained resource references only.

1. Scope / Purpose
- Capture infrastructure orchestration focused on AppHost & in-scope service components.

2. Context & Overview
- .NET Aspire AppHost orchestrates local development resources (Keycloak, KeyVault emulator, Storage emulator) and project startup.
- Mock service project referenced (UI command available) but its internal runtime excluded.

3. Components (In-Scope Infra)
Resources (AppHost Program):
- Parameters: username, password (for Keycloak initialization).
- Keycloak identity provider (data volume, realm import).
- KeyVault emulator (persistent secrets store for dev).
- Azure Storage emulator (Queues, Tables, Blobs sub-resources).
- Management Shell project resource with external HTTP endpoint, references, and launch command.
- Mock project resource (only as reference for local ecosystem completeness; internals out-of-scope).

4. Detailed Elements
Resource Commands (Extensions):
- WithKeycloakUi: Browser launch for admin UI when healthy.
- WithShell: Browser launch for shell when healthy.
- WithMockUi: Retained command (internals excluded).

Dependencies:
- Shell waits for: keyVault, queues, tables, blobs (ensures readiness ordering).
- Keycloak realm import path ./Realms (asset presence unverified).

Configuration:
- ServiceNames constants unify resource naming across AppHost & service code.

Telemetry & Health:
- ServiceDefaults provides OpenTelemetry instrumentation & health checks for in-scope services.
- Health endpoints active only in Development environment.

Security:
- OIDC across shell via Keycloak reference.
- Secrets strategy limited to emulator (production strategy unverified).

5. Cross-Cutting Infra Concerns
- Resilience: Standard HttpClient resilience handler applied.
- Service Discovery: Enabled via ServiceDefaults.
- Observability: Metrics & traces conditionally exported (OTLP endpoint env variable).

6. Non-Functional Characteristics
- Local developer efficiency: Single orchestrator launches dependencies.
- Reliability: WaitFor ensures dependency sequence.
- Scalability: Emulators not reflective of production scaling (gap).
- Security: Local identity provider; production hardening not described.

7. Gaps & Unknowns
- No production IaC (Bicep/Terraform) present.
- No deployment pipeline documentation.
- No containerization artifacts.
- Secrets rotation & management strategy absent beyond emulator.
- Monitoring/alerting configuration absent.

8. Future Indicators
- No TODO markers related to infra.

9. Traceability
- AppHost: src/Shell/UKHO.ADDS.Management.AppHost/Program.cs
- Resource commands: src/Shell/UKHO.ADDS.Management.AppHost/Extensions/ResourceBuilderExtensions.cs
- Service names: src/Shell/UKHO.ADDS.Management/Configuration/ServiceNames.cs
- Telemetry defaults: src/Shell/UKHO.ADDS.Management.ServiceDefaults/Extensions.cs
- Host program/auth: src/Shell/UKHO.ADDS.Management.Host/Program.cs

10. Cross-References
- System overview v0.02.
- Architecture components v0.02.

11. Completion Checklist
- Infra resources enumerated (mock internals excluded).
- Gaps documented.

End of Document.
