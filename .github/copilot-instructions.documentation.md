# Copilot Documentation Instructions (Extended Archive Behavior)

## Spec Versioning & Archiving
- Always create new spec versions; never overwrite existing files.
- Keep only the latest version of each `spec-*.md` in `docs/specs/`.
- Move superseded versions to `docs/specs/archive/` preserving filenames.
- Maintain an `ARCHIVE_README.md` describing policy (already present).

## Workflow
1. Scan `docs/specs/` for files matching `spec-*_v*.md`.
2. Determine current latest version per spec name (prefix before `_v`).
3. When generating an update:
   - Increment minor (e.g., v0.02 -> v0.03).
   - Move older versions to archive if not already archived.
   - Create new file with Change Log referencing prior version.
4. Update cross-references to only point to latest versions.

## Exclusions
- Do not regenerate detailed ADDS Mock specifications; treat mocks as out-of-scope.

## File Naming
- Format: `spec-<scope>-<descriptor>_vX.YY.md` (example: `spec-system-overview_v0.03.md`).

## Validation
- Ensure archive folder exists before moving.
- Never modify archived files.
- Preserve (Unverified) markers for unevidenced areas.

## Plans
- Plans follow similar versioning but reside in `docs/plans/<area>/` (archive policy may mirror specs if adopted; currently only specs enforced).

## Gaps Handling
- Explicitly list gaps; do not remove them unless evidence added.

