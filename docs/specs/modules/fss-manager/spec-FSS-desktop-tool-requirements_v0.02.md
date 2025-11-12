# FileShareService.DesktopClient Functional Requirements Baseline (Tabular Format)

Version: v0.02 (Draft)
Status: Draft / Baseline Extraction (Formatting Update)
Supersedes: spec-baseline-requirements_v0.01.md
Change Log: Converted requirement paragraphs to tables; no semantic changes.
Scope / Purpose: Same as v0.01. Formatting only.

## 1. Executive Summary (Baseline)
The FileShareService.DesktopClient is a .NET 6 Windows desktop (Prism MVVM) application that enables authenticated users (via Azure AD) to search for, download, and administer file batches stored in an external File Share Service (FSS) through admin/client APIs.

## 2. System Overview (Baseline)
- Azure AD interactive & silent authentication
- Environment selection (environments.json)
- Query-based batch search (system + user attributes, pagination)
- Batch detail viewing & file download
- Permission probe for expiry operations
- Administrative batch jobs (create batch, append/replace ACL, set expiry, etc.)
- Macro expansion (date/week tokens)
- Transient error retry logic (Polly)

## 3. Architecture Overview (Baseline)
Component style: Prism MVVM (Regions)
Communication: HTTP via FileShareAdminClient / FileShareClient
Authentication: MSAL PublicClientApplication (interactive + silent)
Authorization: Roles surfaced (no granular enforcement)
Hosting: Windows desktop client

## 4. Functional Requirements (Baseline Extraction)
(All priorities baseline-unassigned: use "Unspecified".)

### 4.1 Authentication
| ID | Requirement Description | Priority |
|----|-------------------------|----------|
| FR1 | Allow interactive login for selected environment when no cached token. | Unspecified |
| FR2 | Attempt silent token acquisition using cached credentials before interactive prompt. | Unspecified |
| FR3 | Parse role claims from JWT and expose to UI. | Unspecified |
| FR4 | Invalidate login state when environment changes; require re-authentication. | Unspecified |
| FR5 | Navigate to Search module on successful login. | Unspecified |
| FR6 | Append timestamped login output including token expiry to log string. | Unspecified |

### 4.2 Environment Management
| ID | Requirement Description | Priority |
|----|-------------------------|----------|
| FR7 | Load and validate environments from environments.json (Name, TenantId, ClientId, BaseUrl). | Unspecified |
| FR8 | Allow user selection of current environment; notify dependents. | Unspecified |
| FR9 | Default current environment to first valid entry at startup. | Unspecified |

### 4.3 Search Criteria Construction
| ID | Requirement Description | Priority |
|----|-------------------------|----------|
| FR10 | Add new search criterion row interactively. | Unspecified |
| FR11 | Remove existing search criterion row. | Unspecified |
| FR12 | Retrieve user batch attributes asynchronously and merge with fixed system attributes. | Unspecified |
| FR13 | Reset operator when selected field changes and operator invalid for new type. | Unspecified |
| FR14 | Disable value entry for Exists/NotExists operators and clear value. | Unspecified |
| FR15 | Hide AND/OR control for first criterion; show for subsequent ones. | Unspecified |
| FR16 | Build FSS query string using operator & attribute type mappings. | Unspecified |
| FR17 | (N/A) Macros not applied within search criteria values (baseline). | Unspecified |

### 4.4 Search Execution & Pagination
| ID | Requirement Description | Priority |
|----|-------------------------|----------|
| FR18 | Execute search clearing previous results and issuing new request. | Unspecified |
| FR19 | Restrict search command to authenticated state and not in progress. | Unspecified |
| FR20 | Page forward when additional results exist beyond current offset. | Unspecified |
| FR21 | Page backward when not on first page. | Unspecified |
| FR22 | Use fixed page size of 25 results. | Unspecified |
| FR23 | Display summary of current result range or "No batches found." | Unspecified |
| FR24 | Show detailed API errors for HTTP 400; generic message otherwise; log all failures. | Unspecified |
| FR25 | Probe expiry permission via SetExpiryDate(Guid.Empty) call; derive capability flag. | Unspecified |
| FR26 | Project each batch entry into BatchDetailsViewModel with attributes/files/dates/flags. | Unspecified |
| FR27 | Provide JSON serialization view of search result payload. | Unspecified |
| FR28 | Re-run search automatically on BatchExpiredEvent. | Unspecified |

