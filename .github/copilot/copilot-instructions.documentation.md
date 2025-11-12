# Copilot Instructions: Documentation

## Scope
Guidelines for specs, plans, API docs, component docs.

## Specifications (Requirements)
- Author with `.github/prompts/spec.innovate.prompt.md` based on `docs/specs/spec-template_v1.1.md`.
- Store under `docs/specs/` grouped by domain/service.
- Filename pattern: `spec-[scope]-[type]_v[version].md` (e.g., `spec-api-functional_v1.0.md`).
- Versioning: never overwrite. Increment version and internal Version field.
- Add: `Supersedes: spec-[...]_v[prev].md` and maintain a Change Log.
- Drafts start at `v0.01`; first release `v1.0`.
- After execution of v1.0 plan, increment toward v2.0 for next changes.

## Plans (Implementation / Execution)
- New features: `.github/prompts/spec.plan.prompt.md` -> execute with `.github/prompts/spec.execute.prompt.md`.
- Refactors: `.github/prompts/refactor.plan.prompt.md` -> execute with `.github/prompts/refactor.execute.prompt.md`.
- Future families: `test`, `pipeline` as needed.
- Store under `docs/plans/{area}` (api, ui, backend, shared, infra, tests).
- Filename pattern: `plan-[area]-[purpose]_v[version].md`.
- Each plan must reference spec versions it is based on: `Based on: spec-api-functional_v1.2.md`.
- Include: Baseline (implemented), Delta (changes), Carry-over (incomplete items).
- Use Work Item / Task / Step structure from plan prompt.

## Validation Checklist
- Correct folder (`docs/specs` or `docs/plans/{area}`).
- Filenames follow patterns; version increments correct.
- Spec Version field matches filename.
- Overview spec references all component specs.
- Plans reference spec versions + Baseline/Delta/Carry-over sections.
- Latest prompt file family/phase used.

## Documentation Maintainability
- Do not duplicate content; reference canonical sources.
- Keep API examples current with implementation.
- Update docs alongside code changes (treat as part of Definition of Done).

## API & Component Docs
- Public API surface documented with purpose & examples.
- Reusable components: parameters, events, usage sample.

## Cross-References
- Architecture overview -> architecture instructions file.
- Frontend specifics -> frontend instructions file.
- Backend specifics -> backend instructions file.
- Testing practices -> testing instructions file.

(High-level index lives in root copilot instructions file.)
