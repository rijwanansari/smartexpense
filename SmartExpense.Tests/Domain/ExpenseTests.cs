using SmartExpense.Domain.Common;
using SmartExpense.Domain.Expenses;

namespace SmartExpense.Tests.Domain;

public class ExpenseTests
{
    private static readonly DateOnly Today = new(2026, 1, 15);

    [Fact]
    public void Create_WithValidInput_Succeeds()
    {
        var expense = Expense.Create(amount: 42.50m, category: "Travel", expenseDate: Today, today: Today, description: "Taxi");

        Assert.Equal(42.50m, expense.Amount);
        Assert.Equal("Travel", expense.Category);
        Assert.Equal(Today, expense.ExpenseDate);
        Assert.Equal("Taxi", expense.Description);
        Assert.NotEqual(Guid.Empty, expense.Id);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-0.01)]
    public void Create_WithNonPositiveAmount_Throws(decimal amount)
    {
        var ex = Assert.Throws<DomainValidationException>(() =>
            Expense.Create(amount, "Travel", Today, Today));

        Assert.Contains("Amount must be greater than zero.", ex.Errors);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithMissingCategory_Throws(string? category)
    {
        var ex = Assert.Throws<DomainValidationException>(() =>
            Expense.Create(10m, category, Today, Today));

        Assert.Contains("Category is required.", ex.Errors);
    }

    [Fact]
    public void Create_WithFutureDate_Throws()
    {
        var futureDate = Today.AddDays(1);

        var ex = Assert.Throws<DomainValidationException>(() =>
            Expense.Create(10m, "Travel", futureDate, Today));

        Assert.Contains("Expense date cannot be in the future.", ex.Errors);
    }

    [Fact]
    public void Create_WithDateEqualToToday_Succeeds()
    {
        var expense = Expense.Create(10m, "Travel", Today, Today);

        Assert.Equal(Today, expense.ExpenseDate);
    }

    [Fact]
    public void Create_WithPastDate_Succeeds()
    {
        var pastDate = Today.AddDays(-1);

        var expense = Expense.Create(10m, "Travel", pastDate, Today);

        Assert.Equal(pastDate, expense.ExpenseDate);
    }

    [Fact]
    public void Create_WithMultipleViolations_ReturnsAllErrors()
    {
        var ex = Assert.Throws<DomainValidationException>(() =>
            Expense.Create(0, "", Today.AddDays(1), Today));

        Assert.Equal(3, ex.Errors.Count);
    }

    [Fact]
    public void Create_TrimsCategoryWhitespace()
    {
        var expense = Expense.Create(10m, "  Travel  ", Today, Today);

        Assert.Equal("Travel", expense.Category);
    }
}
