# FSS Manager Observability Specification
Version: v0.01 (Draft)
Status: Draft / Baseline Extraction
Supersedes: None
Change Log:
- Initial creation based on functional & architecture specs.

## 1. Scope / Purpose
Define logging, tracing and deferred telemetry strategy for FSS Manager v0.01; identify gaps for v0.02.

## 2. Current Observability Stack (v0.01)
| Layer | Tooling | Notes |
|-------|---------|-------|
| Logging | Serilog | Structured events enriched with JobId, CorrelationId, UserId, Capability |
| Tracing | OpenTelemetry (basic spans) | Export OTLP to local Aspire collector; no remote export |
| Metrics | Deferred | No custom counters/gauges/histograms yet |
| Dashboards | Aspire | Basic log & trace viewing; no custom panels |

## 3. Logging Event Schema
| Event Type | Mandatory Fields | Optional Fields | Redaction |
|------------|------------------|-----------------|-----------|
| JobPhase | timestamp, level, jobId, phase, operationType, correlationId | durationMs, batchId | headers, tokens removed |
| RetryAttempt | timestamp, jobId/operationType, attempt, delayMs, statusCode | correlationId | Same redaction |
| FinalFailure | timestamp, operationType, attempts, statusCode, correlationId | fileSize, batchId | Same |
| TransferStart | timestamp, jobId, fileName, fileSize, correlationId | chunkThresholdMB | Same |
| TransferComplete | timestamp, jobId, fileName, durationMs, success | avgChunkSize | Same |
| ErrorRecord | timestamp, category, message, correlationId | batchId, jobId | Sensitive stripped |

## 4. Trace Span Inventory
Spans (from architecture spec) retained; minimal attributes only. No nested chunk spans to reduce cardinality.

## 5. Correlation Strategy
- Generate CorrelationId per high-level operation (search, admin job, transfer).
- Propagate CorrelationId across logging and trace spans.
- BatchId / JobId added when available (post-create).

## 6. Sampling & Retention
| Aspect | v0.01 Approach | Future Consideration |
|--------|----------------|----------------------|
| Log Level | Info + Warning + Error | Dynamic level per environment |
| Trace Sampling | Always on (low internal volume) | Probability sampler for scale |
| Retention | External to module (Aspire default) | Central policy (days) |
| PII Handling | No user personal data stored; only technical claims (UserId). | Additional scrubbing review |

## 7. Deferred Metrics
| Metric (Planned) | Type | Rationale |
|------------------|------|-----------|
| transfer_throughput_bytes | Histogram | Analyze large file performance |
| transfer_duration_ms | Histogram | Identify slow phases |
| retry_attempts_count | Counter | Alert on transient spikes |
| admin_job_failures_count | Counter | Reliability tracking |
| macro_expansions_count | Counter | Unexpected macro growth |

## 8. Alerting & Thresholds (Deferred)
No real-time alerting in v0.01. Future: thresholds on retry_attempts_count, admin_job_failures_count.

## 9. Gaps & Risks
| Gap/Risk | Impact | Planned Action |
|----------|--------|----------------|
| No metrics | Limited performance visibility | Implement metrics v0.02 |
| Always-on tracing | Potential overhead if scale increases | Introduce sampling later |
| No remote export | Prod insight limited to local collector | Evaluate Azure Monitor integration |
| Limited error categorization | May miss nuanced failure patterns | Add error subcodes |
| Lack of macro metrics | Hard to assess macro usage | Add counters v0.02 |

## 10. Future Enhancements
- Integrate metrics & dashboards (Azure Monitor / Grafana).
- Add transfer chunk telemetry (span or event per large file threshold overrun).
- Introduce structured logging schema version (v1 -> v2).

## 11. Traceability
- Functional Spec: spec-functional-fss-manager_v0.01.md
- Architecture Spec: spec-architecture-fss-manager_v0.01.md

End of Document.
