# Copilot Instructions: Documentation

## Scope
Guidelines for authoring and maintaining specifications, plans, API docs, component docs, and archive/versioning workflow.

## Specifications (Requirements)
- Author using appropriate spec prompt (e.g. `.github/prompts/spec.innovate.prompt.md`) based on template `docs/specs/spec-template_v1.1.md`.
- Store specs under `docs/specs/` (cross-cutting) or `docs/specs/modules/<module-name>/` (per-module).
- Filename pattern: `spec-<scope>-<descriptor>_vM.NN.md` (example: `spec-api-functional_v1.0.md`, `spec-domain-fss-manager_v0.01.md`).
- Versioning: never overwrite existing spec; create new file with incremented version suffix and update internal `Version:` field.
- Drafts start at `v0.01`; first stable baseline release `v1.0`. Post-implementation increments toward `v2.0`.
- Include `Supersedes: <previous-file>` and a Change Log section noting prior version(s).
- Exclude detailed ADDS Mock internals (treated out-of-scope) – reference only at high level.

## Spec Versioning & Archiving
- Keep only the latest version of each spec in its active folder (`docs/specs/` or `docs/specs/modules/<module>/`).
- Move superseded versions to `docs/specs/archive/` preserving original filenames.
- Archive folder contains immutable historical snapshots; never edit archived files.
- Ensure `docs/specs/archive/ARCHIVE_README.md` documents policy (maintain if updated).
- When generating a new spec version:
  1. Scan active folders for matching `spec-*_v*.md` excluding `archive/`.
  2. Parse version suffix `_vM.NN` to determine highest existing version.
  3. Increment minor (e.g. v0.02 -> v0.03) or follow release cadence (v0.09 -> v0.10, v0.99 -> v1.0).
  4. Move older versions to archive (if not already moved).
  5. Update cross-references in new spec to point only to latest versions.
- Preserve `(Unverified)` markers where evidence not yet collected.

## Modules
- Module specs live under `docs/specs/modules/<module-name>/`.
- Required initial file: `spec-domain-<module-name>_v0.01.md` capturing purpose, scope, gaps.
- Optional per-module specs: `spec-api-<module-name>_v0.01.md`, `spec-frontend-<module-name>_v0.01.md` if the module exposes APIs or UI components.
- Each module spec independently versioned & archived using same rules.

## Plans (Implementation / Execution)
- New feature plans: `.github/prompts/spec.plan.prompt.md` executed via `.github/prompts/spec.execute.prompt.md`.
- Refactor plans: `.github/prompts/refactor.plan.prompt.md` executed via `.github/prompts/refactor.execute.prompt.md`.
- Store plans under `docs/plans/<area>/` (areas: api, ui, backend, shared, infra, tests).
- Filename pattern: `plan-<area>-<purpose>_vM.NN.md`.
- Each plan references source spec versions: `Based on: spec-api-functional_v1.2.md`.
- Include Baseline (current implemented), Delta (planned changes), Carry-over (incomplete / deferred items).
- Use Work Item / Task / Step hierarchy from plan prompt.
- Archive policy may be extended to plans (currently enforced for specs only).

## Workflow (Authoring Sequence)
1. Inspect codebase / gather evidence.
2. Create or increment spec version (apply archiving rules).
3. Generate or update plan referencing latest specs.
4. Implement code changes; update docs in same feature branch.
5. Merge with branch checks ensuring spec & plan consistency.

## Validation Checklist
- Correct folder placement (`docs/specs`, `docs/specs/modules/<module>`, or `docs/plans/<area>`).
- Filename matches pattern and `Version:` field matches suffix.
- `Supersedes:` line present (except initial v0.01).
- Change Log includes new entry referencing previous version.
- Overview spec references only current component/module spec versions.
- Plans reference latest spec versions and contain Baseline/Delta/Carry-over.
- Archive folder contains only superseded versions; no duplicates in active folders.

## Documentation Maintainability
- Avoid duplication; reference canonical spec rather than copying text.
- Keep API examples synchronized with implementation.
- Treat documentation updates as part of Definition of Done for each change set.

## API & Component Docs
- Document public API surface (routes, verbs, purpose, auth) with examples if stable.
- Document reusable UI components: parameters, events, example usage snippet.

## Cross-References
- Architecture overview spec -> architecture instruction file.
- Frontend spec -> frontend instruction file.
- Backend spec -> backend instruction file.
- Testing references -> testing instruction file.
- Module specs -> system overview & architecture specs.

## Gaps Handling
- Clearly list gaps (missing tests, auth policies, accessibility, performance metrics).
- Retain gaps until evidence added; mark unresolved items as `(Unverified)`.

## Exclusions
- Do not regenerate or version detailed ADDS Mock internals beyond high-level mention.

## File Naming Summary
- Spec: `spec-<scope>-<descriptor>_vM.NN.md`
- Module spec: `spec-<domain|api|frontend>-<module>_vM.NN.md`
- Plan: `plan-<area>-<purpose>_vM.NN.md`

## Archiving Safeguards
- Ensure archive folder exists; create if missing before moves.
- Do not treat files in `archive/` as candidates for version increment.
- Never delete archived files.

End of File.
