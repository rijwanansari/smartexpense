namespace SmartExpense.Application.Expenses;

/// <summary>
/// Output of a successful create-expense use case.
/// </summary>
public sealed record CreateExpenseResult(Guid Id, decimal Amount, string Category, DateOnly ExpenseDate, string? Description);
