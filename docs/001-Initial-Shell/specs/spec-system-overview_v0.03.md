# Functional Specification Document (FSD)

**Project**: ADDS Management  
**Version**: v0.03  
**Date**: 2025-12-15  
**Author**: ADDS Management Team

Supersedes: `docs/specs/spec-system-overview_v0.02.md`

---

## 1. Executive Summary

**Purpose**  
Capture configuration and deployment-selection deltas on top of the adjusted v0.02 system overview.

**Objective**  
Document the introduction of deployment-scoped configuration (`configuration.json`) and per-module deployments metadata (`deployments.json`), and the way the Blazor management shell and modules use these.

### Delta Summary

| Area          | Item                                              | Status  | Before (v0.02)                                                 | After (v0.03)                                                                                                   | Evidence (Q&A) | Notes / Impact                                                                                 |
|---------------|---------------------------------------------------|---------|----------------------------------------------------------------|------------------------------------------------------------------------------------------------------------------|----------------|--------------------------------------------------------------------------------------------------|
| Architecture  | Cross-cutting configuration model                 | Added   | Configuration mentioned only via `ServiceNames` and AppHost    | Single `configuration.json` + per-module `deployments.json` underpin deployment-scoped configuration            | Q1–Q8          | Enables consistent configuration across multiple deployments of the same remote system         |
| Frontend      | Per-module deployment switching                   | Added   | No deployment selection per module                              | Blazor shell exposes a per-module deployment picker, driven by each module’s `deployments.json`                | Q1–Q4, Q6      | Users can dynamically switch deployments at runtime per module                                  |
| Cross-cutting | Deployment-scoped configuration key hierarchy     | Added   | No enforced key structure                                       | All keys scoped under `Deployments.{deploymentId}.Modules.{moduleId}.*` and accessed via a shared helper API    | Q2–Q5          | Reduces key collisions and clarifies module/deployment boundaries                              |
| Security      | Non-secret config rule for JSON files             | Added   | No explicit rule on secrets in config files                     | `configuration.json` and `deployments.json` defined as non-secret only; secrets must live in secure mechanisms   | Q8             | Avoids accidental leakage of secrets                                                            |
| Cross-cutting | Config error handling & fallbacks                 | Added   | Behaviour for invalid/missing configuration not standardised    | Blocking error always  | Q7             | Configuration error stops system                        |

---

## 2. System Overview

- Added: The system supports **multiple deployments per module** of the same remote system.
  - Each module has a `deployments.json` that defines the list of deployments (ID + description).
  - The management shell Blazor UI provides a per-module deployment selector.
- Added: Configuration responsibilities:
  - Shell:
    - Loads `deployments.json` per module.
    - Manages the active deployment per module, persisted per browser.
  - Modules:
    - Obtain configuration via a shared configuration helper that accepts `(deploymentId, moduleId)` and resolves configuration from `configuration.json`.
    - Treat configuration as deployment-scoped; any "global" module values are duplicated per deployment where required.

All other overview content from v0.02: No change.

---

## 3. Architecture Overview

- Added: Central configuration access service wrapping `.NET IConfiguration` and enforcing the hierarchy `Deployments.{deploymentId}.Modules.{moduleId}.*`.
- Added: Per-module deployment selection flow in the Blazor shell that integrates configuration, UI state, and module services.

All other architectural aspects: No change since v0.02.

---

## 4. Functional Requirements

See `docs/specs/spec-frontend-functional_v0.03.md` for detailed frontend functional requirements relating to deployment selection and configuration usage.  
All other functional areas: No change since v0.02.

---

## 5. Non-Functional Requirements

- Added: Shell UI must remain responsive when switching deployments; configuration reload for a module is scoped to that module and MUST NOT trigger a full application restart.

All other Non-Functional Requirements: No change since v0.02.

---

## 6. Security Requirements

- Added: `configuration.json` and `deployments.json` are restricted to non-secret data; secrets MUST be stored in designated secure mechanisms (e.g., Key Vault, environment variables).

All other Security Requirements: No change since v0.02.

---

## 7. Data Model Overview

- Added logical entities:
  - Deployment
    - Attributes: `deploymentId`, `description`.
    - Appears in `deployments.json` and as a key under `Deployments.{deploymentId}` in `configuration.json`.
  - Module Configuration Scope
    - Logical representation of `Deployments.{deploymentId}.Modules.{moduleId}` in `configuration.json`.

All other data model elements: No change since v0.02.

---

## 8. Deployment Strategy

- Added: Configuration files (`configuration.json`, module `deployments.json`) are treated as application content and deployed alongside the shell; environment-specific variants may be provided per environment.

All other deployment strategy aspects: No change since v0.02.

---

## 9. Known Issues / Decisions Pending

| ID         | Topic            | Description                                                                                                      | Status               | Owner | Target Date |
|------------|------------------|------------------------------------------------------------------------------------------------------------------|----------------------|-------|-------------|
| KI-CFG-001 | Identifier rules | (Unverified) Exact naming constraints for `deploymentId` and `moduleId` (characters, length, case sensitivity). | Pending clarification | TBC   | TBC         |

All other known issues: No change since v0.02.
