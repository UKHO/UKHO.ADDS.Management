# FSS Manager Reliability & Resilience Specification
Version: v0.01 (Draft)
Status: Draft / Baseline Extraction
Supersedes: None
Change Log:
- Initial creation from functional & architecture specs.

## 1. Scope / Purpose
Define reliability goals, resilience mechanisms (retry, chunking, error isolation) and gaps for FSS Manager v0.01.

## 2. Reliability Objectives (v0.01 Internal Tool)
| Objective | Target | Notes |
|-----------|--------|-------|
| Admin job completion clarity | 100% phase logging | Achieved via Serilog events |
| Retry transient containment | Fail fast after 3 attempts | No jitter; monitor contention |
| File transfer continuity | Cancellable operations | No resumable support yet |
| Error categorization | Validation/Transient/Permission/Unknown | Subcodes deferred |

## 3. Resilience Mechanisms
| Mechanism | Description | Scope |
|-----------|-------------|-------|
| Exponential Retry | Pure exponential up to 3 attempts | Transient HTTP failures (429/5xx) |
| Adaptive Chunking | Adjusts transfer size (4MB ? 16MB) | Upload/download large files |
| Soft Size Warning | Banner for >5GB file | User awareness only |
| Cancellable Tokens | User-triggered cancellation of transfers/jobs | Job & transfer operations |
| Error Panel Isolation | Categorizes errors; prevents cascading UI failures | All operations |

## 4. Failure Modes & Handling
| Failure Mode | Example | Handling v0.01 | Future Enhancement |
|--------------|---------|----------------|-------------------|
| Transient HTTP | 503 during search | Retry then fail log | Introduce jitter/policy per op |
| Rate Limit | 429 on upload start | Retry with delay | Adaptive backoff and hints |
| Permission | 403 ACL append | Immediate fail; permission category | Capability guidance UI (deferred) |
| Validation | 400 batch create | Categorize + toast | Inline field mapping |
| Large Transfer Timeout | Connection drop mid-transfer | User cancellation; failure log | Resumable chunk manifest |
| Unknown | Unmapped exception | Logged as unknown | Classification refinement |

## 5. Chunking Strategy
- Threshold: 32MB triggers chunk streaming pattern.
- Initial Chunk: 4MB; may increase up to MaxChunk (16MB) based on internal heuristic (simple linear growth or fixed — baseline unspecified; treat as static sizes v0.01).
- No resumable checkpoint; cancellation abandons partial progress.

## 6. Cancellation Flow
| Step | Action |
|------|--------|
| 1 | User triggers cancel (UI) |
| 2 | CancellationToken signaled in transfer/admin job |
| 3 | Current chunk/upload completes or aborts gracefully |
| 4 | Final failure event logged (Canceled) |
| 5 | UI updates job phase -> Canceled |

## 7. Logging for Reliability
Key fields: jobId, correlationId, phase, attempt, delayMs, statusCode, fileSize, elapsedMs.

## 8. Deferred Reliability Features
| Feature | Reason Deferred |
|---------|-----------------|
| Resumable Transfers | Additional manifest/storage design needed |
| SAS Offload | Requires Blob SAS integration & security review |
| Per-Operation Retry Policies | Complexity vs initial value |
| Jitter Strategy | Monitor thundering herd first |
| Automatic Retry Advisory | User simplicity in v0.01 |

## 9. Risks & Mitigations
| Risk | Impact | Mitigation v0.01 | Future Action |
|------|--------|------------------|--------------|
| No jitter | Clustered retries | Low attempt count | Add jitter/policy registry |
| Large file circuit pressure | Performance degradation | Soft warning | Offload/resumable design |
| Lack of metrics | Hard to quantify reliability | Rich logs | Introduce metrics |
| Single retry policy | Suboptimal for different ops | Simplifies baseline | Per-op policies |

## 10. Acceptance / Validation Approach
| Aspect | Validation Method |
|--------|------------------|
| Retry Behavior | Simulate transient 503/429 responses; verify 3 attempts then failure |
| Chunk Transfer | Upload test file > threshold; confirm chunk boundaries + events |
| Cancellation | Cancel in-progress transfer; ensure Canceled phase logged |
| Error Categorization | Force 400/403/500; confirm mapping |

## 11. Traceability
- Functional Spec: spec-functional-fss-manager_v0.01.md
- Architecture Spec: spec-architecture-fss-manager_v0.01.md

End of Document.
