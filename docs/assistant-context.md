# DocuMind — Assistant Context

> This file exists so a new AI assistant or developer can quickly understand the DocuMind project, its long-term vision, current architecture, completed work, development principles, and preferred learning approach.

---

# 1. Project Vision

DocuMind is a production-quality, multi-tenant AI SaaS for intelligent document management, semantic search, and document-based AI conversations.

The goal is not simply to build a CRUD application.

The project is intentionally designed as a serious learning platform for understanding how modern SaaS systems are structured, built, secured, tested, deployed, and maintained.

The final application should resemble a real product that could eventually be used by companies and teams.

The project should demonstrate:

- Clean Architecture
- Vertical Slice Architecture
- Domain-driven design principles where appropriate
- Multi-tenancy
- Authentication and authorization
- Document storage and processing
- Background jobs
- Vector databases
- Embeddings
- Semantic search
- Retrieval-Augmented Generation
- AI chat
- Production-ready frontend architecture
- Logging, monitoring, testing, and deployment

---

# 2. Product Goal

A company should eventually be able to:

- Create an organization
- Invite users to the organization
- Assign roles and permissions
- Create workspaces
- Upload documents
- Organize documents inside workspaces
- Extract text from uploaded documents
- Split document content into chunks
- Generate vector embeddings
- Store embeddings in PostgreSQL with pgvector
- Search documents semantically
- Ask questions about documents
- Receive answers grounded in document content
- View citations and source references
- Manage document versions
- Control access to documents and workspaces
- Monitor processing status
- Scale across many organizations

DocuMind may be similar in spirit to products such as:

- Notion AI
- Microsoft Copilot
- Glean
- ChatPDF
- Confluence AI
- Dropbox AI

DocuMind is not intended to be a direct clone of any of these products.

---

# 3. Main Learning Goal

The primary goal is to understand how experienced engineers design and build production systems.

The project should not be rushed.

When introducing new functionality, the process should normally be:

1. Explain the problem.
2. Explain why the feature is needed.
3. Discuss possible designs.
4. Compare trade-offs.
5. Choose an approach.
6. Implement it incrementally.
7. Review the final result.
8. Record important architectural decisions.

The assistant should not generate large amounts of code without explanation unless explicitly asked to provide the complete implementation.

---

# 4. Technology Stack

## Backend

- .NET
- ASP.NET Core Minimal APIs
- C#
- Entity Framework Core
- PostgreSQL
- pgvector
- Docker

## Frontend

- Next.js
- Next.js App Router
- React
- TypeScript

## Possible Frontend Supporting Libraries

These should only be introduced when needed:

- React Hook Form
- Zod
- TanStack Query
- Tailwind CSS or CSS Modules
- Component library selected later
- Streaming UI support for AI chat

## AI and Document Processing

Planned technologies may include:

- OpenAI API or Azure OpenAI
- Embedding models
- Large language models
- Retrieval-Augmented Generation
- PDF text extraction
- Document chunking
- Background job processing
- Semantic search

## Infrastructure

Planned infrastructure may include:

- Docker
- PostgreSQL
- pgvector
- Blob or object storage
- Background job workers
- Structured logging
- Monitoring
- CI/CD
- Cloud deployment

---

# 5. High-Level System Architecture

```text
                         User
                           │
                           ▼
                  Next.js Frontend
             React + TypeScript + App Router
                           │
                           │ HTTPS
                           ▼
               ASP.NET Core Minimal API
                           │
                           ▼
                 Application Layer
                           │
                           ▼
                    Domain Layer
                           ▲
                           │
                Infrastructure Layer
             ┌─────────────┼─────────────┐
             │             │             │
             ▼             ▼             ▼
       PostgreSQL      Object Storage   External AI
        + pgvector      / Blob Store     Provider
             │
             ▼
      Background Processing
             │
             ▼
        Text Extraction
             │
             ▼
           Chunking
             │
             ▼
     Embedding Generation
             │
             ▼
       Vector Persistence
             │
             ▼
       Semantic Retrieval
             │
             ▼
          RAG Pipeline
```

