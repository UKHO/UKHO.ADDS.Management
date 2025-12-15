# Functional Specification Document (FSD)

**Project**: ADDS Management  
**Version**: v0.03  
**Date**: 2025-12-15  
**Author**: ADDS Management Team

Supersedes: `docs/specs/spec-architecture-components_v0.02.md`

---

## 1. Executive Summary

**Purpose**  
Capture architecture-level deltas for the shared configuration subsystem and per-module multi-deployment support, building on v0.02.

**Objective**  
Define the central configuration access service, configuration file topology, and shell integration needed to support deployment-scoped configuration for each module.

### Delta Summary

| Area         | Item                                     | Status | Before (v0.02)                                   | After (v0.03)                                                                                      | Evidence (Q&A) | Notes / Impact                                            |
|--------------|------------------------------------------|--------|--------------------------------------------------|----------------------------------------------------------------------------------------------------|----------------|-----------------------------------------------------------|
| Architecture | Configuration Access Service             | Added  | Configuration service not standardised           | Shared configuration service wrapping `.NET IConfiguration`, scoped by `(deploymentId, moduleId)`  | Q2–Q5          | Centralises configuration and improves consistency        |
| Architecture | Config file topology                     | Added  | No standard `configuration.json` / `deployments.json` topology | Single `configuration.json` for all modules + per-module `deployments.json` for deployment metadata | Q1–Q3          | Simplifies deployment and configuration management        |
| UI           | Shell deployment selector (per module)   | Added  | Shell did not manage deployment selection        | Shell hosts a per-module deployment selector and feeds active `deploymentId` into module services  | Q1–Q4, Q6      | Introduces runtime deployment switching behaviour         |

---

## 2. System Overview

- Added: Architecture explicitly supports deployment-scoped configuration via a central configuration service and standard JSON file structure.

All other system overview aspects: No change since v0.02.

---

## 3. Architecture Overview

### UI Layer (Blazor Shell and Modules)

- Changed:
  - The management shell includes a **per-module deployment selector** component within each module’s region.
  - Responsibilities:
    - Load the module’s `deployments.json` on module initialisation.
    - Persist the selected deployment per module in browser-local storage (e.g., `localStorage`).
    - Expose the current `deploymentId` to module services and configuration helper calls.

### Application / Services Layer

- Added: **Configuration Access Service** (cross-cutting)
  - Inputs:
    - `deploymentId` (string).
    - `moduleId` (string).
  - Responsibilities:
    - Wrap the root `.NET IConfiguration` instance bound to `configuration.json`.
    - Enforce the hierarchical path: `Deployments:{deploymentId}:Modules:{moduleId}`.
    - Provide APIs:
      - `GetSection(deploymentId, moduleId): IConfiguration`.
      - `GetOptions<TOptions>(deploymentId, moduleId): TOptions`.
    - Coordinate validation and logging of missing settings.

### Configuration / Files

- Added: `configuration.json`
  - Single JSON file used as the central configuration source.
  - Bound into `.NET IConfiguration` at application startup.
  - Structure:
    - Root `Deployments` object whose properties are deployment IDs.
      - `<deploymentId>`:
        - `Modules` object whose properties are module IDs.
          - `<moduleId>`: object holding that module’s deployment-specific settings.
  - Scope:
    - Only non-secret values (URLs, timeouts, feature flags, etc.).

- Added: `deployments.json` (per module)
  - Per-module JSON file containing the list of deployments that the module can target.
  - Minimum fields per deployment entry:
    - `id`: string; matches a deployment key under `Deployments` in `configuration.json`.
    - `description`: string; human-friendly label displayed in the UI.

All other architecture details: No change since v0.02.

---

## 4. Functional Requirements

- No new functional components beyond those described above; all other components: No change since v0.02.

---

## 5. Non-Functional Requirements

- No additional NFRs at architecture level; see system overview and frontend specs.

---

## 6. Security Requirements

- Added: Configuration Access Service MUST NOT expose or depend on secret values from `configuration.json` / `deployments.json`; secrets are obtained from separate secure sources.

All other security requirements: No change since v0.02.

---

## 7. Data Model Overview

- Added logical entities:
  - Configuration Scope:
    - Path: `Deployments:{deploymentId}:Modules:{moduleId}`.
  - Deployment Reference:
    - Fields: `id`, `description` (as in `deployments.json`).

All other data model aspects: No change since v0.02.

---

## 8. Deployment Strategy

- Added: Architecture assumes `configuration.json` and `deployments.json` are available as content files at runtime in all environments.

All other deployment strategy aspects: No change since v0.02.

---

## 9. Known Issues / Decisions Pending

- KI-CFG-001: (Unverified) Exact naming constraints for `deploymentId` and `moduleId` (characters, length, case sensitivity) remain to be specified.

All other known issues: No change since v0.02.
