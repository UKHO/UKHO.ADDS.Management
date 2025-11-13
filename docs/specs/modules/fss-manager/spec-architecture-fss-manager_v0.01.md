# FSS Manager Architecture Specification
Version: v0.01 (Draft)
Status: Draft / Baseline Extraction
Supersedes: None
Change Log:
- Initial creation derived from functional spec v0.01

## 1. Scope / Purpose
Describe structural and cross-cutting architectural aspects of the FSS Manager module within the ADDS Management Shell (excluding deferred features and external mocks domain internals).

## 2. Context
- Module hosted inside existing Blazor Server shell (.NET 9).
- Integrates Kiota-generated FileShareService ReadOnly / ReadWrite clients.
- Development auth: Keycloak; Production auth: Entra ID; claims used directly (fss.read, fss.admin).
- Observability limited to Serilog structured logs + basic traces via Aspire local collector.
- Retry: pure exponential (3 attempts) applied uniformly.

## 3. Module Composition
| Component | Role | Implementation Notes |
|-----------|------|----------------------|
| Navigation Registration | Adds FSS pages (Search, Admin, Jobs) | IModule implementation registers ModulePage entries |
| Search Page | Executes attribute queries, paginates batches | Uses ReadOnly client; page size selector (25/50/100) |
| Batch Detail Page | Displays batch attributes/files/status | Composes data from search result + detail call (ReadOnly) |
| Admin Jobs Page | Hosts controls for batch creation, ACL ops, expiry | Uses ReadWrite client; background job orchestration service |
| Transfer Service | Manages adaptive chunking uploads/downloads | Config-driven threshold/initial/max chunk sizes |
| Macro Service | Expands legacy macro tokens in inputs | Stateless expansion utility invoked before requests |
| Retry Handler | Applies exponential backoff for transient failures | Central HttpClient delegating handler or policy extension |
| Error Panel Service | Aggregates and categorizes errors | Provides UI model (Validation/Transient/Permission/Unknown) |
| Logging Enricher | Adds JobId/CorrelationId to events | Serilog enrichers per scoped operation |
| Trace Instrumentation | Emits basic spans | Minimal Start/Stop spans; no metrics v0.01 |

## 4. Dependency Flow
- UI Components -> Module Services -> Kiota Clients -> FSS API.
- Retry & logging handlers wrap Kiota HTTP pipeline.
- Macro expansion occurs pre-request; no post-response transformation.

## 5. Configuration Keys (Proposed)
```
FssManager:Endpoints:ReadOnlyBaseUrl
FssManager:Endpoints:ReadWriteBaseUrl
FssManager:Transfer:ChunkThresholdMB (32)
FssManager:Transfer:InitialChunkMB (4)
FssManager:Transfer:MaxChunkMB (16)
FssManager:Transfer:PollingIntervalSeconds (2)
FssManager:Transfer:WarningThresholdGB (5)
FssManager:Retry:MaxAttempts (3)
FssManager:Retry:BaseDelayMs (500)
FssManager:Retry:MaxDelayMs (8000)
FssManager:Retry:JitterEnabled (false)
```

## 6. Cross-Cutting Concerns
| Concern | Approach v0.01 | Deferred Items |
|---------|----------------|----------------|
| Authentication | Shell OIDC integration | Role mapping externalization not needed |
| Authorization | Direct claim gate | Fine-grained action policies |
| Observability | Logs + basic spans | Metrics, enriched spans |
| Resilience | Uniform retry policy | Per-operation policies, jitter |
| Large Transfers | Adaptive chunk local streaming | SAS offload, resumable checkpoints |
| Security | Header/token redaction, internal-only | Payload inspection UI |
| Accessibility | Deferred completely | ARIA roles, focus management |

## 7. Error Handling Strategy
- Capture exception + HTTP transient states; categorize on pattern (status codes 400 validation; 403 permission; 5xx transient; else unknown).
- No user-facing retry suggestions; failed operations surface final state only.

## 8. Trace Instrumentation (Initial)
| Span Name | Trigger | Attributes |
|-----------|---------|------------|
| fss.search.execute | Search request | pageSize, criteriaCount, correlationId |
| fss.batch.detail | Batch detail retrieval | batchId, correlationId |
| fss.admin.createBatch | Create batch | batchId (post), correlationId |
| fss.admin.aclAppend | Append ACL | batchId, entryCount |
| fss.admin.aclReplace | Replace ACL | batchId, entryCount |
| fss.admin.setExpiry | Set expiry | batchId, newExpiry |
| fss.transfer.start | Upload/download start | jobId, fileName, fileSize |
| fss.transfer.complete | Upload/download finish | jobId, fileName, durationMs |

## 9. Risks & Mitigations
| Risk | Impact | Mitigation v0.01 | Future Action |
|------|--------|------------------|--------------|
| Large file resource usage | Circuit memory/timeouts | Soft warning >5GB | Offload via SAS |
| No jitter retry | Potential contention spikes | Low attempt count (3) | Introduce jitter later |
| Lack of metrics | Limited operational insight | Logging + minimal spans | Add throughput/retry metrics |
| Accessibility gap | Usability for assistive tech | Deferred | Implement baseline ARIA |
| No raw JSON UI | Debug difficulty | Rely on logs | Add dev toggle view |

## 10. Deferred Architecture Decisions
- SAS offload pipeline design (Blob direct, signed URLs).
- Resumable transfer metadata format (chunk index manifest).
- Telemetry metric taxonomy (transfer_throughput_bytes, retry_attempts_count).
- Accessibility baseline standard.
- Raw JSON inspection scope and authorization.

## 11. Traceability
- Functional spec: spec-functional-fss-manager_v0.01.md
- Desktop legacy baseline: spec-FSS-desktop-tool-requirements_v0.02.md

End of Document.