---

# 6. Frontend Architecture

The frontend should use Next.js with the App Router.

Next.js is responsible for:

- Rendering the web application
- Application routing
- Nested layouts
- Loading states
- Error boundaries
- Authentication UI
- Dashboard UI
- Workspace management UI
- Document upload UI
- Document explorer
- Search interface
- AI chat interface
- Streaming AI responses
- Client-side interaction
- Server-side rendering where useful

The Next.js application should not own the core business logic.

The ASP.NET Core API remains the authority for:

- Business rules
- Authorization
- Organization boundaries
- Workspace access
- Document access
- Persistence
- AI orchestration
- Vector search
- Background processing
- Audit rules

The frontend may contain presentation logic and UI-specific state, but it should not duplicate backend business rules.

---

# 7. Planned Next.js Route Structure

A possible future route structure is:

```text
app/
├── (public)/
│   ├── page.tsx
│   ├── pricing/
│   └── about/
│
├── (auth)/
│   ├── login/
│   ├── register/
│   └── forgot-password/
│
├── (dashboard)/
│   ├── layout.tsx
│   │
│   └── organizations/
│       └── [organizationId]/
│           ├── layout.tsx
│           ├── page.tsx
│           │
│           ├── workspaces/
│           │   ├── page.tsx
│           │   │
│           │   └── [workspaceId]/
│           │       ├── layout.tsx
│           │       ├── page.tsx
│           │       ├── documents/
│           │       ├── search/
│           │       └── chat/
│           │
│           ├── members/
│           └── settings/
│
├── error.tsx
├── loading.tsx
└── not-found.tsx
```

This is only a planned direction.

The final structure should be introduced gradually as frontend development begins.

---

# 8. Backend Architecture

The backend follows Clean Architecture and Vertical Slice Architecture.

Current solution structure:

```text
DocuMind.sln

src/
├── DocuMind.Api
├── DocuMind.Application
├── DocuMind.Domain
└── DocuMind.Infrastructure
```

---

# 9. Dependency Direction

The intended dependency direction is:

```text
DocuMind.Api
      │
      ▼
DocuMind.Application
      │
      ▼
DocuMind.Domain
```

Infrastructure provides implementations required by the application:

```text
DocuMind.Infrastructure
      │
      ├── depends on DocuMind.Application
      └── depends on DocuMind.Domain
```

Important rules:

- Domain must not depend on Application.
- Domain must not depend on Infrastructure.
- Domain must not depend on ASP.NET Core.
- Application must not depend on Infrastructure.
- Application must not depend on Entity Framework Core.
- API may depend on Application.
- Infrastructure may implement Application interfaces.

---

# 10. Layer Responsibilities

## DocuMind.Api

Responsible for:

- HTTP endpoints
- Route definitions
- Request binding
- HTTP validation concerns
- Mapping HTTP requests to commands and queries
- Mapping application results to HTTP responses
- Status codes
- Authentication middleware
- Authorization policies
- API configuration

Should not contain:

- Database queries
- Entity Framework Core logic
- Core business rules
- Document processing logic
- AI orchestration logic

---

## DocuMind.Application

Responsible for:

- Use cases
- Commands
- Queries
- Handlers
- Repository interfaces
- Application services
- Use-case orchestration
- Validation rules
- Application-level result types

Application may coordinate several dependencies.

Example:

```text
Create Workspace
    │
    ├── Check organization exists
    ├── Create workspace
    ├── Save workspace
    └── Return result
```

Application should not directly use:

- ASP.NET Core endpoint types
- Entity Framework Core DbContext
- PostgreSQL APIs
- HTTP-specific status codes

---

## DocuMind.Domain

Responsible for:

