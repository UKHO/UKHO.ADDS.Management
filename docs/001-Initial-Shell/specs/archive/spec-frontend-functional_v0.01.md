Title: ADDS Management Frontend Functional Specification
Version: v0.01 (Draft)
Status: Draft / Baseline Extraction
Supersedes: None
Change Log:
- Initial draft creation

1. Scope / Purpose
- Baseline inventory of Blazor UI components, routing, theming artifacts, and module integration.

2. Context & Overview
- Blazor Server host application with Radzen Components.
- Module-based page extension via IModule discovery at startup.
- Styles provided via site.css and component-specific CSS (SamplePage.razor.css).

3. Components / Modules (UI)
Layouts (Generated or referenced):
- ShellLayout (Host) (generated file indicates existence).
- DashboardLayout (Mocks) (generated file in mocks).

Pages (Host Shell):
- ServicesPage.razor (Host) (generated .ide.g.cs).
- ExplorerPage.razor (Host) (generated .ide.g.cs).
- App.razor (Host) root component.
- AppRouter.razor (custom Router with AdditionalAssemblies for Sample module).

Pages (Mocks Dashboard):
- ServicesPage.razor (Mocks dashboard).
- ExplorerPage.razor (Mocks dashboard).
- TrafficPage.razor (Mocks dashboard).
- AppRouter.razor (Mocks dashboard).
- App.razor (Mocks dashboard root).

Module Page (Sample Module):
- SamplePage.razor (module) with associated SamplePage.razor.css styling.

Shared Components:
- NavigationItem.razor (Host & Mocks versions).
- EventConsole.razor (Host & Mocks versions).
- RenderOnceComponent.razor (Host & Mocks versions).

Service Abstractions:
- ModulePageService aggregates pages from IModule implementations and adds static shell pages ("Services", "Explorer").

Data/Model Objects (UI context):
- ModulePage: navigation + metadata (Name, Path, Icon, Title, Description, state flags New/Updated/Pro, Tags, Children, Toc).
- ModulePageSection: table of contents section (Text, Anchor).
- KeyboardShortcut: Key, Action (Used in Dashboard side, not shown wired in provided code) (Unverified usage in shell UI).

4. Detailed Elements
Routing:
- Router configured with AppAssembly and AdditionalAssemblies referencing sample module assembly for page discovery.
- Module pages appear under defined Path values (e.g., /sample/main).

Navigation & Discovery:
- ModulePageService holds flattened page collection; FindCurrent(Uri) resolves current page by absolute path match.
- TitleFor/DescriptionFor methods supply page metadata except for Overview pages (special-case logic).

State Management:
- No explicit global state container (Fluxor/Mediator) present; simple service for navigation state.

Theming & Styling:
- site.css includes highlight.js syntax styling, Radzen theme variable usage, layout adjustments, animations, error UI styling, responsive breakpoints.
- SamplePage.razor.css defines `.my-component` style (demo) referencing background image (background.png) (file existence not verified) (Unverified asset).
- RadzenQueryStringThemeService registered for dynamic theme handling.

JavaScript Interop:
- exampleJsInterop.js exports showPrompt(message) for dynamic prompt usage (actual invocation in components not shown) (Unverified usage).

Accessibility:
- No explicit ARIA attributes or guidance; reliance on Radzen defaults (Unverified).

Error Display:
- site.css defines #blazor-error-ui and .blazor-error-boundary styles for runtime error surfacing.

5. Cross-Cutting Concerns (Frontend)
- DI supplies ModulePageService scoped per request circuit.
- Authentication state cascaded (AddCascadingAuthenticationState) enabling <AuthorizeView> usage (not shown) (Unverified actual usage).
- Output cache registered (potential benefit for static resources) (Unverified application to UI endpoints/components).

6. Non-Functional Characteristics
- Performance: Default Blazor Server model; no pre-render customization beyond interactive server render mode.
- Scalability: Blazor Server circuits (CircuitOptions.DetailedErrors enabled for dev; may impact performance) (Development only).
- Reliability: Error boundary visuals present; no custom circuit error recovery strategy.
- Security: Auth integrated; pages not individually annotated with [Authorize] (Unverified access control granularity).
- Accessibility: Styling heavy; no documented focus management (Unverified).

7. Gaps & Unknowns
- No component-level tests present.
- No documented accessibility strategy.
- No global state management pattern beyond ModulePageService.
- No localization usage in shell (Mocks register localization) (Unverified if shell localizes).
- JavaScript interop usage not evidenced.
- Absence of dark/light theme toggle logic (Radzen theme service present) (Unverified UI integration).

8. Future Indicators
- No TODO markers found referencing UI.

9. Traceability
- Router: src/Shell/UKHO.ADDS.Management.Host/Shell/AppRouter.razor
- Module abstractions: src/Shell/UKHO.ADDS.Management.Shell/Modules/*.cs
- ModulePageService: src/Shell/UKHO.ADDS.Management.Shell/Services/ModulePageService.cs
- Sample module page & CSS: src/Modules/UKHO.ADDS.Management.Modules.Samples/SampleModule.cs, SamplePage.razor.css
- Styles: src/Shell/UKHO.ADDS.Management.Host/wwwroot/css/site.css
- JS interop: src/Modules/UKHO.ADDS.Management.Modules.Samples/wwwroot/exampleJsInterop.js

10. Cross-References
- Architecture components spec for DI & cross-cutting.
- API functional spec for endpoint interactions (login/logout).

11. Completion Checklist
- Component inventory captured.
- Gaps enumerated.
- No future features invented.

End of Document.
