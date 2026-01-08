# Implementation Plan – Uplift from v0.02 to v0.03

## Configuration & Deployment Selection – Foundations

- [x] **Work Item 1: Introduce central configuration model and JSON topology** – Completed (validated and logged `configuration.json`, ensured the Sample module `deployments.json` is present and emitted with the module, and documented that these files must not contain secrets).
  - **Purpose**: Establish a single, predictable configuration source (`configuration.json` + per-module `deployments.json`) underpinning deployment-scoped behaviour.
  - **Acceptance Criteria**:
    - Shell host loads `configuration.json` into `.NET IConfiguration` using the standard .NET 9 hosting model.
    - A sample `configuration.json` exists with the structure `Deployments -> {deploymentId} -> Modules -> {moduleId}` and non-secret values.
    - The Sample module has a `deployments.json` file listing at least two deployments, each with `id` and `description`.
    - Documentation clearly states that these files must not contain secrets.
  - **Definition of Done**:
    - Configuration files created, included as content, and loaded at runtime without errors.
    - Logging exists for configuration load success/failure.
    - Documentation updated to describe file locations and structures.
    - End-to-end: shell runs and an observable health/diagnostic path confirms configuration is loaded.
  - [x] **Task 1: Define configuration JSON structures and locations**
    - [x] Step 1: Create/extend `configuration.json` in the shell host content root with a root `Deployments` object containing deployment IDs, and under each, a `Modules` object with module IDs and example settings.
    - [x] Step 2: Create `deployments.json` for the Sample module under its project with entries that include `id` and `description` per deployment.
    - [x] Step 3: Ensure both files are marked as content and copied to output in the relevant `.csproj` files.
    - [x] Step 4: Add a short markdown note under `docs/` or comments noting that no secrets are allowed in these JSON files.
  - [x] **Task 2: Wire `configuration.json` into the shell host configuration pipeline**
    - [x] Step 1: Review `Program.cs` in `UKHO.ADDS.Management.Host` to see current configuration sources.
    - [x] Step 2: Add `configuration.json` as an additional configuration source (if not present), ensuring correct merge order with other config (e.g., `appsettings.*`).
    - [x] Step 3: Add startup logging to report successful load and missing/invalid file conditions.
  - **Files**:
    - `configuration.json`: central configuration file (new/updated).
    - `src/Modules/UKHO.ADDS.Management.Modules.Samples/deployments.json`: deployments list for Sample module (new).
    - `src/Shell/UKHO.ADDS.Management.Host/UKHO.ADDS.Management.Host.csproj`: content inclusion for `configuration.json` if required.
    - `src/Modules/UKHO.ADDS.Management.Modules.Samples/UKHO.ADDS.Management.Modules.Samples.csproj`: content inclusion for `deployments.json` if required.
    - `docs/specs/spec-system-overview_v0.03.md`: specification reference.
  - **Work Item Dependencies**: None.
  - **Run / Verification Instructions**:
    - Build and run `UKHO.ADDS.Management.Host`.
    - Inspect logs to confirm `configuration.json` load.
    - Optionally expose a temporary diagnostic endpoint that returns a known configuration value.

