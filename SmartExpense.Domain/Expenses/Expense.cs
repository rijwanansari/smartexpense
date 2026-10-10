using SmartExpense.Domain.Common;

namespace SmartExpense.Domain.Expenses;

/// <summary>
/// An expense record. Invariants are enforced only through <see cref="Create"/>;
/// there is no public constructor so an invalid instance cannot exist.
/// </summary>
/// <remarks>
/// NOTE: AGENTS.md rule 7 requires all tenant-owned data to carry a TenantId.
/// Tenant context resolution was explicitly deferred for this increment
/// (no auth/tenant-resolution middleware exists yet). This entity does not
/// yet carry a TenantId and is therefore not tenant-isolated. This is a
/// known, flagged gap to close before this feature is considered complete
/// for multi-tenant use.
/// </remarks>
public sealed class Expense
{
    public Guid Id { get; }
    public decimal Amount { get; }
    public string Category { get; }
    public DateOnly ExpenseDate { get; }
    public string? Description { get; }

    private Expense(Guid id, decimal amount, string category, DateOnly expenseDate, string? description)
    {
        Id = id;
        Amount = amount;
        Category = category;
        ExpenseDate = expenseDate;
        Description = description;
    }

    /// <summary>
    /// Creates a new <see cref="Expense"/>, validating all invariants.
    /// </summary>
    /// <param name="amount">Must be strictly greater than zero.</param>
    /// <param name="category">Must be non-null, non-empty, non-whitespace.</param>
    /// <param name="expenseDate">Must not be later than <paramref name="today"/>.</param>
    /// <param name="today">The caller-supplied current date, so date validation
    /// stays deterministic and framework-independent (no hidden clock access).</param>
    /// <param name="description">Optional free-text description.</param>
    /// <exception cref="DomainValidationException">
    /// Thrown with every violated invariant when one or more checks fail.
    /// </exception>
    public static Expense Create(decimal amount, string? category, DateOnly expenseDate, DateOnly today, string? description = null)
    {
        var errors = new List<string>();

        if (amount <= 0)
        {
            errors.Add("Amount must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(category))
        {
            errors.Add("Category is required.");
        }

        if (expenseDate > today)
        {
            errors.Add("Expense date cannot be in the future.");
        }

        if (errors.Count > 0)
        {
            throw new DomainValidationException(errors);
        }

        return new Expense(Guid.NewGuid(), amount, category!.Trim(), expenseDate, description);
    }
}
