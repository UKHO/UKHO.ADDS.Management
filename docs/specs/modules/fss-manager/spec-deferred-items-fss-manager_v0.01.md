# FSS Manager Deferred Items Specification
Version: v0.01 (Draft)
Status: Draft / Baseline Extraction
Supersedes: None
Change Log:
- Initial creation enumerating deferred scope from functional/architecture specs.

## 1. Scope / Purpose
Capture items explicitly deferred from v0.01 release of FSS Manager to avoid ambiguity and aid future planning.

## 2. Deferred Features Summary
| Feature | Category | Rationale for Deferral |
|---------|----------|------------------------|
| Raw JSON Inspection UI | Debug/Developer UX | Prioritize core operational UI first |
| Accessibility Baseline (ARIA, Focus) | UX/Compliance | Internal tool; schedule formal review later |
| SAS Offload (Blob Direct) | Large Transfer Performance | Requires security design & infra integration |
| Resumable Transfers | Reliability | Needs chunk manifest spec and storage strategy |
| Per-Operation Retry Policies | Resilience | Complexity vs uniform baseline benefit |
| Jitter Strategy | Resilience | Monitor contention before introducing complexity |
| Telemetry Metrics (Throughput, Retry Counters) | Observability | Start with logs/traces only |
| Enriched Trace Spans (Chunk-level, ACL detail) | Observability | Reduce initial tracing overhead |
| Capability Guidance Messages (403 detailed UI) | UX/Authorization | Simplicity in error surface for v0.01 |
| Durable Job History Store | Reliability/Audit | Logs sufficient for initial release |
| Macro Usage Metrics | Observability | Lower priority vs functional completeness |
| Error Subcodes & Classification Refinement | Error Handling | Baseline categories first |
| Retry Advisory Hints | UX | Avoid user complexity; fail-fast approach |
| Accessibility Testing Suite | QA | Pending baseline implementation |

## 3. Future Release Grouping (Indicative)
| Release Target | Candidate Items |
|----------------|-----------------|
| v0.02 | Telemetry Metrics, SAS Offload design, Raw JSON dev toggle, Basic accessibility (aria-live) |
| v0.03 | Resumable transfers, Per-operation retry policies, Jitter introduction |
| v0.04 | Durable job history store, Error subcode taxonomy, Macro usage metrics |

## 4. Raw JSON Inspection (Deferred Detail)
Consider dev-only toggle (config key) with security review (no sensitive tokens). Need claim check (fss.admin) or separate developer role.

## 5. SAS Offload (Deferred Detail)
Design points: Signed URL generation, client-side direct upload/download, integrity verification (hash), lifecycle cleanup.

## 6. Resumable Transfer (Deferred Detail)
Manifest schema: jobId, chunkCount, chunkSize, completedChunks[], checksum (final). Requires persistence (table or blob metadata).

## 7. Accessibility (Deferred Detail)
Baseline targets: aria-live for toasts, role=alert for errors, focus return on dialog close, color contrast checks.

## 8. Metrics (Deferred Detail)
Planned metrics: transfer_throughput_bytes, transfer_duration_ms, retry_attempts_count, admin_job_failures_count, macro_expansions_count.

## 9. Decision Log Reference
Items map to KI10..KI19 issues across specs; consolidated here for planning alignment.

## 10. Exit Criteria from Deferred Status
An item exits deferred status when: (a) design approved, (b) configuration keys agreed, (c) test strategy outlined.

## 11. Traceability
- Functional Spec: spec-functional-fss-manager_v0.01.md
- Architecture Spec: spec-architecture-fss-manager_v0.01.md
- Reliability & Resilience Spec: spec-reliability-resilience-fss-manager_v0.01.md
- Observability Spec: spec-observability-fss-manager_v0.01.md

End of Document.
