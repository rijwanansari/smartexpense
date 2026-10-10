using SmartExpense.Application.Common;
using SmartExpense.Application.Expenses;
using SmartExpense.Domain.Expenses;

namespace SmartExpense.Tests.Application;

internal sealed class FixedClock(DateOnly today) : IClock
{
    public DateOnly Today { get; } = today;
}

internal sealed class InMemoryExpenseRepository : IExpenseRepository
{
    public List<Expense> Added { get; } = [];

    public Task AddAsync(Expense expense, CancellationToken cancellationToken)
    {
        Added.Add(expense);
        return Task.CompletedTask;
    }
}

public class CreateExpenseHandlerTests
{
    private static readonly DateOnly Today = new(2026, 1, 15);

    [Fact]
    public async Task HandleAsync_WithValidCommand_PersistsAndReturnsSuccess()
    {
        var repository = new InMemoryExpenseRepository();
        var handler = new CreateExpenseHandler(repository, new FixedClock(Today));
        var command = new CreateExpenseCommand(25m, "Meals", Today, "Lunch");

        var result = await handler.HandleAsync(command, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Single(repository.Added);
        Assert.Equal(25m, result.Value!.Amount);
        Assert.Equal("Meals", result.Value.Category);
    }

    [Fact]
    public async Task HandleAsync_WithInvalidAmount_ReturnsFailureAndDoesNotPersist()
    {
        var repository = new InMemoryExpenseRepository();
        var handler = new CreateExpenseHandler(repository, new FixedClock(Today));
        var command = new CreateExpenseCommand(0m, "Meals", Today, null);

        var result = await handler.HandleAsync(command, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Contains("Amount must be greater than zero.", result.Errors);
        Assert.Empty(repository.Added);
    }

    [Fact]
    public async Task HandleAsync_WithFutureDate_ReturnsFailure()
    {
        var repository = new InMemoryExpenseRepository();
        var handler = new CreateExpenseHandler(repository, new FixedClock(Today));
        var command = new CreateExpenseCommand(10m, "Meals", Today.AddDays(1), null);

        var result = await handler.HandleAsync(command, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Contains("Expense date cannot be in the future.", result.Errors);
    }

    [Fact]
    public async Task HandleAsync_WithMissingCategory_ReturnsFailure()
    {
        var repository = new InMemoryExpenseRepository();
        var handler = new CreateExpenseHandler(repository, new FixedClock(Today));
        var command = new CreateExpenseCommand(10m, "   ", Today, null);

        var result = await handler.HandleAsync(command, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Contains("Category is required.", result.Errors);
    }
}
