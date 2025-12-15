# Functional Specification Document (FSD)

**Project**: ADDS Management  
**Version**: v0.03  
**Date**: 2025-12-15  
**Author**: ADDS Management Team

Supersedes: `docs/specs/spec-infra-deployment_v0.02.md`

---

## 1. Executive Summary

**Purpose**  
Capture infra/deployment deltas that clarify how configuration files (`configuration.json` and per-module `deployments.json`) are handled by build and deployment processes.

**Objective**  
Ensure infrastructure recognises these files as required non-secret configuration artefacts and that secrets are sourced from secure stores instead.

### Delta Summary

| Area      | Item                          | Status | Before (v0.02)                        | After (v0.03)                                                                                       | Evidence (Q&A) | Notes / Impact                                      |
|-----------|-------------------------------|--------|---------------------------------------|-----------------------------------------------------------------------------------------------------|----------------|-----------------------------------------------------|
| Infra     | Configuration file deployment | Added  | No explicit description of these files| `configuration.json` and per-module `deployments.json` are deployed as application content          | Q1–Q3          | Ensures infra understands required config artefacts |
| Security  | Secrets handling for config   | Added  | Secrets policy for these files unclear| Explicit requirement that these JSON files contain only non-secret config; secrets live elsewhere   | Q8             | Reduces operational security risk                   |

---

## 2. System Overview

- Added: The application runtime depends on:
  - A global `configuration.json` file.
  - One `deployments.json` file per module that supports multiple deployments.

All other infra/system overview aspects: No change since v0.02.

---

## 3. Architecture Overview

- No structural infra architecture change beyond recognising the new configuration artefacts.

All other architecture aspects: No change since v0.02.

---

## 4. Functional Requirements

- Added FR-CFG-INFRA-001:
  - Build and deployment pipelines MUST ensure that:
    - `configuration.json` is available in the application’s content directory at runtime.
    - Each module’s `deployments.json` (where applicable) is deployed alongside the module/shell assets as expected by the Blazor shell.

All other Functional Requirements: No change since v0.02.

---

## 5. Non-Functional Requirements

- No additional NFRs beyond existing infra performance/availability; No change since v0.02.

---

## 6. Security Requirements

| ID            | Category | Description                                                                                                       | Implementation |
|---------------|----------|-------------------------------------------------------------------------------------------------------------------|----------------|
| SEC-CFG-INF-001 | Data Protection | `configuration.json` and `deployments.json` used in any environment MUST NOT contain secrets (API keys, credentials, tokens). | Secrets MUST reside in secure stores (e.g., Key Vault, env vars). |
| SEC-CFG-INF-002 | Network/File Security | File system permissions for configuration files on server hosts MUST follow least-privilege principles.      | OS-level access controls apply. |

All other Security Requirements: No change since v0.02.

---

## 7. Data Model Overview

- No additional infra-level entities; refer to system and frontend specs for configuration entity details.  
All other data model aspects: No change since v0.02.

---

## 8. Deployment Strategy

| ID        | Component                 | Description                                                                                                    | Tool/Platform |
|-----------|---------------------------|----------------------------------------------------------------------------------------------------------------|---------------|
| DS-CFG-01 | Configuration JSON files  | `configuration.json` (global) and module-specific `deployments.json` files are deployed as static content with the application. | n/a           |

All other deployment strategy elements: No change since v0.02.

---

## 9. Known Issues / Decisions Pending

- KI-CFG-001: (Unverified) Exact strategy for environment-specific variants of `configuration.json` and `deployments.json` (e.g., transform vs per-env copies) is not yet documented.

All other known issues: No change since v0.02.
