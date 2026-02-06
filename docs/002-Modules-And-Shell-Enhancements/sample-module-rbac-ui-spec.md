# Sample Module RBAC Page + Button Specification

- **Work Package**: `002-Modules-And-Shell-Enhancements`
- **Output folder**: `docs/002-Modules-And-Shell-Enhancements/`

## Purpose
Demonstrate role-based access control in the shell and module navigation.

## New page
- Add a new Sample Module page with route: `sample/secure`.

## Page access control
- Required role(s) for the page: `showsamplepage`.
- Navigation behavior:
  - The nav item for this page is shown only to users in role `showsamplepage`.
  - Users without the role should not see the nav item.
- Direct URL behavior:
  - Users without the role attempting to access the URL should be redirected to **Access Denied**.

## Role-gated button
- Within the `sample/secure` page:
  - Add a button that is only rendered when user is in role `showsamplebutton`.

## Related navigation model rules
- `ModulePage` will be extended to support multiple required roles (any-of semantics).
- `ModulePageService` will filter `Pages` for the current user (auth-aware).
