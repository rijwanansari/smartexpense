using SmartExpense.Domain.Expenses;

namespace SmartExpense.Application.Expenses;

/// <summary>
/// Persistence port for <see cref="Expense"/>. Implemented in Infrastructure.
/// </summary>
public interface IExpenseRepository
{
    Task AddAsync(Expense expense, CancellationToken cancellationToken);
}
