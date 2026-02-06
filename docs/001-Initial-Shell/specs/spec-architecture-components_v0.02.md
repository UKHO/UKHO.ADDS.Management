# ADDS Management — Architecture Components

**Title:** ADDS Management Architecture Components

**Version:** v0.02 (Draft)

**Status:** Draft / Baseline Extraction (Adjusted Scope)

**Supersedes:** `spec-architecture-components_v0.01.md`

**Change Log:**
- v0.02: Removed internal ADDS Mock domain/component detail per updated scope.

---

## 1. Executive Summary

**Purpose**
- Enumerate core architectural components for the ADDS Management solution while excluding mock internals.

**Objective**
- Provide a concise reference for component responsibilities, DI patterns, telemetry, resilience, and orchestration.

---

## 2. Scope / Purpose

**In scope**
- Blazor Server shell and module system
- Cross-cutting `ServiceDefaults`
- `AppHost` orchestration resources

**Out of scope**
- Mock service internal components and domain details

---

## 3. System Overview

**Context**
- Architecture centers on a modular Blazor Server shell, local orchestration via Aspire (AppHost), and cross-cutting service defaults.

**Core capabilities**
- Modular page injection via `IModule`
- Centralized telemetry and resilience defaults
- Local resource orchestration for development

---

## 4. Components (Summary)

- UI Shell Program: Host responsibilities include auth, OpenAPI, Radzen UI, OutputCache, Blazor server components
- Module system: `IModule`, `ModulePage`, `ModulePageSection`, keyboard shortcut metadata
- `ModulePageService`: Aggregates module pages and static pages (`Services`, `Explorer`)
- SampleModule: Example `IModule` implementation (singleton)
- `ServiceDefaults`: OpenTelemetry, health checks, service discovery, HTTP resilience
- `ServiceNames`: Central service identifiers consumed by `AppHost`
- `AppHost` Program: Resource definitions for Keycloak, KeyVault emulator, Storage emulator
- `ResourceBuilderExtensions`: Convenience commands (`WithShell`, `WithKeycloakUi`, `WithMockUi`) — mock UI retained as reference only

---

## 5. Detailed Elements

**Dependency Injection patterns**
- Singleton: `SampleModule` (implements `IModule`)
- Scoped: `ModulePageService`
- Transient: `AuthorizationHandler`
- Scoped `HttpClient` instances per navigation base address for the host

**Routing & composition**
- `AppRouter` configured with `AdditionalAssemblies` for module discovery
- Authentication endpoints grouped under `/authentication`

**Resilience & telemetry**
- Resilience handler applied to `HttpClient`
- OpenTelemetry: AspNetCore, HttpClient, runtime metrics, and conditional OTLP exporter

**Security**
- OIDC via Keycloak: client id `ADDSManagementShell`, authorization code flow, scope `addsmanagement:all`
- Authorization registration present but no policies defined (gap)
- `AuthorizationHandler` adds bearer token for outgoing requests

**Configuration & parameters**
- `AppHost` uses `builder.AddParameter` for Keycloak credentials
- `ServiceNames` constants used across app and AppHost

**Health**
- Health checks mapped in development (`/health`, `/alive`) via MapDefaultEndpoints

---

## 6. Cross-Cutting Concerns

- Logging: Microsoft logging with OpenTelemetry exporter
- Error handling: Default pipeline (no centralized middleware)
- Caching: `OutputCache` enabled; policies unverified
- Serialization: Default JSON serializer (no source-gen)
- Security: Authentication configured; fine-grained authorization unspecified
- Accessibility: Not addressed
- Configuration hygiene: Central constants exist; validation patterns unverified

---

## 7. Non-Functional Characteristics

- Observability: Telemetry baseline via `ServiceDefaults`
- Reliability: Readiness/liveness health checks in Development
- Maintainability: Module abstraction encourages separation
- Scalability: Aspire facilitates local composition; production scaling not documented

---

## 8. Gaps & Unknowns

- Missing tests for module system and auth pipeline
- Absent error handling middleware and authorization policies
- OutputCache and resilience configuration not tuned or documented
- No accessibility or localization strategy
- No options validation pattern observed

---

## 9. Traceability

- Auth & Program: `src/Shell/UKHO.ADDS.Management.Host/Program.cs`
- Auth endpoints: `src/Shell/UKHO.ADDS.Management.Host/Extensions/LoginLogoutEndpointRouteBuilderExtensions.cs`
- `AuthorizationHandler`: `src/Shell/UKHO.ADDS.Management.Host/Extensions/AuthorizationHandler.cs`
- Module abstractions: `src/Shell/UKHO.ADDS.Management.Shell/Modules/*.cs`
- Module service: `src/Shell/UKHO.ADDS.Management.Shell/Services/ModulePageService.cs`
- Sample module: `src/Modules/UKHO.ADDS.Management.Modules.Samples/*.cs`
- Service defaults: `src/Shell/UKHO.ADDS.Management.ServiceDefaults/Extensions.cs`
- AppHost orchestration: `src/Shell/UKHO.ADDS.Management.AppHost/Program.cs`
- Resource commands: `src/Shell/UKHO.ADDS.Management.AppHost/Extensions/ResourceBuilderExtensions.cs`

---

## 10. Cross-References

- `spec-system-overview_v0.02.md`
- `spec-api-functional_v0.02.md`
- `spec-frontend-functional_v0.02.md`
- `spec-infra-deployment_v0.02.md`

---

## 11. Completion Checklist

- Mock internals excluded
- Gaps recorded

---

_End of Document._
