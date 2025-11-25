# ADDS Management — API Functional Specification

**Title:** ADDS Management API Functional Specification

**Version:** v0.02 (Draft)

**Status:** Draft / Baseline Extraction (Adjusted Scope)

**Supersedes:** `spec-api-functional_v0.01.md`

**Change Log:**
- v0.02: Removed detailed mock endpoint catalog; retained only in-scope host endpoints.

---

## 1. Executive Summary

Purpose
- Enumerate the management host API surface and in-scope infra endpoints for the ADDS Management solution (mock endpoints excluded from this version).

Objective
- Provide clear reference for authentication and health endpoints used by the Blazor host and local orchestration.

---

## 2. Scope / Purpose

In scope
- Host authentication endpoints
- Host health endpoints (development)

Out of scope
- Mock service endpoints and their internal contracts

---

## 3. System Overview

Context
- Host exposes authentication and health endpoints.
- OpenAPI UI available in Development to document host endpoints.

Core capabilities
- Provide OIDC integration for user authentication
- Surface health checks for local development and orchestration

---

## 4. Public API Surface (In-scope)

Authentication
- `GET /authentication/login` ? Initiates OIDC challenge (redirectUri "/")
- `GET /authentication/logout` ? Signs out cookie + OIDC (redirectUri "/")
- `POST /authentication/logout` ? Same sign-out sequence

Health
- `GET /health` ? Aggregated readiness checks (Development only)
- `GET /alive` ? Liveness check (tagged "live", Development only)

Supporting structures
- `AuthorizationHandler` ? Injects bearer token into outgoing `HttpClient` requests for authenticated users

---

## 5. Detailed Elements

Authentication flow
- Uses OIDC authorization code flow with Keycloak
- Scope: `addsmanagement:all` referenced (role/claim strategy not documented)

Health checks
- Implemented via `ServiceDefaults` health registration
- Exposed only in Development environment

---

## 6. Cross-Cutting API Concerns

- Authentication: Keycloak OIDC + cookie authentication
- Authorization: No explicit policies registered (gap)
- Versioning: No API versioning implemented (gap)
- Validation: Not applicable for infra endpoints
- Error handling: Default ASP.NET Core pipeline
- Documentation: OpenAPI available in Development (no custom descriptions for auth routes)

---

## 7. Non-Functional Characteristics

- Security: OIDC challenge and sign-out flows used
- Reliability: Health endpoints for readiness and liveness in Development
- Observability: Telemetry via `ServiceDefaults`

---

## 8. Gaps & Unknowns

- No API versioning strategy
- Missing authorization policy definitions and role mapping
- No standardized error response model
- No rate-limiting strategy

---

## 9. Traceability

- Auth endpoints: `src/Shell/UKHO.ADDS.Management.Host/Extensions/LoginLogoutEndpointRouteBuilderExtensions.cs`
- Auth configuration: `src/Shell/UKHO.ADDS.Management.Host/Program.cs`
- AuthorizationHandler: `src/Shell/UKHO.ADDS.Management.Host/Extensions/AuthorizationHandler.cs`
- Health mapping: `src/Shell/UKHO.ADDS.Management.ServiceDefaults/Extensions.cs`

---

## 10. Cross-References

- `spec-system-overview_v0.02.md`
- `spec-architecture-components_v0.02.md`

---

## 11. Completion Checklist

- Host endpoints enumerated
- Mock endpoints excluded
- Gaps identified

---

_End of Document._
