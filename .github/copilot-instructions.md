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
- Specifications: versioned under `docs/specs/` (never overwrite; increment versions).
- Plans: versioned under `docs/plans/{area}/` (reference specs; include Baseline/Delta/Carry-over).
- Use appropriate prompt family & phase from `.github/prompts/`.

## Detailed Topic Guides
Refer to specialized instruction files for full detail:
- Architecture: `.github/copilot/copilot-instructions.architecture.md`
- Frontend (Blazor/UI): `.github/copilot/copilot-instructions.frontend.md`
- Backend (APIs/Services): `.github/copilot/copilot-instructions.backend.md`
- Testing: `.github/copilot/copilot-instructions.testing.md`
- Documentation Authoring: `.github/copilot/copilot-instructions.documentation.md`

All original guidance now resides in one of these files. Do not duplicate; update the relevant file when changing practices.