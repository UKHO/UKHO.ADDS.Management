# Prompt: SPEC Archive Rules Enforcement

## Objective
Ensure newly generated specification versions automatically archive prior versions while keeping only the latest in `docs/specs/`.

## Archive Policy
- Latest version per spec stays in `docs/specs/`.
- Prior versions moved to `docs/specs/archive/` (retain original filenames).
- Never delete archived specs; do not edit their content.
- Excluded domains: ADDS Mock detailed internals (do not regenerate or update).

## Steps for Agent
1. Discover existing spec files matching pattern `spec-*_v*.md` in `docs/specs/`.
2. Parse semantic version suffix `_vX.YY`.
3. For the spec being updated:
   - Identify highest version number; increment minor for new draft.
   - Move all older versions to archive folder if not already archived.
4. Create new spec file with updated version, Change Log referencing superseded version.
5. Update cross-reference sections to only list current versions.

## Safeguards
- If no previous version exists, create v0.01.
- If previous version exists but archive folder missing, create folder before moving.
- Do not process files in `archive/` as candidates for new versions.

## Gaps & Unknowns Handling
- Preserve (Unverified) markers from prior versions if evidence not added.
- Do not invent test coverage or infrastructure not scanned.

## Completion Checklist
- Archive folder present with previous versions.
- New spec file created with incremented version.
- Change Log updated.
- Cross-references updated to latest.

