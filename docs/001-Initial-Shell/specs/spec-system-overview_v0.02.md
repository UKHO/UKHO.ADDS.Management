# ADDS Management — System Overview

**Title:** ADDS Management System Overview

**Version:** v0.02 (Draft)

**Status:** Draft / Baseline Extraction (Adjusted Scope)

**Supersedes:** `spec-system-overview_v0.01.md`

**Change Log:**
- v0.02: Removed detailed ADDS Mock specifications per updated scope; retained high-level references only.

---

## 1. Executive Summary

**Purpose**
- Provide a concise, high-level baseline of in-scope projects and architecture. Mock internals (the `mock/` folder) are referenced but intentionally out-of-scope for this version.

**Objective**
- Capture scope, components, cross-cutting concerns, and traceability for the Management Shell (Blazor Server) and supporting modules and infra.

---

## 2. Scope / Purpose

**In scope**
- Management Shell (Blazor Server host and modules)
- Service Defaults (cross-cutting infra: telemetry, resilience, health checks, service discovery)
- AppHost (Aspire orchestration and local resource composition)
- Sample Module (extensibility example)

**Out of scope**
- Internal details of ADDS Mock services (`mock/`) — only referenced by name/path.

---

## 3. System Overview

**Context**
- Solution targets .NET 9.
- UI: Blazor Server + Radzen.
- Authentication: Keycloak OIDC (OpenIdConnect + Cookies).

**Core capabilities**
- Host a modular Blazor management shell
- Provide cross-cutting defaults for telemetry, resilience, health checks and service discovery
- Offer an AppHost for local resource orchestration (Aspire)
- Demonstrate extensibility via a Sample Module

---

## 4. Architecture Overview

**Architecture style**
- Monolithic Blazor Server host with modular extensibility points (modules implementing `IModule`).

**Communication mechanisms**
- HTTP (internal/external endpoints), bearer propagation via a custom `AuthorizationHandler` (transient), OIDC cookie auth.

**Hosting platform**
- Blazor Server host project: `src/Shell/UKHO.ADDS.Management.Host/`

**Layers**
- UI Layer: Blazor Server host + modules
- Cross-Cutting Layer: `ServiceDefaults` (telemetry/resilience)
- Infra Layer: `AppHost` (Aspire resource orchestration)
- Extensibility Layer: Modules implementing `IModule`

---

## 5. Components & Modules (Summary)

- Shell Host: `src/Shell/UKHO.ADDS.Management.Host/` — Blazor host, auth, routing
- Shell Core: `src/Shell/UKHO.ADDS.Management.Shell/` — module abstractions, page catalog service
- Samples Module: `src/Modules/UKHO.ADDS.Management.Modules.Samples/` — example extensibility entry point
- Service Defaults: `src/Shell/UKHO.ADDS.Management.ServiceDefaults/` — telemetry, resilience, health checks, service discovery
- Configuration: `src/Shell/UKHO.ADDS.Management/Configuration/ServiceNames.cs`
- AppHost: `src/Shell/UKHO.ADDS.Management.AppHost/` — Aspire resource orchestration
- Mocks (out-of-scope): `mock/` — referenced only for existence

---

## 6. Public API Surface (In-scope)

- Authentication endpoints: `/authentication/login`, `/authentication/logout` (GET/POST)
- Health endpoints (development): `/health`, `/alive`

(Mocks endpoints excluded; historical details are available in v0.01.)

---

## 7. Blazor UI Components (Summary)

- Layout: `ShellLayout`
- Pages: `ServicesPage`, `ExplorerPage`, `SamplePage` (module)
- Shared: `NavigationItem`, `EventConsole`, `RenderOnceComponent`

---

## 8. Domain Entities & Relationships (High-Level)

- `ModulePage` / `ModulePageSection` / `KeyboardShortcut` — navigation metadata
- `IModule` (extensibility contract) -> Sample module implementation
- `ServiceNames` constants consumed by `AppHost` resource declarations

(Mocks domain entities excluded.)

---

## 9. Cross-Cutting Concerns

- DI: Module registration, scoped `ModulePageService`, transient `AuthorizationHandler`
- Logging: ASP.NET Core logging + OpenTelemetry (ServiceDefaults). Serilog usage confined to mocks (out-of-scope)
- Error handling: Default pipeline and Blazor error boundary styles
- Caching: `OutputCache` registered (policies unverified)
- Configuration: Parameters via `AppHost`; `ServiceNames` constant class
- Serialization: Default JSON (no source-gen) — unverified enhancements
- Security: OIDC + cookie auth; bearer propagation via `AuthorizationHandler`
- Accessibility: Not explicitly addressed (gap)

---

## 10. Non-Functional Characteristics

- Reliability: Development health checks present
- Observability: OpenTelemetry instrumentation; conditional OTLP exporter
- Scalability: Aspire supports local multi-resource composition; production scaling not documented here
- Performance: No specific tuning observed
- Accessibility: Undocumented (gap)

---

## 11. Gaps & Unknowns

- No test projects for in-scope areas
- No explicit error handling middleware
- `OutputCache` policies undefined
- Authorization policies & role usage absent (only `scope` claim referenced)
- Accessibility strategy absent
- Serialization customization absent

---

## 12. Traceability (Representative In-scope Paths)

- Auth & Program: `src/Shell/UKHO.ADDS.Management.Host/Program.cs`
- Auth endpoints: `src/Shell/UKHO.ADDS.Management.Host/Extensions/LoginLogoutEndpointRouteBuilderExtensions.cs`
- Module abstractions: `src/Shell/UKHO.ADDS.Management.Shell/Modules/*.cs`
- Module registration: `src/Modules/UKHO.ADDS.Management.Modules.Samples/Registration/ModuleRegistration.cs`
- Navigation service: `src/Shell/UKHO.ADDS.Management.Shell/Services/ModulePageService.cs`
- Telemetry & resilience: `src/Shell/UKHO.ADDS.Management.ServiceDefaults/Extensions.cs`
- AppHost orchestration: `src/Shell/UKHO.ADDS.Management.AppHost/Program.cs`

---

## 13. Cross-References

- `spec-architecture-components_v0.02.md` (updated components)
- `spec-api-functional_v0.02.md` (updated API scope)
- `spec-frontend-functional_v0.02.md` (UI components)
- `spec-infra-deployment_v0.02.md` (infra details)

---

## 14. Future Indicators

- No TODO markers found in baseline (v0.01 search performed)

---

## 15. Completion Checklist

- Mocks detail removed; high-level references retained
- Gaps reaffirmed
- No invention beyond observed implementation

---

_End of Document._
