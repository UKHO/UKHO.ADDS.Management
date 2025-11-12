# UKHO.ADDS.Management

A .NET 9 / Blazor (Aspire orchestrated) solution for ADDS Management.

## 1. Prerequisites

- .NET 9 SDK (confirm with `dotnet --version`).
- Visual Studio 2022 (latest preview) or VS Code + C# Dev Kit.
- Docker Desktop running (containers required for Aspire + Keycloak).
- Git.

## 2. Clone (includes submodules)

This repository contains Git submodules. Ensure they are initialised.

```bash
git clone https://github.com/UKHO/UKHO.ADDS.Management.git
cd UKHO.ADDS.Management
# Initialise and fetch all submodules
git submodule update --init --recursive
```

If you have already cloned without submodules, just run:
```bash
git submodule update --init --recursive
```
When pulling future changes that may affect submodules, you can use:
```bash
git pull --recurse-submodules
```

### 2.a Repoint submodules to latest upstream commits
To move each submodule to the latest commit on its configured branch:
```bash
git submodule update --remote --merge
```
Notes:
- `--remote` fetches latest commits from the submodule's remote.
- `--merge` merges the fetched commit into the currently checked out submodule branch (fast-forward when possible).
- Commit the resulting submodule SHA changes in the parent repo:
```bash
git add <submodule-paths>
git commit -m "chore: update submodules to latest remote"
```
(Or simply `git add .` if no other unintended changes.)

## 3. First-time Setup

1. Ensure Docker Desktop is started before opening the solution.
2. Open the solution in Visual Studio.
3. Set the startup project to `UKHO.ADDS.Management.AppHost` (Aspire AppHost).  
   - In Solution Explorer: right-click `UKHO.ADDS.Management.AppHost` → Set as Startup Project.
4. Build solution once (`Ctrl+Shift+B`) to restore all projects.
5. Press `F5` (or Run) – this launches the Aspire AppHost which will:
   - Start required containers and dependent services.
   - Launch the Aspire Dashboard.

## 4. Aspire Dashboard

After F5 a browser should open (or link appears in output). Typical URLs:

- Dashboard: http://localhost:18888 (default Aspire dashboard port – may differ if configured).
- Keycloak: http://localhost:8080 (from dashboard service list).
- Management Host (Blazor UI/API): use HTTPS link exposed in dashboard (e.g. https://localhost:56357).

If the dashboard does not open automatically, check Debug Output or run the AppHost again.

## 5. Adding a User in Keycloak (ADDSManagement Realm)

1. Navigate to Keycloak admin: http://localhost:8080/.
2. Log in with admin credentials (default is `admin / kcpassword`  set in UKHO.ADDS.Management.Host appsettings.json).
3. In the left sidebar select the realm `ADDSManagement` (create it if absent; name must match application configuration).
4. Go to Users → Add user:
   - Username: meaningful identifier.
   - Email / First / Last Name (optional).
   - Enable user.
5. Save → Credentials tab → Set password (disable temporary if you do not want forced reset).
6. (Optional) Assign Roles: Realm Roles → select any required role (e.g. `adds-user`, `adds-admin` if defined).
7. (Optional) Client Role Mapping: pick client corresponding to this application and assign roles.
8. Log out admin and test login via the application’s login flow.

## 6. Starting UKHO.ADDS.Management.Host

The Host service is orchestrated by Aspire. From the dashboard:

1. Locate `UKHO.ADDS.Management.Host`.
2. Use the HTTPS endpoint (click to open in browser).  
3. Authenticate (redirect to Keycloak). Use the user you created.

## 7. Project Structure (High-Level)

- AppHost: Orchestration & service wiring (startup target).
- Host: Blazor shell hosting dynamic modules.
- Modules.*: Feature/module implementations (e.g. Samples).
- ServiceDefaults / Shell projects: Cross-cutting concerns & composition.
- Mocks: Local mock implementations (optional for testing).

## 8. Changing the Host HTTPS Port

If you need to change the `UKHO.ADDS.Management.Host` HTTPS port:

1. Edit `src/Shell/UKHO.ADDS.Management.Host/Properties/launchSettings.json` – modify the `applicationUrl` ports (both https and http if desired).
2. Edit Keycloak realm import file `src/Shell/UKHO.ADDS.Management.AppHost/Realms/adds-management-realm.json`:
   - Search for existing port (e.g. `56357`) and replace in all redirectUris, webOrigins and post logout URIs with the new HTTPS port.
3. Stop the running AppHost / dashboard.
4. Remove the Keycloak persistent volume via Docker Desktop (Volumes → locate Keycloak volume → Delete) to force re-import with updated realm settings.
5. Restart (F5) the AppHost – Keycloak will re-import the realm with the new port mappings.
6. Re-create test users if the volume deletion cleared them.

## 9. Common Troubleshooting

- Containers not starting: confirm Docker is running; re-run AppHost.
- Keycloak unreachable: verify port 8080 not in use; restart containers.
- Login loops: check realm name matches configuration; clear cookies.
- HTTPS certificate warnings: trust the dev certificate (`dotnet dev-certs https --trust`).
- Dashboard missing: ensure AppHost is actually running (Debug vs Release); check Output window.
- Changed port not reflected: ensure realm JSON updated and Keycloak volume removed before restart.

## 10. Useful Commands

```bash
# Trust HTTPS dev cert (if not already)
dotnet dev-certs https --trust

# Clean + Restore
dotnet clean && dotnet restore

# List running containers
docker ps

# Update submodules after pulling
 git submodule update --recursive --remote
```

## 11. Updating / Pulling Changes

```bash
git pull origin main
# Then ensure submodules are current
git submodule update --init --recursive
```

If new dependencies added, rebuild AppHost and re-run.

## 12. Security / Credentials

Do not commit real secrets. Use environment variables / user secrets. Rotate default Keycloak admin password on first setup.

## 13. Next Steps

- Implement real roles & claims mapping in Keycloak.
- Add automated tests (unit/integration) under test projects when introduced.
- Extend modules under `src/Modules/`.

## 14. Useful URLs (Recap)

- Aspire Dashboard: http://localhost:18888
- Keycloak Admin: http://localhost:8080
- Host (example): https://localhost:7xxx (actual port from dashboard)

---

Short Setup: Docker → Set AppHost startup → F5 → Create Keycloak user in ADDSManagement realm → Open Host HTTPS endpoint.
