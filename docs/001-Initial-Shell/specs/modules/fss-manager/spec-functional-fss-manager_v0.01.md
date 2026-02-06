# Functional Specification Document (FSD)

**Project**: ADDS Management Shell – FSS Manager Module  
**Version**: v0.01 (Draft)  
**Date**: (TBC)  
**Author**: (TBC)

---
## 1. Executive Summary
**Purpose**  
Provide an internal Blazor (.NET 9) module to replace the legacy FileShareService desktop client, centralising batch/file operational and administrative capabilities.

**Objective**  
Migrate desktop functionality (attribute search, batch detail, unlimited adaptive chunked transfers, batch creation, ACL append/replace, expiry, macro expansion, resilience handling, event refresh) using Kiota clients (ReadOnly / ReadWrite) inside the ADDS Management Shell. Auth uses Keycloak in development and Entra ID in production via direct claims (fss.read / fss.admin). First release defers raw JSON inspection, telemetry enrichment, accessibility, SAS offload and resumable transfer design.

---
## 2. System Overview
**Core Capabilities**  
- Attribute-based batch search with selectable page size (25 / 50 / 100).  
- Batch detail view (attributes, files, expiry, ACL indicators, status).  
- Unlimited file upload/download (soft warning > 5GB) with adaptive chunking (threshold 32MB; initial 4MB; max 16MB; poll 2s) and background job UI.  
- Administrative actions: create batch, ACL append, ACL replace, set expiry.  
- Full legacy macro token set (dates, offsets, week numbers) across file paths, attributes, expiry, ACL inputs.  
- Structured error handling (toast + categorized panel: Validation / Transient / Permission / Unknown).  
- Pure exponential retry (3 attempts, base delay 500ms, no jitter).  
- Event-driven UI refresh after admin changes.  
- Logging & basic traces (search, batch detail, admin ops, transfer start/end) to local Aspire collector.  
- Raw JSON inspection fully deferred.  
- Accessibility fully deferred (Gap).  

---
## 3. Architecture Overview
| Component | Description |
|-----------|-------------|
| **Architecture Style** | Modular Blazor Server (IModule registration) |
| **Communication Mechanisms** | HTTP via Kiota generated FSS ReadOnly / ReadWrite clients |
| **Authentication & Authorization** | Keycloak (dev) / Entra ID (prod); claims fss.read / fss.admin direct evaluation |
| **Hosting Platform** | ADDS Management Shell (.NET 9, server-side Blazor) |
| **Observability (v0.01)** | Serilog structured logs + basic OpenTelemetry traces to local Aspire (no metrics) |
| **Transfer Handling** | Adaptive chunking + background job UI; SAS offload deferred |
| **Configuration** | AppSettings: endpoints, chunk thresholds, polling, retry settings, 5GB warning threshold |
| **Retry Strategy** | Pure exponential (MaxAttempts=3, BaseDelayMs=500, MaxDelayMs=8000) |
| **Security** | Internal tool; direct claim gating; headers/tokens redacted in logs |

---
## 4. Functional Requirements
### 4.1 Search & Batch Exploration
| ID | Requirement Description | Priority |
|----|-------------------------|----------|
| FR1 | Provide attribute-based search with page sizes 25/50/100. | High |
| FR2 | Clear previous results before new search execution. | High |
| FR3 | Display batch list with pagination controls and range summary. | High |
| FR4 | Surface validation / transient / permission errors in categorized panel. | High |
| FR5 | Refresh search results on relevant batch state change events (expiry). | Medium |

### 4.2 Batch Detail & File Operations
| ID | Requirement Description | Priority |
|----|-------------------------|----------|
| FR6 | Show batch attributes, file list, expiry and ACL indicators. | High |
| FR7 | Initiate cancellable download with adaptive chunking boundaries. | High |
| FR8 | Warn (soft banner) when file size >5GB before transfer start. | Medium |
| FR9 | Log structured transfer start/end events (JobId, CorrelationId, FileSize). | High |

### 4.3 Administrative Jobs
| ID | Requirement Description | Priority |
|----|-------------------------|----------|
| FR10 | Create batch (attributes, ACL, expiry) before uploads. | High |
| FR11 | Append ACL entries to existing batch. | High |
| FR12 | Replace ACL entirely. | Medium |
| FR13 | Set expiry for a batch. | High |
| FR14 | Provide background job UI (phases: Queued, Uploading, Committing, Completed, Failed, Canceled). | High |
| FR15 | Capture and log all job phase transitions (Serilog events forwarded to Aspire). | High |

### 4.4 Macro Expansion
| ID | Requirement Description | Priority |
|----|-------------------------|----------|
| FR16 | Expand full legacy macro set across input fields pre-request. | High |
| FR17 | Iteratively resolve nested macros until none remain. | Medium |

