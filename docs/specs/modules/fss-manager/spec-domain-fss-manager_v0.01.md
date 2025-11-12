Title: FSS Manager Domain Specification
Version: v0.01 (Draft)
Status: Draft / Baseline Extraction
Supersedes: None
Change Log:
- Initial draft creation

1. Scope / Purpose
- Establish baseline for the FSS Manager module: domain intent, extensibility hooks, and current implementation status.

2. Context & Overview
- Module will integrate into the Shell via the IModule pattern.
- Provides management UI for FSS related operations (actual pages/endpoints not yet implemented) (Unverified).

3. Components / Modules
- Planned module path: src/Modules/UKHO.ADDS.Management.Modules.FssManager/ (Unverified - not yet created).
- IModule implementation: FssManagerModule (Unverified).
- Navigation model entries: ModulePage instances for FSS Manager UI (Unverified).

4. Detailed Elements
Domain Entities (anticipated):
- FSSBatch, FSSFile, FSSAttribute models (Unverified; will derive from actual code once added).
Relationships: (Unverified)
- FSSBatch contains files & attributes.
Blazor Components (placeholder):
- BatchListPage.razor (Unverified)
- BatchDetailPage.razor (Unverified)
Contracts / DTOs: None implemented (Unverified).

5. Cross-Cutting Concerns
DI Registrations: Will add AddFssManagerModule extension similar to AddSampleModule (Unverified).
Auth: Will rely on existing OIDC & scope addsmanagement:all (Unverified additional role needs).
Logging: Standard OpenTelemetry + host logging (no custom enrichment) (Unverified).
Configuration: Potential module-specific settings (e.g., polling intervals) (Unverified).

6. Non-Functional Characteristics
Performance: No specific constraints yet (Unverified).
Observability: Will inherit telemetry; no custom spans defined (Unverified).
Scalability: UI-only module; server-side circuit scaling same as shell (Unverified).
Reliability: Depends on host error boundary behavior.
Security: Relies on host authorization (granular policies TBD) (Unverified).
Accessibility: Pending review (Unverified).

7. Gaps & Unknowns
- Module project not yet created.
- No pages/components implemented.
- No domain models defined.
- No tests.
- No configuration or authorization policies.

8. Traceability
- (Pending) src/Modules/UKHO.ADDS.Management.Modules.FssManager/ (Unverified).
- Shell integration point: Module registration extension method (to be added) (Unverified).

End of Document.
