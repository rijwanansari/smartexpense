# Known Gap: Expense entity has no TenantId (tenant isolation not enforced)

## Status

Open. Deliberately deferred during the first create-expense increment (2026-10-10).

## AGENTS.md rules affected

- Rule 7: "All tenant-owned data must have TenantId."
- Rule 6: "Tenant context must never come from an untrusted request body."
- "Tenant isolation rules remain enforced" (Testing section — architecture tests).

## Current state

`SmartExpense.Domain/Expenses/Expense.cs` has no `TenantId` property, and
`POST /expenses` (`SmartExpense.Api/Expenses/ExpenseEndpoints.cs`) does not
resolve or require any tenant context. `SmartExpense.Infrastructure/Expenses/EfExpenseRepository.cs`
persists and would later query expenses without any tenant filter.

This was a scoped decision for the first increment: no authentication or
tenant-resolution middleware exists yet in the solution, so tenant handling
was deferred rather than stubbed with an untrustworthy source (e.g., reading
TenantId from the request body, which rule 6 explicitly forbids).

## Impact

- Expense data from different tenants cannot currently be isolated; any
  multi-tenant deployment of this feature as-is would leak data across
  tenants.
- No architecture or integration test currently enforces tenant isolation
  for expenses, because there is nothing to enforce yet.
- This gap blocks production/multi-tenant readiness of the create-expense
  feature. It does not block single-tenant local development or the
  currently implemented unit/integration/architecture tests.

## Required follow-up

1. Decide the tenant-resolution strategy (e.g., authenticated user claim,
   trusted header populated by an API gateway/auth middleware, or
   subdomain-based resolution). Must not read TenantId from the request
   body (rule 6).
2. Add `TenantId` to the `Expense` domain entity and its creation invariants.
3. Update `SmartExpenseDbContext` EF configuration and any future
   migrations to include and index `TenantId`.
4. Update `IExpenseRepository`/`EfExpenseRepository` so all reads/writes are
   scoped to the current tenant.
5. Add an architecture test asserting tenant-scoped query/repository usage,
   plus an integration test proving one tenant cannot read/create another
   tenant's expenses.

## References

- `AGENTS.md` (rules 6, 7, and the Testing section)
- `SmartExpense.Domain/Expenses/Expense.cs`
- `SmartExpense.Api/Expenses/ExpenseEndpoints.cs`
- `SmartExpense.Infrastructure/Expenses/EfExpenseRepository.cs`
