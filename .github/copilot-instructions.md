# Copilot Instructions (High-Level)

You are an agent. Continue working until queries are fully resolved.
Be concise but complete. Prefer current research (Microsoft Learn) for Microsoft technologies.

## Quick Principles
- Use feature branches (primary branch: `main`).
- Verify each command succeeds before proceeding; run commands sequentially.
- Prefer latest C#/.NET features; async/await; nullable reference types.

## MCP Tool Selection
- Azure DevOps intent: use Azure DevOps tools.
- GitHub intent: use GitHub tools.
- Microsoft tech (Blazor, ASP.NET Core, Azure, .NET): use Microsoft Learn tools.

## Documentation Workflow (Summary)
- For each new Work Package/piece of work: create a new numbered folder under `./docs/` named `xxx-<descriptor>` (e.g. `001-Initial-Shell`).
- Store ALL related documents (specs, plans, architecture notes, etc.) together inside that Work Package folder.
- Do not overwrite prior work packages; create the next incremental folder (e.g. `002-...`).
- Use appropriate prompt family & phase from `.github/prompts/`.

## Detailed Topic Guides
Refer to specialized instruction files for full detail:
- Architecture: `.github/copilot/copilot-instructions.architecture.md`
- Frontend (Blazor/UI): `.github/copilot/copilot-instructions.frontend.md`
- Backend (APIs/Services): `.github/copilot/copilot-instructions.backend.md`
- Testing: `.github/copilot/copilot-instructions.testing.md`
- Documentation Authoring: `.github/copilot/copilot-instructions.documentation.md`
- Codeing Standards: `.github/copilot/copilot-instructions.coding-standards.md`

All original guidance now resides in one of these files. Do not duplicate; update the relevant file when changing practices.
