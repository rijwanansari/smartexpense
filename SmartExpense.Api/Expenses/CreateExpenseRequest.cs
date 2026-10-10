namespace SmartExpense.Api.Expenses;

public sealed record CreateExpenseRequest(decimal Amount, string? Category, DateOnly ExpenseDate, string? Description);
