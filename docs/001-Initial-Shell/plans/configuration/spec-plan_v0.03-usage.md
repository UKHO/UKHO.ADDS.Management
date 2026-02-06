# v0.03 Configuration & Deployments Usage Guide

This document supplements `docs/plans/configuration/spec-plan_v0.03.md` with concrete examples for module authors.

## Files and locations

### Host: `configuration.json`
- Location: `src/Shell/UKHO.ADDS.Management.Host/configuration.json`
- Purpose: central configuration keyed by deployment id and module id.

Shape:
- `Deployments:{deploymentId}:Modules:{moduleId}:<settings>`

Example (Sample module):
- `Deployments:dev:Modules:Samples:BaseUrl`
- `Deployments:dev:Modules:Samples:DisplayName`
- `Deployments:dev:Modules:Samples:TimeoutSeconds`

### Module: `deployments.json`
- Location (Sample module): `src/Modules/UKHO.ADDS.Management.Modules.Samples/deployments.json`
- Purpose: list of available deployments for a specific module.

Shape:
- An array of objects: `{ "id": "dev", "description": "Development" }`

## Reading configuration in a module

Modules should use `IModuleConfigurationProvider` to read deployment-scoped configuration.

- Section path: `Deployments:{deploymentId}:Modules:{moduleId}`
- Typed binding: `GetOptions<TOptions>(deploymentId, moduleId)`

The Sample module demonstrates this in `SamplePage.razor` via `SamplePageState.BindOptions(...)`.

## Deployment selection persistence

Deployment selection is stored per module in browser storage.

- C# service: `DeploymentSelectionStorage`
- JS helper: `src/Shell/UKHO.ADDS.Management.Host/wwwroot/js/deploymentSelection.js`

Keying:
- The `moduleId` string is used as the storage key.

Behavior:
- On load: if stored deployment id is valid, it is used.
- If invalid: the UI falls back to the first deployment and shows a warning.

## Security

Do not store secrets in either:
- `configuration.json`
- module `deployments.json`

Secrets should be provided via secure stores and injected at runtime.
