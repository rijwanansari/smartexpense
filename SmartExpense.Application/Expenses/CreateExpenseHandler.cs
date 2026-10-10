using SmartExpense.Application.Common;
using SmartExpense.Domain.Common;
using SmartExpense.Domain.Expenses;

namespace SmartExpense.Application.Expenses;

/// <summary>
/// Use-case handler for creating an expense. Translates domain invariant
/// violations into a <see cref="Result{T}"/> failure instead of letting
/// exceptions cross the Application boundary for expected validation cases.
/// </summary>
public sealed class CreateExpenseHandler(IExpenseRepository repository, IClock clock)
{
    public async Task<Result<CreateExpenseResult>> HandleAsync(CreateExpenseCommand command, CancellationToken cancellationToken)
    {
        Expense expense;
        try
        {
            expense = Expense.Create(command.Amount, command.Category, command.ExpenseDate, clock.Today, command.Description);
        }
        catch (DomainValidationException ex)
        {
            return Result<CreateExpenseResult>.Failure(ex.Errors);
        }

        await repository.AddAsync(expense, cancellationToken);

        return Result<CreateExpenseResult>.Success(
            new CreateExpenseResult(expense.Id, expense.Amount, expense.Category, expense.ExpenseDate, expense.Description));
    }
}