- [ ] **Work Item 2: Implement configuration access service scoped by `(deploymentId, moduleId)`**
  - **Purpose**: Provide a single abstraction to read deployment- and module-scoped configuration using `Deployments:{deploymentId}:Modules:{moduleId}`.
  - **Acceptance Criteria**:
    - DI-registered service exposes:
      - `IConfiguration GetSection(string deploymentId, string moduleId);`
      - `TOptions GetOptions<TOptions>(string deploymentId, string moduleId) where TOptions : class, new();`
    - Missing sections result in warnings and safe defaults.
    - No secrets are exposed or required.
  - **Definition of Done**:
    - Interface and implementation added and wired in DI.
    - Unit tests cover path resolution, missing-section handling, and typed binding.
    - Usage documented for module developers.
    - End-to-end: test endpoint or page uses the provider with a known `(deploymentId, moduleId)`.
  - [x] **Task 1: Define interface and implementation**
    - [x] Step 1: Add `IModuleConfigurationProvider` with `GetSection` and `GetOptions<TOptions>`.
    - [x] Step 2: Implement `ModuleConfigurationProvider` using injected `IConfiguration` to build the `Deployments:{deploymentId}:Modules:{moduleId}` path.
    - [x] Step 3: Add logging for missing sections and failed binding.
  - [x] **Task 2: Register provider in DI and add tests**
    - [x] Step 1: Register the provider in `Program.cs` using appropriate lifetime.
    - [x] Step 2: Add unit tests verifying correct paths and behaviour under missing/invalid config. – Completed (added `tests/Shell/UKHO.ADDS.Management.Shell.Tests` with `ModuleConfigurationProviderTests` covering section retrieval, binding, missing sections, and binding failures; referenced Shell project; ensured .NET 10 target).
  - **Files**:
    - `src/Shell/UKHO.ADDS.Management.Shell/Configuration/IModuleConfigurationProvider.cs`.
    - `src/Shell/UKHO.ADDS.Management.Shell/Configuration/ModuleConfigurationProvider.cs`.
    - `src/Shell/UKHO.ADDS.Management.Host/Program.cs` (registration).
    - `tests/Shell/UKHO.ADDS.Management.Shell.Tests/UKHO.ADDS.Management.Shell.Tests.csproj`.
    - `tests/Shell/UKHO.ADDS.Management.Shell.Tests/ModuleConfigurationProviderTests.cs`.
  - **Work Item Dependencies**:
    - Depends on Work Item 1.
  - **Run / Verification Instructions**:
    - Run tests.
    - Optionally call the provider from a temporary Razor page to display configuration values.

## Per-Module Deployment Selection (Blazor UI)

- [x] **Work Item 3: Add per-module deployment selector UI and basic state** – Completed (Radzen dropdown selector wired; deployments loaded and bound; module state updates on selection; error shown when `deployments.json` missing/invalid.)
  - **Purpose**: Show deployments defined in a module’s `deployments.json` and allow users to select them.
  - **Acceptance Criteria**:
    - Sample module page displays a deployment selector listing deployments from `deployments.json`.
    - Default selection is first valid deployment.
    - Selection updates active deployment state in the module.
  - **Definition of Done**:
    - Reusable Blazor component for deployment selection exists.
    - `deployments.json` is loaded at runtime and bound to the selector.
    - Missing/invalid `deployments.json` triggers a visible error and log entry.
  - [x] **Task 1: Implement loader for `deployments.json`**
    - [x] Step 1: Create `DeploymentRef` model with `id` and `description`. – Added `src/Shell/UKHO.ADDS.Management.Shell/Models/DeploymentRef.cs`.
    - [x] Step 2: Implement a loader service to fetch and deserialize `deployments.json` for a module. – Added `src/Shell/UKHO.ADDS.Management.Shell/Services/DeploymentsJsonLoader.cs` with error handling and logging.
    - [x] Step 3: Handle IO/JSON errors with logs and an error result. – Implemented `DeploymentsLoadResult` with `HasError` + message, updated loader and Sample page to display blocking error.
  - [x] **Task 2: Create `DeploymentSelector` Blazor component**
    - [x] Step 1: Build component with `IEnumerable<DeploymentRef>` input and `SelectedDeploymentId` + change callback. – Added `src/Shell/UKHO.ADDS.Management.Shell/Components/DeploymentSelector.razor`.
    - [x] Step 2: Render as dropdown (Radzen) aligned with shell styling. – Updated component to use `RadzenDropDown` with `TextProperty`/`ValueProperty`.
  - [x] **Task 3: Integrate selector into Sample module UI**
    - [x] Step 1: Update `SamplePage.razor` to load deployments and render selector. – Updated page to use `DeploymentsJsonLoader`, display errors, and bind selector.
    - [x] Step 2: Maintain a `SelectedDeploymentId` in component state and update it on selector change. – Implemented state update and options binding.
  - **Files**:
    - `src/Shell/UKHO.ADDS.Management.Shell/Models/DeploymentRef.cs`.
    - `src/Shell/UKHO.ADDS.Management.Shell/Services/DeploymentsJsonLoader.cs`.
    - `src/Shell/UKHO.ADDS.Management.Shell/Components/DeploymentSelector.razor`.
    - `src/Modules/UKHO.ADDS.Management.Modules.Samples/Pages/SamplePage.razor`.
  - **Work Item Dependencies**:
    - Depends on Work Item 1.
  - **Run / Verification Instructions**:
    - Run host and navigate to Sample module page to verify selector behaviour.

