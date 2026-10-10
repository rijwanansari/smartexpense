namespace SmartExpense.Application.Expenses;

/// <summary>
/// Input for the create-expense use case.
/// </summary>
public sealed record CreateExpenseCommand(decimal Amount, string? Category, DateOnly ExpenseDate, string? Description);