- Entities
- Value objects
- Domain rules
- Domain behavior
- Domain events where appropriate

The Domain layer should contain business concepts, not infrastructure details.

Examples:

- Organization
- Workspace
- Document
- DocumentChunk
- Membership
- Role
- Permission

---

## DocuMind.Infrastructure

Responsible for:

- Entity Framework Core
- PostgreSQL
- pgvector
- Repository implementations
- Database configuration
- Migrations
- Object storage
- External AI provider integrations
- Email provider integrations
- Background job implementations
- File processing implementations

Infrastructure implements interfaces defined by the Application layer.

---

# 11. Architectural Style

The project uses both Clean Architecture and Vertical Slice Architecture.

Clean Architecture controls dependencies and project responsibilities.

Vertical Slice Architecture organizes functionality by feature or use case.

Example:

```text
Workspaces/
├── IWorkspaceRepository.cs
│
├── CreateWorkspace/
│   ├── CreateWorkspaceCommand.cs
│   ├── CreateWorkspaceHandler.cs
│   └── CreateWorkspaceResult.cs
│
├── GetWorkspace/
│   ├── GetWorkspaceQuery.cs
│   ├── GetWorkspaceHandler.cs
│   └── GetWorkspaceResult.cs
│
└── ListWorkspaces/
    ├── ListWorkspacesQuery.cs
    ├── ListWorkspacesHandler.cs
    └── ListWorkspacesResult.cs
```

Each use case should be understandable without navigating through many technical folders.

---

# 12. Current Domain Model

## Organization

Current fields:

```text
Id
Name
CreatedAtUtc
```

## Workspace

Current fields:

```text
Id
OrganizationId
Name
CreatedAtUtc
```

Relationship:

```text
Organization
    1
    │
    *
Workspace
```

An organization may contain many workspaces.

A workspace belongs to exactly one organization.

---

# 13. Current API Design

Current organization routes:

```http
POST /api/organizations
GET  /api/organizations/{organizationId}
```

Current workspace routes:

```http
POST /api/organizations/{organizationId}/workspaces
GET  /api/organizations/{organizationId}/workspaces/{workspaceId}
```

Workspace routes are organization-scoped.

This is intentional.

The URL reflects that a workspace belongs to an organization.

Example:

```http
GET /api/organizations/{organizationId}/workspaces/{workspaceId}
```

The application verifies both:

- Workspace ID
- Organization ID

A workspace must not be returned through an organization it does not belong to.

Example:

```text
Workspace W1 belongs to Organization O1.
```

This request should return `404`:

```http
GET /api/organizations/O2/workspaces/W1
```

Even though workspace `W1` exists, it does not belong to organization `O2`.

This is an early part of enforcing multi-tenant boundaries.

---

# 14. Current Features

## Organizations

Completed:

- Create Organization
- Get Organization

Application structure:

```text
Organizations/
├── IOrganizationRepository.cs
│
├── CreateOrganization/
│   ├── CreateOrganizationCommand.cs
│   ├── CreateOrganizationHandler.cs
│   └── CreateOrganizationResult.cs
│
└── GetOrganization/
    ├── GetOrganizationQuery.cs
    ├── GetOrganizationHandler.cs
    └── GetOrganizationResult.cs
```

API structure includes:

```text
OrganizationEndpoints
```

---

## Workspaces

Completed:

- Create Workspace
- Get Workspace

Application structure:

```text
Workspaces/
├── IWorkspaceRepository.cs
│
├── CreateWorkspace/
│   ├── CreateWorkspaceCommand.cs
│   ├── CreateWorkspaceHandler.cs
│   └── CreateWorkspaceResult.cs
│
└── GetWorkspace/
    ├── GetWorkspaceQuery.cs
    ├── GetWorkspaceHandler.cs
    └── GetWorkspaceResult.cs
```

API structure includes:

```text
WorkspaceEndpoints
```

---

# 15. Repository Pattern

