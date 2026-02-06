# FSS Manager — Domain Specification

**Title:** FSS Manager Domain Specification

**Version:** v0.01 (Draft)

**Status:** Draft / Baseline Extraction

**Supersedes:** None

**Change Log:**
- Initial draft creation

---

## 1. Executive Summary

Purpose
- Establish a baseline for the FSS Manager module describing domain intent, planned extensibility hooks, and the current implementation status (many items unverified or pending implementation).

Objective
- Provide a concise reference for module authors and reviewers to understand expected domain entities, UI pages, DI registrations, and gaps to address during implementation.

---

## 2. Scope / Purpose

In scope
- Domain intent and anticipated entities for the FSS Manager module
- Module integration pattern via `IModule` and navigation model
- High-level UI component expectations and DI/authorization considerations

Out of scope
- Implementation details (code) and runtime behavior — the module project is not yet implemented

---

## 3. Context & Overview

- The FSS Manager module will integrate into the host Shell using the existing `IModule` extensibility pattern.
- The module provides management UI for FSS-related operations. Specific pages, endpoints, DTOs, and domain model shapes are pending and therefore marked as unverified.

---

## 4. Planned Components & Structure

Planned project path (unverified)
- `src/Modules/UKHO.ADDS.Management.Modules.FssManager/`

Planned module entry
- `FssManagerModule` (implements `IModule`) — registration and service wiring via an `AddFssManagerModule` extension method (pattern similar to `AddSampleModule`)

Navigation model
- `ModulePage` entries to represent module pages and sections (e.g., `Batches`, `Files`, `Attributes`)

Pages (placeholders, unverified)
- `BatchListPage.razor`
- `BatchDetailPage.razor`
- Additional pages to manage FSS files and attributes

---

## 5. Domain Entities (Anticipated)

- `FSSBatch` — represents a logical grouping of FSS files; contains metadata and collection of `FSSFile`
- `FSSFile` — represents an individual file within a batch; includes file metadata and status
- `FSSAttribute` — key-value metadata associated with batches or files

Relationships (anticipated)
- `FSSBatch` 1..* `FSSFile`
- `FSSFile` 0..* `FSSAttribute`

Contracts / DTOs
- None implemented yet; to be defined when domain model and API surface are agreed

---

## 6. Blazor Components & UI Behavior

- Root pages:
  - `BatchListPage.razor` — list of batches, paging/filtering controls, navigation to batch details
  - `BatchDetailPage.razor` — details for a single batch, file list, attribute editor
- Shared components:
  - `NavigationItem` entries for module navigation
  - Reuse `EventConsole`, `RenderOnceComponent` from host shared components where applicable
- Styling and theming:
  - Module-local CSS files (e.g., `SamplePage.razor.css` pattern)
- JavaScript interop:
  - None currently defined; use host patterns if interop is required

---

## 7. Services, DI & Integration

- DI registration:
  - Provide an `AddFssManagerModule(IServiceCollection)` extension following the host pattern
  - Register module as singleton implementing `IModule`
  - Register page service(s) (scoped) if required for stateful navigation or data access
- Integration points:
  - Use `ModulePageService` for page aggregation
  - Use `AuthorizationHandler` and host auth scopes for outgoing requests if module calls backend APIs

---

## 8. Cross-Cutting Concerns

- Authentication/Authorization: Rely on host OIDC configuration; module should request `addsmanagement:all` scope as needed and define authorization policies if finer-grained control is required
- Logging & Telemetry: Inherit OpenTelemetry instrumentation from `ServiceDefaults`; add custom spans only where beneficial
- Error handling: Rely on host error boundaries and pipeline; consider user-friendly error messages in UI
- Serialization: Follow host JSON conventions (default System.Text.Json) unless otherwise required
- Accessibility & Localization: Not yet addressed; include accessibility review and localization support in implementation plan

---

## 9. Non-Functional Characteristics

- Performance: UI-only module; performance constraints follow Blazor Server host characteristics
- Observability: Inherit host telemetry; add domain-specific metrics as needed
- Scalability: Module scales with host (Blazor Server circuits); server-side scaling considerations align with host
- Reliability: Leverage host health checks and error boundaries

---

## 10. Gaps & Next Steps

- Module project not yet created
- No pages/components implemented
- Domain models and DTOs not defined
- No tests (unit/integration)
- No configuration or authorization policies

Next actions
- Create module project scaffold using host module pattern
- Define domain model classes and DTOs
- Implement basic pages and register module via `AddFssManagerModule`
- Add tests and CI checks
- Define configuration options and authorization policies

---

## 11. Traceability

- Planned module folder: `src/Modules/UKHO.ADDS.Management.Modules.FssManager/` (pending creation)
- Shell integration: Module registration extension method in module project (to be added)

---

## 12. Future Indicators

- Add TODO markers for implementation tasks in the module scaffold
- Consider creating a detailed functional spec once domain model and API surface are agreed

---

## 13. Completion Checklist

- Module project scaffold created
- Core domain models implemented
- Pages implemented and navigable via `ModulePageService`
- DI registration and auth policies configured
- Unit and integration tests added

---

_End of Document._
