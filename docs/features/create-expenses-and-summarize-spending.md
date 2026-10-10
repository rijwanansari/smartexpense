# Feature: Create expenses and summarize spending

## User story
As a user, I want to record expenses and view spending grouped by category over a date range.

## Requirements
- Create an expense with amount, category, transaction date, and description.
- Reject amounts less than or equal to zero.
- Reject blank categories and descriptions.
- Reject future transaction dates.
- Provide a spending summary grouped by category for an inclusive date range.
- Reject invalid date ranges where the start date is after the end date.
- Use decimal for monetary calculations.
- Use UTC or a clearly documented date-handling convention consistently.

## Architecture
- Follow AGENTS.md and existing Clean Architecture boundaries.
- Put business rules in the appropriate domain or application layer.
- Keep persistence behind an abstraction.
- Use in-memory persistence for this demonstration unless a database already exists.

## Acceptance criteria
- Valid expenses can be created through the API.
- Invalid inputs return appropriate validation responses.
- Category summaries include only expenses within the requested inclusive date range.
- Unit tests cover successful behavior and all specified validation rules.
- API integration tests cover the main HTTP scenarios.
- `dotnet build` and `dotnet test` pass.

## Out of scope
- Authentication and authorization.
- Production database setup.
- Multi-currency conversion.
- Deployment to a cloud environment.