### 4.5 Batch Details / File Download
| ID | Requirement Description | Priority |
|----|-------------------------|----------|
| FR29 | Initiate file download from batch detail. | Unspecified |
| FR30 | Prompt user for destination using SaveFileDialog abstraction. | Unspecified |
| FR31 | Confirm overwrite if file already exists. | Unspecified |
| FR32 | Stream file content to FileStream with size & cancellation token. | Unspecified |
| FR33 | Support cancellation of in-progress file download. | Unspecified |
| FR34 | Display success message (filename & batch ID) on download completion. | Unspecified |
| FR35 | Confirm and execute batch expiry setting current timestamp. | Unspecified |
| FR36 | Show detailed error descriptions or fallback message on expiry failure. | Unspecified |
| FR37 | Publish BatchExpiredEvent after successful expiry. | Unspecified |
| FR38 | Indicate ability to set batch expiry (permission flag). | Unspecified |

### 4.6 Job Execution Framework (General)
| ID | Requirement Description | Priority |
|----|-------------------------|----------|
| FR39 | Validate configuration before execution; surface errors. | Unspecified |
| FR40 | Aggregate validation/execution errors into ExecutionResult/ErrorMessages. | Unspecified |
| FR41 | Prompt user to confirm cancellation of executing job. | Unspecified |
| FR42 | On cancellation during upload, attempt rollback; fallback to expiry when conflict. | Unspecified |
| FR43 | Maintain state flags (IsExecuting, IsCommitting, IsExecutingComplete, IsCanceled). | Unspecified |

### 4.7 New Batch Job
| ID | Requirement Description | Priority |
|----|-------------------------|----------|
| FR44 | Search for duplicate batches (unless ignored) by business unit & attributes. | Unspecified |
| FR45 | Prompt to proceed or cancel when duplicates found. | Unspecified |
| FR46 | Confirm ignoring duplicate check when option selected. | Unspecified |
| FR47 | Create batch and obtain batch handle (ID) before uploads. | Unspecified |
| FR48 | Enumerate files (macro-expanded paths), validate expected counts/wildcards. | Unspecified |
| FR49 | Track per-file upload progress blocks & percentage. | Unspecified |
| FR50 | Include per-file attributes (macro-expanded) during upload. | Unspecified |
| FR51 | Cancel remaining uploads on failure or user cancellation. | Unspecified |
| FR52 | Commit batch after successful uploads. | Unspecified |
| FR53 | Poll batch status (<= 60 min, 10s interval) for committed state. | Unspecified |
| FR54 | Rollback batch on commit failure or upload exception. | Unspecified |
| FR55 | Set batch expiry on cancellation during commit phase. | Unspecified |
| FR56 | Validate expiry date format/future constraint (RFC3339 or macro-expanded). | Unspecified |
| FR57 | Apply ACL (read users/groups) on creation. | Unspecified |
| FR58 | Provide detailed directory & file listing context on count mismatch. | Unspecified |

### 4.8 ACL Append Job
| ID | Requirement Description | Priority |
|----|-------------------------|----------|
| FR59 | Append ACL entries via AppendAclAsync. | Unspecified |
| FR60 | On forbidden, append permission hint; else list errors or status fallback. | Unspecified |
| FR61 | Log start, completion, and errors (display name, batch ID). | Unspecified |

### 4.9 ACL Replace Job
| ID | Requirement Description | Priority |
|----|-------------------------|----------|
| FR62 | Replace ACL entirely via ReplaceAclAsync. | Unspecified |
| FR63 | Same error/permission handling pattern as append job. | Unspecified |
| FR64 | Log start, completion, and errors (display name, batch ID). | Unspecified |

### 4.10 Set Expiry Date Job
| ID | Requirement Description | Priority |
|----|-------------------------|----------|
| FR65 | Set batch expiry via SetExpiryDateAsync. | Unspecified |
| FR66 | Validate expiry date presence & format (RFC3339 or macro-expanded). | Unspecified |
| FR67 | Append permission hint on forbidden. | Unspecified |
| FR68 | Log start, completion, and errors. | Unspecified |

### 4.11 Macro Expansion
| ID | Requirement Description | Priority |
|----|-------------------------|----------|
| FR69 | Substitute macros for current/offset date, day, month, year, week number. | Unspecified |
| FR70 | Derive UKHO week number & year for current/offset periods. | Unspecified |
| FR71 | Iteratively replace all macro tokens until none remain. | Unspecified |
| FR72 | Return original value unchanged if null/empty. | Unspecified |

### 4.12 Date Time Validation
| ID | Requirement Description | Priority |
|----|-------------------------|----------|
| FR73 | Detect empty/whitespace expiry input; record missing error. | Unspecified |
| FR74 | Parse RFC3339 formats directly when valid. | Unspecified |
| FR75 | Attempt macro expansion then parse when direct parse fails. | Unspecified |
| FR76 | Record invalid format error if parsing fails after expansion. | Unspecified |

