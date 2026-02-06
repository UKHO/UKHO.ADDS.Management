# ADDS Management — Infrastructure & Deployment Specification

**Title:** ADDS Management Infrastructure & Deployment Specification

**Version:** v0.02 (Draft)

**Status:** Draft / Baseline Extraction (Adjusted Scope)

**Supersedes:** `spec-infra-deployment_v0.01.md`

**Change Log:**
- v0.02: Removed internal mock runtime details; retained resource references only.

---

## 1. Executive Summary

**Purpose**
- Describe local infrastructure orchestration managed by the .NET Aspire `AppHost` and enumerate in-scope infrastructure resources for development. Mock runtime internals are intentionally out-of-scope for this document.

**Objective**
- Provide a concise reference for resources, orchestration commands, dependencies, telemetry and security patterns used during local development.

---

## 2. Scope / Purpose

**In scope**
- AppHost resource definitions and orchestration commands used to bring up local development dependencies (Keycloak, KeyVault emulator, Storage emulator, Management Shell)
- Resource readiness and dependency ordering
- Telemetry and health registration for in-scope services

**Out of scope**
- Internal runtime behavior of mock services (the `mock/` project is referenced only for completeness)
- Production IaC, deployment pipelines, and production-grade secrets rotation strategies

---

## 3. System Overview

**Context**
- Local development orchestration is performed by the .NET Aspire `AppHost` that composes multiple local resources and launches the Management Shell for developer testing.

**Core capabilities**
- Provision local Keycloak identity provider with realm import
- Provision a KeyVault emulator for persistent dev secrets
- Provision an Azure Storage emulator (Queues, Tables, Blobs)
- Launch Management Shell with external HTTP endpoint and readiness checks

---

## 4. Components & Resources (AppHost)

- **Keycloak identity provider**
  - Data volume for persistence
  - Realm import path: `./Realms` (asset presence unverified)
  - Parameters: `username`, `password` for initial admin credentials
- **KeyVault emulator**
  - Persistent secrets store for development scenarios
- **Azure Storage emulator**
  - Queues, Tables, Blobs sub-resources
- **Management Shell resource**
  - External HTTP endpoint, runtime references, and launch command
- **Mock project resource**
  - Included as an environment reference only; internal runtime details excluded from this spec

---

## 5. Resource Commands & Orchestration

**Provided convenience commands (resource builder extensions):**
- `WithKeycloakUi` — open admin UI in browser when Keycloak is healthy
- `WithShell` — open the Management Shell in browser when healthy
- `WithMockUi` — retained command for ecosystem completeness (internals excluded)

**Dependency ordering**
- The Shell resource waits for KeyVault and Azure Storage sub-resources (queues, tables, blobs) to ensure readiness ordering before launch

**Configuration**
- `ServiceNames` constants unify resource naming across `AppHost` and service code

---

## 6. Telemetry, Health & Observability

- `ServiceDefaults` registers OpenTelemetry instrumentation and health checks for in-scope services
- Health endpoints are exposed only in the Development environment (`/health`, `/alive`) and used by orchestration for readiness/liveness
- Observability exports (OTLP) are conditional and controlled via environment variables

---

## 7. Security & Secrets

- OIDC authentication for the Management Shell is configured via the Keycloak resource
- Secrets for local development are stored in the KeyVault emulator; production secrets handling and rotation are not covered by this spec

---

## 8. Cross-Cutting Infrastructure Concerns

- Resilience: Standard `HttpClient` resilience handler is applied to service clients
- Service discovery: Enabled via `ServiceDefaults` for in-scope services
- Observability: Metrics and traces are collected and conditionally exported

---

## 9. Non-Functional Characteristics

- Developer efficiency: Single orchestrator simplifies bringing up local dependencies
- Reliability: `WaitFor` patterns enforce dependency readiness ordering
- Scalability: Emulators do not reflect production scalability; production scaling is outside scope
- Security: Local identity provider used for dev; production hardening not described

---

## 10. Gaps & Unknowns

- No production IaC (Bicep/Terraform) is present in the repository
- No deployment pipeline documentation included
- No containerization artifacts or images documented
- Secrets rotation and production secrets strategy absent beyond emulator usage
- Monitoring and alerting configuration for production environments absent

---

## 11. Future Indicators

- No TODO markers related to infrastructure were found in the analyzed baseline

---

## 12. Traceability

- `AppHost` program: `src/Shell/UKHO.ADDS.Management.AppHost/Program.cs`
- Resource commands: `src/Shell/UKHO.ADDS.Management.AppHost/Extensions/ResourceBuilderExtensions.cs`
- Service names constants: `src/Shell/UKHO.ADDS.Management/Configuration/ServiceNames.cs`
- Telemetry defaults & health mapping: `src/Shell/UKHO.ADDS.Management.ServiceDefaults/Extensions.cs`
- Host program & auth: `src/Shell/UKHO.ADDS.Management.Host/Program.cs`

---

## 13. Cross-References

- `spec-system-overview_v0.02.md`
- `spec-architecture-components_v0.02.md`

---

## 14. Completion Checklist

- Infra resources enumerated (mock internals excluded)
- Dependency ordering and orchestration commands documented
- Gaps and unknowns recorded

---

_End of Document._
