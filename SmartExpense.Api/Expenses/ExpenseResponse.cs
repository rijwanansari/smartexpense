namespace SmartExpense.Api.Expenses;

public sealed record ExpenseResponse(Guid Id, decimal Amount, string Category, DateOnly ExpenseDate, string? Description);
