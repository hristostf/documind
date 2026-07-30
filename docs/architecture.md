# Architecture

## Projects

- DocuMind.Api
  - HTTP endpoints
  - Request and response mapping
  - Converts application results into HTTP responses

- DocuMind.Application
  - Commands and queries
  - Handlers
  - Repository interfaces
  - Business use-case orchestration

- DocuMind.Domain
  - Entities
  - Domain rules
  - No dependency on EF Core or ASP.NET Core

- DocuMind.Infrastructure
  - EF Core
  - PostgreSQL
  - Repository implementations

## Dependency direction

Api -> Application -> Domain
Infrastructure -> Application and Domain

## Current route convention

Workspace routes are organization-scoped:

POST /api/organizations/{organizationId}/workspaces
GET  /api/organizations/{organizationId}/workspaces/{workspaceId}