### 4.5 Retry & Resilience
| ID | Requirement Description | Priority |
|----|-------------------------|----------|
| FR18 | Load retry config (MaxAttempts=3, BaseDelayMs=500) from AppSettings with fallback defaults. | High |
| FR19 | Apply pure exponential backoff without jitter to transient HTTP failures (429/5xx). | High |
| FR20 | Log each retry attempt (attempt#, delay, status code, operation type). | High |
| FR21 | Emit final failure log event on exhausting attempts (Attempts=3). | High |
| FR22 | Suppress advisory or escalation hints in v0.01 (fail and log only). | High |

### 4.6 Error Handling & Notifications
| ID | Requirement Description | Priority |
|----|-------------------------|----------|
| FR23 | Show toast (success/failure summary) for search, admin job, transfer completion. | High |
| FR24 | Present categorized error panel with expandable sections. | High |
| FR25 | Redact sensitive headers/tokens before logging/display. | High |
| FR26 | Provide copy action for error details (sanitized). | Low |

### 4.7 Observability (v0.01 Scope)
| ID | Requirement Description | Priority |
|----|-------------------------|----------|
| FR27 | Emit structured Serilog events for all job phases and retry attempts. | High |
| FR28 | Export basic traces (search, batch detail, admin ops, transfer start/end) to local Aspire. | Medium |
| FR29 | No metrics (throughput/retry counters) in v0.01; mark deferred. | High |

### 4.8 Deferred Features
| ID | Requirement Description | Priority |
|----|-------------------------|----------|
| FR30 | Raw JSON inspection UI (search/admin responses) deferred. | High |
| FR31 | Accessibility (ARIA roles, focus management) fully deferred. | High |
| FR32 | SAS offload & resumable transfers deferred. | High |
| FR33 | Per-operation differentiated retry policies deferred. | Medium |
| FR34 | Telemetry metrics & enriched spans deferred. | High |

---
## 5. Non-Functional Requirements
| ID | Category | Description | Target |
|----|----------|-------------|--------|
| NFR1 | Performance | Typical interactive operations (<200ms excluding large transfer). | Unverified |
| NFR2 | Reliability | All admin phases logged; final failures surfaced clearly. | Required |
| NFR3 | Scalability | Support internal concurrent use; large transfer offload deferred. | Unverified |
| NFR4 | Maintainability | Central config keys; deferred features isolated. | Required |
| NFR5 | Observability | Logs + basic traces only; metrics deferred. | Required |
| NFR6 | Security | Internal only; sensitive data redaction enforced. | Required |
| NFR7 | Accessibility | Deferred entirely (Gap). | Gap |
| NFR8 | Resilience | Consistent retry with bounded attempts (no jitter). | Required |

---
## 6. Security Requirements
| ID | Category | Description | Implementation |
|----|----------|-------------|----------------|
| SR1 | Authentication | Keycloak (dev) / Entra ID (prod) OIDC tokens. | Existing Shell OIDC setup |
| SR2 | Authorization | Claims fss.read / fss.admin control feature visibility. | Direct evaluation |
| SR3 | Data Protection | No raw payload UI; sensitive headers not logged. | Redaction filters |
| SR4 | Audit & Logging | Structured job & retry events archived via Aspire logs. | Serilog + OTel |
| SR5 | Transport Security | HTTPS enforced by shell hosting; internal network. | Platform configuration |
| SR6 | Access Control | No public endpoints; internal role claims only. | Environment policy |

---
## 7. Data Model Overview
| ID | Entity | Description | Key Attributes |
|----|--------|-------------|----------------|
| DM1 | Batch | Represents FSS batch with attributes/files. | Id, Attributes[], Files[], Expiry, AclFlags, Status |
| DM2 | FileTransferJob | Upload/download lifecycle. | JobId, BatchId, FileName, Size, Phase, StartTime, EndTime |
| DM3 | AdminJob | Generic admin operation record. | JobId, Type, BatchId, Phase, User, Started, Completed |
| DM4 | RetryEvent | Logged retry attempt context. | OperationType, Attempt, DelayMs, StatusCode, CorrelationId |
| DM5 | ErrorRecord | Categorized error surfaced to UI. | Id, Category, Message, Timestamp, CorrelationId |

Relationships:
- Batch → Files (1:many)
- AdminJob / FileTransferJob → RetryEvent (1:many)
- Job → ErrorRecord (1:many)

---
## 8. Deployment Strategy
| ID | Component | Description | Tool/Platform |
|----|-----------|-------------|---------------|
| DS1 | CI/CD | Standard shell pipeline (build, test, deploy). | GitHub Actions (internal) |
| DS2 | Observability | Local Aspire collector for logs/traces. | .NET Aspire |
| DS3 | Environments | Dev (Keycloak) / Prod (Entra ID). | AppHost / Cloud hosting |
| DS4 | Config | AppSettings for FssManager:Transfer & Retry keys. | Configuration provider |
| DS5 | Feature Flags | Deferred features tagged for future toggles. | AppSettings/FeatureFlag (future) |
| DS6 | Large Transfer Future | Plan SAS offload & resumable design v0.02+. | Deferred |

---
## 9. Known Issues / Decisions Pending
| ID | Topic | Description | Status |
|----|-------|-------------|--------|
| KI10 | Durable job history | Only logs; no separate persistence. | Accepted |
| KI11 | SAS offload | Offload & resumable strategy pending. | Pending |
| KI15 | No jitter | Thundering herd risk monitored. | Accepted |
| KI16 | Retry escalation | Possible future increase if transient rate high. | Pending |
| KI17 | Raw JSON inspection | Entire feature deferred. | Deferred |
| KI18 | Accessibility | No ARIA/focus features in v0.01. | Gap |
| KI19 | Metrics | Throughput & retry counters deferred. | Deferred |

---
End of Document.
