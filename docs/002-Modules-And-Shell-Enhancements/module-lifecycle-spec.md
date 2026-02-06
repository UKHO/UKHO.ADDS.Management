# Module Lifecycle Specification

- **Work Package**: `002-Modules-And-Shell-Enhancements`
- **Output folder**: `docs/002-Modules-And-Shell-Enhancements/`

## Purpose
Define lifecycle notifications so modules can react to changes in:

- **Deployment selection** (driven by `DeploymentSelector` selection changes).
- **Configuration reload** (from local `configuration.json`).

## Lifecycle triggers
### Deployment selection
- Source UI: `DeploymentSelector.razor` raises `SelectedDeploymentIdChanged`.
- A parent/shell component updates a shell-scoped DI deployment context service.
- Modules are notified when the selected deployment id changes.
- Deployment selection is enforced as **always set** once deployments are available (default selection).

### Configuration reload
- Source of truth: local `configuration.json`.
- Current mechanism is unknown and must be discovered in the codebase.
- Modules are notified when configuration is reloaded.
- Remote configuration store integration is out of scope for this work package.

## Lifecycle method model
### Contract
- Lifecycle methods are **async** (`Task`).

### Invocation
- The shell invokes lifecycle methods **sequentially** across modules.

### Failure policy
- If a module lifecycle callback throws:
  - The shell logs the error.
  - The module is marked **unhealthy**.
  - Future lifecycle callbacks for that module are skipped.
  - Navigation for that module is disabled (non-clickable) and displays an unhealthy badge.

### Recovery
- A module recovers from unhealthy state automatically on the next successful lifecycle handling.

## UX and routing behavior for unhealthy modules
- Unhealthy modules remain visible in navigation but are disabled.
- Clicking a disabled item does nothing.
- Direct URL navigation to unhealthy module pages is blocked and redirects to **Home (`/`)**.

## Open items (for implementation planning)
- Locate how `configuration.json` is loaded today in the shell.
- Define the shell services/events used to publish:
  - deployment selection changes
  - configuration reload changes
- Define how module health state is maintained and exposed to navigation filtering.
