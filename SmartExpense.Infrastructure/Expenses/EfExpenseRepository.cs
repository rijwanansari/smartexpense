using SmartExpense.Application.Expenses;
using SmartExpense.Domain.Expenses;

namespace SmartExpense.Infrastructure.Expenses;

public sealed class EfExpenseRepository(SmartExpenseDbContext dbContext) : IExpenseRepository
{
    public async Task AddAsync(Expense expense, CancellationToken cancellationToken)
    {
        await dbContext.Expenses.AddAsync(expense, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