Repository interfaces are defined in the Application project.

Repository implementations are defined in Infrastructure.

Example:

```text
Application
    IWorkspaceRepository

Infrastructure
    WorkspaceRepository
```

Handlers depend on repository interfaces.

Handlers do not depend directly on Entity Framework Core.

Avoid introducing a generic repository.

Repositories should represent meaningful application or domain access patterns.

Examples:

```text
IOrganizationRepository
IWorkspaceRepository
IDocumentRepository
IDocumentChunkRepository
```

---

# 16. Dependency Injection

The project currently uses dependency injection for:

- Handlers
- Repositories
- DbContext

Application registers handlers.

Infrastructure registers:

- Repository implementations
- DbContext
- External infrastructure services

Typical lifetimes:

```text
DbContext      Scoped
Repositories   Scoped
Handlers       Scoped
```

Scoped means one instance is created per HTTP request.

---

# 17. Static Classes and Handler Classes

Endpoint registration classes may be static because they:

- Hold no state
- Require no constructor dependencies
- Only register endpoint mappings

Example:

```text
OrganizationEndpoints
WorkspaceEndpoints
```

Handlers should generally not be static because they:

- Receive dependencies through constructor injection
- Represent application use cases
- Are created by dependency injection

Example:

```text
CreateOrganizationHandler
GetOrganizationHandler
CreateWorkspaceHandler
GetWorkspaceHandler
```

---

# 18. Current Error Handling

Some current handlers return nullable results for simple not-found cases.

Example:

```text
CreateWorkspaceResult?
GetWorkspaceResult?
```

A `null` result currently means the requested resource or parent resource does not exist.

The API maps that result to:

```http
404 Not Found
```

Fake values must not be used to represent failure.

Avoid:

```text
Guid.Empty
DateTime.MinValue
Empty success objects
```

As the application grows, a structured result or error model may be introduced.

Possible future cases include:

- Not found
- Validation failure
- Conflict
- Unauthorized
- Forbidden
- Storage failure
- Processing failure

This should be introduced only when the simple nullable approach becomes insufficient.

---

# 19. Database

The application currently uses PostgreSQL through Docker.

Current Docker image:

```text
pgvector/pgvector:pg17
```

pgvector is included because the application will later store embedding vectors.

PostgreSQL will store:

- Organizations
- Workspaces
- Users
- Memberships
- Documents
- Document metadata
- Document chunks
- Embedding vectors
- Processing status
- Chat conversations
- Search history
- Audit data

Object or blob storage should eventually store original uploaded files.

Large files should not normally be stored directly in relational database rows.

---

# 20. Planned Document Domain

Possible future document model:

```text
Document
├── Id
├── OrganizationId
├── WorkspaceId
├── Name
├── FileName
├── ContentType
├── StorageKey
├── Size
├── Status
├── CreatedAtUtc
└── UpdatedAtUtc
```

Possible processing statuses:

```text
Uploaded
Pending
Processing
Ready
Failed
```

Possible document chunk model:

```text
DocumentChunk
├── Id
├── OrganizationId
├── WorkspaceId
├── DocumentId
├── Content
├── ChunkIndex
├── TokenCount
├── Embedding
└── CreatedAtUtc
```

This model is not final.

It should be designed when the Documents feature begins.

---

# 21. Planned Document Processing Pipeline

The expected processing flow is:

```text
User uploads document
        │
        ▼
API validates upload
        │
        ▼
Original file stored
        │
        ▼
Document record created
        │
        ▼
Background job scheduled
        │
        ▼
Text extracted
        │
        ▼
Text cleaned
        │
        ▼
Text split into chunks
        │
        ▼
Embeddings generated
        │
        ▼
Chunks and vectors stored
        │
        ▼
Document marked as Ready
```

Document processing should eventually happen outside the original HTTP request.

Uploading a large document should not require the client to wait for the entire extraction and embedding process.

