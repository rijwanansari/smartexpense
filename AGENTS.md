# Repository instructions

## Architecture

- .NET 10
- Clean Architecture
- Modular Monolith for Core
- AI and Notifications are independently deployable services
- Multi-tenant by design
- Domain-driven design
- Explicit module boundaries
- Event-driven integration
- Transactional Outbox
- Inbox/idempotency
- No shared database between independently deployable services
- Core modules own their data through explicit module boundaries

## .NET conventions

- Target .NET 10 (`net10.0`) and preserve the existing nullable-reference-type and implicit-using settings in project files.
- Keep the Domain project independent of web, persistence, and infrastructure frameworks; follow the project-reference direction already established by the solution.
- The API currently uses ASP.NET Core minimal APIs and OpenAPI. Keep endpoints focused on HTTP concerns and place use-case and domain behavior in their owning layers.
- The current test project uses xUnit. Prefer its existing test setup and patterns before introducing new test packages or infrastructure.

## Rules

1. Domain must not depend on Infrastructure.
2. Domain must not depend on ASP.NET Core.
3. Domain must not depend on EF Core.
4. Core modules must not access another module's DbContext, repositories,
    entities, schema-owned tables, or Infrastructure project.
5. Independently deployable services must not access another service's database
    or reference another service's assemblies directly.
6. Tenant context must never come from an untrusted request body.
7. All tenant-owned data must have TenantId.
8. All integration events must contain TenantId.
9. All asynchronous consumers must be idempotent.
10. No generic repository abstraction unless justified.
11. No distributed transactions.
12. No business logic in controllers/endpoints.
13. APIs must return ProblemDetails.
14. Every production feature requires tests.
15. Architecture tests must protect dependency boundaries.

## Testing

Production features require tests.

Use:

- Unit tests
- Integration tests
- Architecture tests
- Contract tests
- Add integration tests for important HTTP behavior when appropriate.
- Test invalid amounts, missing categories, future dates, and date boundaries.
- Run dotnet build and dotnet test before declaring completion.
- Never claim a test passed unless it was executed successfully.

Important architectural tests:

- Domain does not depend on Infrastructure
- Domain does not depend on ASP.NET
- Modules do not access another module's Infrastructure
- Services do not access another service
- Tenant isolation rules remain enforced

## Agent boundaries

- Inspect existing code before changing it.
- Follow existing patterns unless there is a clear reason to change them.
- Do not add secrets, credentials, or personal data.
- Do not modify CI, security settings, or unrelated projects unless required.
- Summarize changes, test results, assumptions, and remaining risks.
- Do not merge or deploy changes without explicit human approval.

## Build and test

Run these from the repository root:

- Build: `dotnet build SmartExpense.slnx`
- Tests: `dotnet test SmartExpense.slnx`
- Run the API: `dotnet run --project SmartExpense.Api`
