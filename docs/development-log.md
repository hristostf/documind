# Development Log

## 2026-07-30

Implemented:

- Organization creation
- Organization retrieval
- Workspace creation
- Workspace retrieval
- Application dependency injection
- Infrastructure dependency injection
- PostgreSQL through Docker
- HTTP request file for manual API testing

Important decisions:

- Handlers are instance classes because they receive dependencies
- DbContext, repositories and handlers use scoped lifetime
- API endpoints are grouped by feature
- Workspace retrieval verifies both WorkspaceId and OrganizationId
- Application returns null for simple not-found cases
- API converts null into HTTP 404