---

# 22. Vector Search

The project will use PostgreSQL with pgvector.

Each document chunk will eventually have an embedding vector.

A semantic search request will follow a flow similar to:

```text
User enters a search query
        │
        ▼
Generate query embedding
        │
        ▼
Search nearest vectors
        │
        ▼
Filter by organization
        │
        ▼
Filter by workspace and permissions
        │
        ▼
Return relevant document chunks
```

Every vector query must respect tenant boundaries.

A user from one organization must never retrieve chunks belonging to another organization.

---

# 23. Retrieval-Augmented Generation

The future AI chat system will likely use Retrieval-Augmented Generation.

Expected flow:

```text
User asks a question
        │
        ▼
Question converted into an embedding
        │
        ▼
Relevant document chunks retrieved
        │
        ▼
Permissions and organization boundaries enforced
        │
        ▼
Prompt created with retrieved context
        │
        ▼
Prompt sent to language model
        │
        ▼
Answer streamed to frontend
        │
        ▼
Sources and citations displayed
```

The language model should answer from retrieved document context whenever possible.

The system should avoid presenting unsupported model output as document-grounded fact.

---

# 24. Authentication and Authorization

Authentication has not yet been implemented.

Future capabilities may include:

- User registration
- Login
- Logout
- Password reset
- Email verification
- Session or token management
- External identity providers

Authorization should eventually support:

- Organization membership
- Workspace access
- Roles
- Permissions
- Document access
- Administrative actions

Possible roles:

```text
Organization Owner
Organization Admin
Workspace Admin
Member
Viewer
```

The exact authorization model should be designed before implementation.

Multi-tenancy must not rely only on IDs supplied by the client.

The backend must verify that the authenticated user has access to the requested organization and resource.

---

# 25. Multi-Tenancy

DocuMind is a multi-tenant system.

An organization represents a tenant.

Tenant-aware resources include:

- Workspaces
- Documents
- Document chunks
- Members
- Roles
- Chat conversations
- Search requests

Tenant boundaries must be enforced consistently.

Possible protection layers include:

- Organization-scoped routes
- Authorization policies
- Application-level checks
- Repository query filters
- Database constraints
- Tests specifically designed to detect cross-tenant access

Never trust an `organizationId` simply because it was provided in the URL.

---

# 26. Background Processing

Document processing should eventually use background jobs.

Possible jobs include:

- Extract document text
- Generate document chunks
- Generate embeddings
- Reprocess failed documents
- Delete document files
- Send invitation emails
- Generate summaries
- Clean expired data

A background job system should be selected when the document-processing phase begins.

Possible options may include:

- Hosted services
- BackgroundService
- Hangfire
- Quartz.NET
- A queue-based worker architecture

The simplest suitable option should be chosen first.

---

# 27. Observability and Production Concerns

The project should eventually include:

- Structured logging
- Request tracing
- Error monitoring
- Health checks
- Database health checks
- Background worker health checks
- Metrics
- Performance monitoring
- Audit logging

Potential future areas:

- OpenTelemetry
- Centralized logs
- Application metrics
- Distributed tracing
- Alerting

These should be introduced when the system has enough behavior to justify them.

---

# 28. Testing Strategy

Testing should be added incrementally.

Potential test layers:

## Unit Tests

For:

- Domain rules
- Value objects
- Small application logic
- Validation

## Integration Tests

For:

- API endpoints
- Repository implementations
- Database queries
- Tenant isolation
- Authentication and authorization

## End-to-End Tests

For:

- Login
- Creating an organization
- Creating a workspace
- Uploading a document
- Searching
- AI chat

Tenant-isolation tests are especially important.

The system should explicitly test that users cannot access resources belonging to other organizations.

---

# 29. Development Roadmap

The roadmap is flexible.

Architecture should evolve based on actual requirements rather than following this list blindly.

## Phase 1 — Foundation

