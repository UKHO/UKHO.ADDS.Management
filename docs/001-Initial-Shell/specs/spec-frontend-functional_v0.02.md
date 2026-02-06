# ADDS Management — Frontend Functional Specification

**Title:** ADDS Management Frontend Functional Specification

**Version:** v0.02 (Draft)

**Status:** Draft / Baseline Extraction (Adjusted Scope)

**Supersedes:** `spec-frontend-functional_v0.01.md`

**Change Log:**
- v0.02: Removed specification of mock dashboard pages/components.

---

## 1. Executive Summary

**Purpose**
- Inventory and describe Blazor Server shell UI elements, module integration, and component responsibilities while excluding mock dashboard internals.

**Objective**
- Provide a clear reference for UI components, navigation model, DI patterns, and gaps for future improvements.

---

## 2. Scope / Purpose

**In scope**
- Blazor Server host UI and modules (SampleModule)
- Shared components and navigation abstractions

**Out of scope**
- Mock dashboard pages and components

---

## 3. System Overview

**Context**
- Blazor Server application using Radzen components and module-based extensibility.

**Core capabilities**
- Host layout and navigation for module pages
- Dynamic discovery of module pages via assembly injection
- Theming and basic styling via `site.css` and Radzen theme service

---

## 4. UI Components (Summary)

**Layouts**
- `ShellLayout` (host layout)

**Pages**
- `ServicesPage.razor` (host)
- `ExplorerPage.razor` (host)
- `SamplePage.razor` (module)
- `App.razor` (root shell component)
- `AppRouter.razor` (router with `AdditionalAssemblies` for modules)

**Shared components**
- `NavigationItem.razor`
- `EventConsole.razor`
- `RenderOnceComponent.razor`

**Services / abstractions**
- `ModulePageService` (page aggregation / resolution)
- `IModule`, `ModulePage`, `ModulePageSection`, `KeyboardShortcut` metadata classes

**Styles & theming**
- `wwwroot/css/site.css` (layout, Radzen variables, error UI)
- `SamplePage.razor.css` (module-local styling)
- `RadzenQueryStringThemeService` used for dynamic theme (activation logic unverified)

**JavaScript interop**
- `exampleJsInterop.js` (function `showPrompt`) — usage not evidenced in codebase

---

## 5. Detailed Elements

**Routing**
- Router includes module assemblies enabling page discovery (e.g., `/sample/main` path)

**Navigation metadata**
- `ModulePage` defines `Name`, `Path`, `Icon`, optional `Title`, and `Description` for display

**Page resolution**
- `ModulePageService.FindCurrent(Uri)` flattens hierarchical pages to resolve the active page

**Title/description logic**
- `TitleFor` returns `Title` unless page `Name` is `Overview`; otherwise uses fallback

**State management**
- Navigation state contained in `ModulePageService`; no global store pattern observed

**Authentication integration**
- `CascadingAuthenticationState` registered; per-component auth usage not present in the spec

**Error visualization**
- `.blazor-error-boundary` styling and `#blazor-error-ui` overlay present for circuit errors

---

## 6. Cross-Cutting Concerns

- DI: Scoped `ModulePageService`; singleton module instances
- Security: App-level auth configured; pages lack `[Authorize]` attributes (access control granularity gap)
- Accessibility: Unspecified (gap)
- Performance: Default Blazor Server behavior; `CircuitOptions.DetailedErrors` enabled in development
- Localization: Not registered in host (gap)

---

## 7. Non-Functional Characteristics

- Extensibility: Additional modules can register `IModule` implementations to plug into the UI
- Maintainability: Separation of navigation model and rendering improves clarity
- Reliability: Blazor error boundary and overlay provide runtime diagnostics

---

## 8. Gaps & Unknowns

- No component/unit tests
- Accessibility strategy absent
- Localization support not present
- JavaScript interop usage unverified
- No documented performance profiling or server pre-render strategy

---

## 9. Traceability

- Router: `src/Shell/UKHO.ADDS.Management.Host/Shell/AppRouter.razor`
- Module & pages: `src/Shell/UKHO.ADDS.Management.Shell/Modules/*.cs`
- Navigation service: `src/Shell/UKHO.ADDS.Management.Shell/Services/ModulePageService.cs`
- Styles: `src/Shell/UKHO.ADDS.Management.Host/wwwroot/css/site.css`
- Module sample assets: `src/Modules/UKHO.ADDS.Management.Modules.Samples/*`

---

## 10. Cross-References

- `spec-architecture-components_v0.02.md` for DI and architecture context
- `spec-api-functional_v0.02.md` for auth endpoints impacting UI

---

## 11. Completion Checklist

- In-scope UI items documented
- Mock dashboard removed from scope
- Gaps listed

---

_End of Document._
