Title: ADDS Management Frontend Functional Specification
Version: v0.02 (Draft)
Status: Draft / Baseline Extraction (Adjusted Scope)
Supersedes: spec-frontend-functional_v0.01.md
Change Log:
- v0.02: Removed specification of mock dashboard pages/components.

1. Scope / Purpose
- Inventory and describe in-scope Blazor Server shell UI elements and module integration excluding mock dashboard.

2. Context & Overview
- Blazor Server application with Radzen UI components.
- Extensibility via IModule implemented (SampleModule).

3. Components (In-Scope)
Layouts:
- ShellLayout (Host).

Pages:
- ServicesPage.razor (Host).
- ExplorerPage.razor (Host).
- SamplePage.razor (Sample module).
- App.razor (root shell component).
- AppRouter.razor (router configuration with AdditionalAssemblies referencing module).

Shared Components:
- NavigationItem.razor.
- EventConsole.razor.
- RenderOnceComponent.razor.

Services / Abstractions:
- ModulePageService (page aggregation logic).
- IModule + ModulePage + ModulePageSection + KeyboardShortcut metadata classes.

Styles & Theming:
- site.css (syntax highlighting, layout, Radzen theme variable usage, error UI styles).
- SamplePage.razor.css (demo component styling).
- RadzenQueryStringThemeService for dynamic theme (activation logic unverified).

JavaScript Interop:
- exampleJsInterop.js (showPrompt) (usage not evidenced).

4. Detailed Elements
Routing:
- Router adds Sample module assembly to enable page discovery (/sample/main path).
Navigation Metadata:
- ModulePage entries define Name, Path, Icon; additional metadata Title, Description (optional) for display.
Page Resolution:
- ModulePageService.FindCurrent(Uri) flattens children recursively to resolve active page.
Title/Description Logic:
- TitleFor returns Title if page Name != "Overview"; DescriptionFor returns Description or empty.

State Management:
- Navigation state encapsulated in ModulePageService; no global store pattern.
Authentication Integration:
- CascadingAuthenticationState registered; component-level usage not shown.
Error Visualization:
- .blazor-error-boundary styling and #blazor-error-ui overlay for circuit errors.

5. Cross-Cutting Concerns
- DI: Scoped ModulePageService; singleton module instances.
- Security: Auth integrated at app level; pages lack explicit [Authorize] attributes (access control granularity gap).
- Accessibility: Unspecified (gap).
- Performance: Default Blazor Server; CircuitOptions.DetailedErrors enabled (development overhead).
- Localization: Not registered in host (gap); (Mocks had localization, removed from scope).

6. Non-Functional Characteristics
- Extensibility: Additional modules can plug by registering IModule implementations.
- Maintainability: Clear separation of navigation model vs rendering.
- Reliability: Error UI and boundary present for diagnostics.

7. Gaps & Unknowns
- No component/unit tests.
- Accessibility strategy absent.
- No documented localization support.
- JavaScript interop usage unverified.
- No explicit performance profiling or pre-render strategy.

8. Future Indicators
- No TODO markers related to frontend.

9. Traceability
- Router: src/Shell/UKHO.ADDS.Management.Host/Shell/AppRouter.razor
- Module & pages: src/Shell/UKHO.ADDS.Management.Shell/Modules/*.cs
- Navigation service: src/Shell/UKHO.ADDS.Management.Shell/Services/ModulePageService.cs
- Styles: src/Shell/UKHO.ADDS.Management.Host/wwwroot/css/site.css
- Module Sample assets: src/Modules/UKHO.ADDS.Management.Modules.Samples/*

10. Cross-References
- Architecture components v0.02 for DI overview.
- API functional v0.02 for auth endpoints impacting UI.

11. Completion Checklist
- In-scope UI items documented.
- Mock dashboard removed.
- Gaps listed.

End of Document.
