# Specs Archive

This folder contains superseded specification versions. Archiving rules:

## Policy
- Current (latest) version for each spec remains in `docs/specs/` root.
- Previous versions (e.g., _v0.01_) are moved here preserving original filename.
- Do not modify archived files; create a new version instead (e.g., increment minor: v0.02 -> v0.03).
- Domain areas explicitly out-of-scope (e.g., detailed ADDS Mock internals) should not receive new spec versions.

## Automation Guidance (for prompts / agents)
1. When generating a new spec version `spec-<name>_vX.YY.md`:
   - Detect existing versions with lower semantic version.
   - Move those files to `docs/specs/archive/`.
2. Never overwrite a file; always create new version.
3. Update cross-references in new spec to point to latest versions only.
4. Preserve change log entries.

## Example
- Generate `spec-system-overview_v0.03.md`.
- Move `spec-system-overview_v0.02.md` to archive.

## Out-of-Scope
- Detailed mock domain specs (removed per scope change) remain archived if present but no further versions.