- Create backend solution
- Set up Clean Architecture
- Set up Vertical Slice Architecture
- Configure dependency injection
- Configure PostgreSQL
- Configure Docker
- Create Organization
- Get Organization
- Create Workspace
- Get Workspace

Status:

```text
Completed
```

---

## Phase 2 — Complete Basic Workspace Functionality

Possible next steps:

- List workspaces for an organization
- Update workspace
- Delete workspace
- Input validation
- Better application errors
- API response consistency
- Integration tests

---

## Phase 3 — Documents

- Define Document entity
- Upload document
- Store metadata
- Store original file
- Get document
- List documents
- Download document
- Delete document
- Processing status

---

## Phase 4 — Background Processing

- Introduce background jobs
- Extract text
- Handle processing failures
- Retry failed processing
- Track processing progress

---

## Phase 5 — Embeddings and Vector Database

- Enable pgvector extension
- Define document chunks
- Implement chunking
- Generate embeddings
- Store vectors
- Query nearest vectors
- Filter vector results by tenant

---

## Phase 6 — Semantic Search

- Search within a workspace
- Search across organization documents
- Return ranked chunks
- Return source document metadata
- Add search filters
- Add pagination

---

## Phase 7 — AI Chat and RAG

- Create conversations
- Store messages
- Retrieve relevant document context
- Build prompts
- Call language model
- Stream responses
- Show citations
- Handle unsupported answers

---

## Phase 8 — Authentication and Authorization

The exact timing may change.

Possible work:

- User registration
- Login
- Organization membership
- Invitations
- Roles
- Permissions
- Protected endpoints
- Tenant authorization
- Audit logging

Authentication may be introduced earlier if upcoming features require a real user identity.

---

## Phase 9 — Next.js Frontend

- Create Next.js application
- Configure App Router
- Create public and authenticated layouts
- Add authentication screens
- Add organization selection
- Add workspace dashboard
- Add workspace management
- Add document upload
- Add document explorer
- Add processing status
- Add semantic search
- Add AI chat
- Add streaming responses
- Add loading and error states

---

## Phase 10 — Production Readiness

- Structured logging
- Monitoring
- Health checks
- Rate limiting
- Caching
- Security review
- Performance testing
- CI/CD
- Deployment
- Backups
- Secrets management

---

# 30. Development Principles

Prefer:

- Simple, explicit code
- Small vertical slices
- Constructor injection
- Async and await
- CancellationToken
- Sealed classes where inheritance is not needed
- Records for commands, queries, and results
- Feature folders
- Clear names
- Small, focused interfaces
- Business rules enforced on the backend
- Incremental architecture

Avoid:

- Generic repositories
- Large service classes
- Static business services
- Premature microservices
- Premature event-driven architecture
- Unnecessary abstractions
- Magic code
- Framework-heavy solutions without understanding them
- Moving backend business logic into Next.js
- Overengineering simple use cases

---

# 31. Coding Style

Preferred patterns:

```text
CreateWorkspaceCommand
CreateWorkspaceHandler
CreateWorkspaceResult
```

```text
GetWorkspaceQuery
GetWorkspaceHandler
GetWorkspaceResult
```

Prefer one use case per handler.

Prefer constructor injection.

Prefer cancellation support for asynchronous operations.

Use explicit result types rather than returning domain entities directly from API endpoints.

Keep HTTP concerns in the API project.

Keep persistence concerns in Infrastructure.

---

# 32. Preferred Teaching Style

The user is an experienced frontend developer learning backend architecture.

The assistant should:

- Explain why before showing how
- Explain responsibilities of each file
- Explain dependency direction
- Explain what happens at runtime
- Compare reasonable alternatives
- Point out trade-offs
- Ask conceptual questions when useful
- Review the user's implementation
- Let the user write the code when appropriate
- Provide the full solution when explicitly requested
- Avoid restarting the architecture without a strong reason
- Preserve previous project decisions
- Avoid overwhelming the user with unrelated patterns

