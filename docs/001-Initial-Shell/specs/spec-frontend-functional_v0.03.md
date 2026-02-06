# Functional Specification Document (FSD)

**Project**: ADDS Management  
**Version**: v0.03  
**Date**: 2025-12-15  
**Author**: ADDS Management Team

Supersedes: `docs/specs/spec-frontend-functional_v0.02.md`

---

## 1. Executive Summary

**Purpose**  
Capture frontend (Blazor) deltas introducing per-module deployment selection and configuration-driven behaviour on top of v0.02.

**Objective**  
Specify how the Blazor management shell and modules expose deployment switching to end users, how selections are persisted, and how configuration binds to the active deployment.

### Delta Summary

| Area      | Item                                   | Status | Before (v0.02)                                      | After (v0.03)                                                                                   | Evidence (Q&A) | Notes / Impact                                                |
|-----------|----------------------------------------|--------|-----------------------------------------------------|-------------------------------------------------------------------------------------------------|----------------|----------------------------------------------------------------|
| Frontend  | Per-module deployment selector UI      | Added  | No deployment selection per module                  | Deployment picker per module, populated from that module’s `deployments.json`                  | Q1–Q4          | Enables runtime selection of target deployment per module     |
| Frontend  | Deployment selection persistence       | Added  | No persistence of deployment choice                 | Selected deployment stored per module in browser-local storage and restored on reload          | Q6             | Improves usability across sessions per browser               |
| Frontend  | Config binding based on active deployment | Added| Modules did not bind config by active deployment    | Modules obtain scoped configuration using `(deploymentId, moduleId)` via configuration helper  | Q2–Q5          | Ensures UI reflects correct deployment-specific behaviour     |
| Frontend  | Config error handling & UI feedback    | Added  | No defined UX for config/deployment errors          | Blocking UI if no valid deployments exist | Q7             | Provides clear feedback on misconfiguration                  |

---

## 2. System Overview

- Added: For each module hosted within the Blazor management shell:
  - A deployment selector is presented in the module’s UI area.
  - The selector is populated from that module’s `deployments.json` (deployment ID + description).
  - The active selection controls which deployment’s configuration is used for that module.

All other frontend system overview aspects: No change since v0.02.

---

## 3. Architecture Overview

- Added: Frontend now depends on the configuration access service and deployment selection state to determine which configuration scope to use per module.

All other frontend architecture aspects: No change since v0.02.

---

## 4. Functional Requirements

| ID          | Requirement Description                                                                                                                                     | Priority |
|-------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------|----------|
| FR-CFG-001  | The Blazor management shell MUST render a deployment selector within each module’s UI region where the module supports multiple deployments.               | High     |
| FR-CFG-002  | On module initialisation, the shell/module MUST load the module’s `deployments.json` and display all defined deployments (ID + description).               | High     |
| FR-CFG-003  | The selected deployment for a module MUST be persisted per browser (e.g., `localStorage`) and restored when the shell is reloaded in the same browser.     | Medium   |
| FR-CFG-004  | When loading or reloading a module, the module MUST obtain configuration from the configuration helper using the currently selected `(deploymentId, moduleId)`. | High  |
| FR-CFG-005  | If `deployments.json` is missing/invalid or a selected `deploymentId` has no configuration, the shell/module MUST attempt safe fallbacks, log warnings, and surface a non-blocking UI warning. If no valid deployment can be determined, a blocking error state MUST be shown for that module. | High |

All other functional requirements: No change since v0.02.

---

## 5. Non-Functional Requirements

| ID             | Category  | Description                                                                                                                         | Target |
|----------------|-----------|-------------------------------------------------------------------------------------------------------------------------------------|--------|
| NFR-CFG-UI-001 | Usability | The deployment selector MUST clearly indicate the currently active deployment and present human-readable descriptions.             | n/a    |
| NFR-CFG-UI-002 | Usability | Changing the active deployment for a module MUST trigger only that module’s configuration reload and UI refresh, not a full restart. | n/a    |

All other Non-Functional Requirements: No change since v0.02.

---

## 6. Security Requirements

- Added: The frontend MUST NOT expose secret values via `configuration.json` or `deployments.json` (only non-secret settings are surfaced in the UI).

All other Security Requirements: No change since v0.02.

---

## 7. Data Model Overview

| ID   | Entity        | Description                                      | Key Attributes               |
|------|---------------|--------------------------------------------------|-----------------------------|
| DM-C1| DeploymentRef | Represents a deployment entry from `deployments.json`. | `id`, `description`         |

**Key Relationships**

- A Module has many `DeploymentRef` entries (from its `deployments.json`).
- The active `DeploymentRef.id` is used to resolve the configuration scope `Deployments.{deploymentId}.Modules.{moduleId}`.

All other data model elements: No change since v0.02.

---

## 8. Deployment Strategy

- No change since v0.02.

---

## 9. Known Issues / Decisions Pending

- KI-CFG-001: (Unverified) Exact naming constraints for `deploymentId` and `moduleId` (characters, length, case sensitivity) remain to be specified.

All other known issues: No change since v0.02.
