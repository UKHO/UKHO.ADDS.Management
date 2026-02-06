# Specification Overview — Modules and Shell Enhancements

- **Work Package**: `002-Modules-And-Shell-Enhancements`
- **Output folder**: `docs/002-Modules-And-Shell-Enhancements/`

## 1. Overview
This work package enhances the Blazor management shell by:

- Adding skeleton modules for File Share Service management and Permit Service management.
- Extending the shell module framework with async, sequential lifecycle notifications to react to:
  - Deployment selection changes (driven by the UI `DeploymentSelector` component).
  - Runtime configuration reload (local `configuration.json`).
- Adding RBAC-gated UI to the Sample Module via a new page route and a role-gated button.
- Introducing a minimal Access Denied page/route.

Navigation is assembled by `ModulePageService`, which aggregates the "Home" page plus each module’s `IModule.Pages`.

## 2. High-level Goals / Non-Goals
### Goals
- **New skeleton module: File Share Service management**
- **New skeleton module: Permit Service management**
- Add module lifecycle hooks to support deployment and configuration changes.
- Add Sample Module RBAC page + button.
- Add an Access Denied page/route for unauthorized redirects.

### Non-Goals
- Implementing full management functionality for File Share Service or Permit Service.
- Integrating with a remote configuration store (explicitly deferred to a later work package).

## 3. System / Component Scope (high level)
### Shell
- `IModule` contract changes to include async lifecycle callbacks.
- Deployment context state service (DI) for current selected deployment id.
- Configuration reload notifier for `configuration.json`.
- Module health tracking (unhealthy modules disabled in navigation).

### Modules
- New modules:
  - File Share Service module (skeleton)
  - Permit Service module (skeleton)
- Sample module updates:
  - New page `sample/secure`
  - Role-gated button on that page

### Navigation + Authorization
- Extend `ModulePage` with required role metadata (multiple roles; any-of semantics).
- `ModulePageService` filters pages by current user roles and module health.
- Unauthorized pages are removed from navigation.
- Unhealthy modules remain visible but disabled and show an unhealthy badge.
- Clicking a disabled unhealthy nav item does nothing.
- Direct URL access to an unhealthy module page redirects to Home.
- Unauthorized (role) access redirects to Access Denied.

## Referenced component specifications
- `docs/002-Modules-And-Shell-Enhancements/module-lifecycle-spec.md`
- `docs/002-Modules-And-Shell-Enhancements/modules-file-share-service-spec.md`
- `docs/002-Modules-And-Shell-Enhancements/modules-permit-service-spec.md`
- `docs/002-Modules-And-Shell-Enhancements/sample-module-rbac-ui-spec.md`