Treat the user as capable but still learning backend architecture.

Do not oversimplify important concepts.

Do not introduce advanced abstractions merely to appear production-ready.

---

# 33. Current Project Status

Current date of this context:

```text
2026-07-30
```

Completed:

- Backend solution structure
- Clean Architecture project separation
- Vertical Slice feature organization
- PostgreSQL running through Docker
- pgvector PostgreSQL image
- Entity Framework Core configuration
- Application dependency injection
- Infrastructure dependency injection
- Organization repository
- Workspace repository
- Create Organization
- Get Organization
- Create Workspace
- Get Workspace
- Organization-scoped workspace routes
- HTTP request file for manual API testing

Current working routes:

```http
POST /api/organizations
GET  /api/organizations/{organizationId}

POST /api/organizations/{organizationId}/workspaces
GET  /api/organizations/{organizationId}/workspaces/{workspaceId}
```

---

# 34. Current Important Decisions

## Organization-scoped Workspace Routes

Chosen route style:

```http
/api/organizations/{organizationId}/workspaces/{workspaceId}
```

Reason:

- Makes the parent-child relationship explicit
- Supports multi-tenant boundaries
- Keeps the organization context visible
- Allows queries to validate both organization and workspace IDs

---

## Handler Classes Are Not Static

Handlers receive dependencies through constructor injection.

Example dependencies:

```text
IWorkspaceRepository
IOrganizationRepository
```

Dependency injection creates handler instances.

---

## Endpoint Registration Classes May Be Static

Endpoint mapping classes hold no state and only register routes.

They are suitable for extension methods such as:

```text
MapOrganizationEndpoints
MapWorkspaceEndpoints
```

---

## Scoped Lifetime

The following are currently scoped:

- DbContext
- Repositories
- Handlers

This gives one instance per HTTP request.

---

## Nullable Results for Simple Not Found Cases

Current handlers may return `null` when a resource does not exist.

The API converts that into `404 Not Found`.

A richer result model may be introduced later when more failure types appear.

---

## Next.js Is the Frontend Framework

The final frontend should use:

```text
Next.js
React
TypeScript
App Router
```

Next.js provides useful conventions for:

- Nested layouts
- Protected application sections
- Route-level loading states
- Error boundaries
- Server and client components
- Streaming AI responses
- Dashboard routing

The ASP.NET Core API remains the backend and source of business truth.

---

# 35. Likely Next Development Step

The most likely next feature is:

```text
List Workspaces for an Organization
```

Possible route:

```http
GET /api/organizations/{organizationId}/workspaces
```

After that, likely topics include:

- Update workspace
- Delete workspace
- Validation
- Structured errors
- Integration tests
- Documents feature

The next step should still be discussed before implementation.

---

# 36. Instructions for a New Assistant

When this file is provided in a new conversation:

- Continue the existing DocuMind project.
- Do not redesign the architecture from scratch.
- Preserve the current route conventions.
- Preserve Clean Architecture boundaries.
- Preserve Vertical Slice organization.
- Remember that Next.js is the planned frontend.
- Remember that the final goal includes pgvector, embeddings, semantic search, and AI chat.
- Explain concepts before introducing implementations.
- Allow the user to implement code themselves when they prefer.
- Provide complete code when directly requested.
- Track completed work and architectural decisions.
- Suggest updating this file after meaningful milestones.

Before recommending a new abstraction, explain:

1. What problem it solves.
2. Whether that problem exists yet.
3. What simpler alternatives exist.
4. Why the abstraction is justified now.

---

# 37. Context Maintenance

This document should be updated after meaningful milestones.

Update areas such as:

- Completed features
- Current domain model
- Route conventions
- New infrastructure
- Authentication design
- Document processing design
- Vector search design
- Frontend architecture
- Important trade-offs
- Next development step