### 4.13 Retry / Resilience
| ID | Requirement Description | Priority |
|----|-------------------------|----------|
| FR77 | Retry transient HTTP statuses with exponential backoff. | Unspecified |
| FR78 | Add Retry-After delay for 429 responses. | Unspecified |
| FR79 | Log each retry (URI, delay, attempt, correlation ID, status code). | Unspecified |

### 4.14 Navigation
| ID | Requirement Description | Priority |
|----|-------------------------|----------|
| FR80 | Navigate views via Prism RegionManager & RegionNames.MainRegion. | Unspecified |
| FR81 | Navigate to Authenticate view when IsLoggedIn becomes false. | Unspecified |
| FR82 | Navigate to Search view on successful login (trace duplication of FR5). | Unspecified |

### 4.15 Version Information
| ID | Requirement Description | Priority |
|----|-------------------------|----------|
| FR83 | Expose application file version via reflection. | Unspecified |

### 4.16 Token Roles Parsing
| ID | Requirement Description | Priority |
|----|-------------------------|----------|
| FR84 | Extract all JWT claims of type "roles" for downstream use. | Unspecified |

### 4.17 Event Handling & Messaging
| ID | Requirement Description | Priority |
|----|-------------------------|----------|
| FR85 | Publish BatchExpiredEvent after successful expiry updates. | Unspecified |
| FR86 | Subscribe to BatchExpiredEvent and re-execute search on UI thread. | Unspecified |

### 4.18 Cancellation & Concurrency
| ID | Requirement Description | Priority |
|----|-------------------------|----------|
| FR87 | Propagate cancellation to halt remaining parallel file uploads. | Unspecified |
| FR88 | Support cancellation of file download via token usage. | Unspecified |

### 4.19 Security & Permissions (Observations)
| ID | Requirement Description | Priority |
|----|-------------------------|----------|
| FR89 | Persist token cache for silent renewals (TokenCacheHelper). | Unspecified |
| FR90 | Expose roles without granular authorization enforcement (not implemented). | Unspecified |

## 5. Non-Functional Requirements (Observed / Baseline Indicators)
| ID | Category | Description | Target |
|----|----------|-------------|--------|
| NFR1 | Performance | Page size limits payload (no latency target coded). | Unspecified |
| NFR2 | Resilience | Exponential backoff retry for transient statuses. | Unspecified |
| NFR3 | Logging | Structured logging around jobs, uploads, auth, retries, errors. | Unspecified |
| NFR4 | Maintainability | MVVM modular design; service abstractions. | Unspecified |
| NFR5 | Extensibility | Attribute provider & query builder support future additions. | Unspecified |
| NFR6 | Observability | Logging only; no telemetry (gap). | Gap |
| NFR7 | Security | Azure AD auth; cached tokens; role enumeration. | Unspecified |
| NFR8 | Reliability | Rollback & expiry fallback strategies for failure/cancellation. | Unspecified |

## 6. Gaps & Unknowns
| ID | Topic | Description | Status |
|----|-------|------------|--------|
| G1 | Accessibility | Keyboard navigation / screen reader support not identified. | Unverified |
| G2 | ErrorDeserializingJobs | Detailed behavior not documented. | Gap |
| G3 | Permission Service | Centralized authorization absent. | Gap |
| G4 | Retry Config Externalization | Retry parameters not externalized/configurable. | Gap |
| G5 | Telemetry | No metrics / Application Insights instrumentation. | Gap |
| G6 | Auth Testing | Interactive auth untested due to external dependency. | Accepted Limitation |
| G7 | Role-Based Enforcement | Granular role checks not implemented. | Gap |

## 7. Traceability
(See v0.01 for source path list; unchanged.) Key files:
- Auth: Core/AuthProvider.cs
- JWT Roles: Core/JwtTokenParser.cs
- Environments: Core/EnvironmentLoader.cs
- Search Criteria: Modules/Search/SearchCriteriaViewModel.cs
- Search Exec: Modules/Search/SearchViewModel.cs
- Batch Details & Download: Modules/Search/BatchDetailsViewModel.cs
- New Batch: Modules/Admin/JobViewModels/NewBatchJobViewModel.cs
- Append ACL: Modules/Admin/JobViewModels/AppendAclJobViewModel.cs
- Replace ACL: Modules/Admin/JobViewModels/ReplaceAclJobViewModel.cs
- Set Expiry: Modules/Admin/JobViewModels/SetExpiryDateJobViewModel.cs
- Macro: Helper/MacroTransformer.cs
- DateTime Validation: Helper/DateTimeValidator.cs
- Query Builder: Core/FssSearchStringBuilder.cs
- Retry: TransientErrorsHelper.cs
- Navigation: NavigationManager.cs
- Version: VersionProvider.cs

## 8. Future Indicators
- Macro system extensible for more tokens.
- Duplicate FR5/FR82 indicates potential consolidation opportunity.

End of baseline functional requirements specification (tabular version).