- [x] **Work Item 4: Persist deployment selection per module using browser storage** – Completed (JS helper and C# interop added; Sample module reads/writes selection and validates against available deployments.)
  - **Purpose**: Remember a user’s selected deployment for each module per browser.
  - **Acceptance Criteria**:
    - Selected deployment is stored (e.g. in `localStorage`) keyed by module ID.
    - On load, stored selection is applied if valid; otherwise fallbacks to first deployment and updates storage.
  - **Definition of Done**:
    - JS interop wrapper for local storage implemented.
    - Integration with selector/module state complete.
    - Manual tests confirm selection persists across reloads.
  - [x] **Task 1: Add local storage helper**
    - [x] Step 1: Add JS helper for `localStorage` read/write. – Added `wwwroot/js/deploymentSelection.js` and referenced from `App.razor`.
    - [x] Step 2: Add C# interop service (e.g. `DeploymentSelectionStorage`) to call JS helper. – Implemented and registered in DI.
  - [x] **Task 2: Wire storage into module UI**
    - [x] Step 1: On module init, read stored deployment for `moduleId` and validate against loaded deployments. – Implemented fallback to first.
    - [x] Step 2: Use stored ID if valid, else default to first deployment and update storage. – Implemented.
    - [x] Step 3: On user change, update storage immediately. – Implemented.
  - **Files**:
    - `src/Shell/UKHO.ADDS.Management.Host/wwwroot/js/deploymentSelection.js`.
    - `src/Shell/UKHO.ADDS.Management.Shell/Services/DeploymentSelectionStorage.cs`.
    - `DeploymentSelector.razor` or parent module component: updated to use storage.
  - **Work Item Dependencies**:
    - Depends on Work Item 3.
  - **Run / Verification Instructions**:
    - Run host, select deployment, reload page/browser, and verify persistence.

## Module Behaviour & Error Handling

- [ ] **Work Item 5: Use configuration provider in Sample module as a vertical slice**
  - **Purpose**: Demonstrate per-deployment behaviour in a module driven by configuration.
  - **Acceptance Criteria**:
    - `SampleModuleOptions` is bound from `configuration.json` via `IModuleConfigurationProvider`.
    - Behaviour changes across deployments (e.g., different labels/URLs).
  - **Definition of Done**:
    - Sample module reads typed options for active `(deploymentId, moduleId)`.
    - Visible behaviour differs across deployments.
    - Unit tests cover binding and module behaviour.
  - [ ] **Task 1: Define `SampleModuleOptions` and config values**
    - [ ] Step 1: Create `SampleModuleOptions` with properties such as `BaseUrl`, `DisplayName`, etc.
    - [ ] Step 2: Populate `configuration.json` with differing values for each Sample module deployment.
  - [ ] **Task 2: Integrate provider into Sample module**
    - [ ] Step 1: Inject `IModuleConfigurationProvider` into Sample module/page.
    - [ ] Step 2: On init and deployment change, call `GetOptions<SampleModuleOptions>(deploymentId, moduleId)`.
    - [ ] Step 3: Use options to modify UI or downstream calls.
  - [ ] **Task 3: Implement error handling at module level**
    - [ ] Step 1: Mark required properties; handle missing ones with defaults and warnings.
    - [ ] Step 2: For missing configuration sections, apply fallback or show blocking error according to spec.
  - **Files**:
    - `src/Modules/UKHO.ADDS.Management.Modules.Samples/Configuration/SampleModuleOptions.cs`.
    - Sample module pages/services.
    - `configuration.json` updates.
  - **Work Item Dependencies**:
    - Depends on Work Items 2–4.
  - **Run / Verification Instructions**:
    - Run host, navigate to Sample module, switch deployments, and observe behaviour changes.

- [ ] **Work Item 6: Implement consistent configuration error handling & UX**
  - **Purpose**: Align runtime behaviour with spec for configuration and deployment data issues.
  - **Acceptance Criteria**:
    - Missing/empty `deployments.json` -> blocking module error.
    - Invalid stored `deploymentId` -> fallback to first valid deployment + warning.
    - Missing/incomplete configuration -> blocking error for any configuration issues.
  - **Definition of Done**:
    - Shared components exist for warnings and blocking errors.
    - Error behaviour implemented in loader, selection, and module-level config handling.
  - [ ] **Task 1: Define shared UI for warnings/errors**
    - [ ] Step 1: Implement a blocking error view/state for all configuration errors
    - [ ] Step 2: Implement a blocking error view/state when module cannot function.
  - [ ] **Task 2: Wire validation into flows**
    - [ ] Step 1: In `DeploymentsJsonLoader`, detect missing/empty file and surface blocking error.
    - [ ] Step 2: In module init, handle invalid stored `deploymentId` via fallback and logging.
    - [ ] Step 3: In options consumption, decide which errors are non-blocking vs blocking and update UI.
  - **Files**:
    - `src/Shell/UKHO.ADDS.Management.Shell/Components/ConfigWarning.razor` (or similar).
    - Updated loader and Sample module pages/services.
  - **Work Item Dependencies**:
    - Depends on Work Items 1–5.
  - **Run / Verification Instructions**:
    - Intentionally break config in dev and verify UI + logging paths.

## Testing & Documentation

- [ ] **Work Item 7: Testing and documentation consolidation**
  - **Purpose**: Ensure the v0.03 uplift is robust and clearly documented.
  - **Acceptance Criteria**:
    - Unit tests cover provider, loaders, and selection logic.
    - Where infra exists, an integration-style test exercises end-to-end flow.
    - Docs and specs are cross-checked and consistent.
  - **Definition of Done**:
    - All new tests pass in CI.
    - Documentation for configuration and deployments is updated.
  - [ ] **Task 1: Strengthen test coverage**
    - [ ] Step 1: Add unit tests for `ModuleConfigurationProvider`, `DeploymentsJsonLoader`, and selection storage.
    - [ ] Step 2: Add Blazor component tests for `DeploymentSelector` if bUnit is available.
  - [ ] **Task 2: Update documentation**
    - [ ] Step 1: Add or update `docs/` markdown describing how to define deployments and module configuration.
    - [ ] Step 2: Cross-link `spec-system-overview_v0.03`, `spec-architecture-components_v0.03`, `spec-frontend-functional_v0.03`, and `spec-infra-deployment_v0.03`.
  - **Files**:
    - Test project files and new test classes.
    - `docs/` markdown files.
  - **Work Item Dependencies**:
    - Depends on Work Items 1–6.
  - **Run / Verification Instructions**:
    - Run the full test suite and manually verify UI flows.

---

# Architecture

## Overall Technical Approach

- Extend the existing .NET 9 Blazor Server host with a central configuration system based on `configuration.json`.
- Introduce per-module `deployments.json` files to describe deployment options.
- Provide `IModuleConfigurationProvider` to map `(deploymentId, moduleId)` to config sections and typed options.
- Enhance shell UI with per-module deployment selectors and client-side persistence.

## Frontend

- Blazor Server host renders shell layout and module regions.
- Each module page:
  - Loads `deployments.json` via `DeploymentsJsonLoader`.
  - Renders `DeploymentSelector.razor` for users to choose a deployment.
  - Persists chosen deployment per module using local storage interop.
  - Passes active `deploymentId` to module logic and `IModuleConfigurationProvider`.

## Backend

- Host configuration pipeline includes `configuration.json`.
- `IModuleConfigurationProvider`:
  - Injects root `IConfiguration`.
  - Builds `Deployments:{deploymentId}:Modules:{moduleId}` path.
  - Returns a section or typed options.
  - Logs missing/malformed configuration.
- Modules:
  - Define typed options classes bound to deployment-scoped config.
  - Use options to alter behaviour (e.g. URLs, feature flags).
- CI/CD ensures that `configuration.json` and `deployments.json` are deployed as non-secret content and that secrets remain in secure stores.
