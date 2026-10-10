using NetArchTest.Rules;

namespace SmartExpense.Tests.Architecture;

/// <summary>
/// Protects the dependency boundaries required by AGENTS.md rules 1-3:
/// Domain must not depend on Infrastructure, ASP.NET Core, or EF Core.
/// </summary>
public class DependencyBoundaryTests
{
    [Fact]
    public void Domain_Should_Not_DependOn_Infrastructure()
    {
        var result = Types.InAssembly(typeof(global::SmartExpense.Domain.Expenses.Expense).Assembly)
            .ShouldNot()
            .HaveDependencyOn("SmartExpense.Infrastructure")
            .GetResult();

        Assert.True(result.IsSuccessful, Describe(result));
    }

    [Fact]
    public void Domain_Should_Not_DependOn_AspNetCore()
    {
        var result = Types.InAssembly(typeof(global::SmartExpense.Domain.Expenses.Expense).Assembly)
            .ShouldNot()
            .HaveDependencyOnAny("Microsoft.AspNetCore")
            .GetResult();

        Assert.True(result.IsSuccessful, Describe(result));
    }

    [Fact]
    public void Domain_Should_Not_DependOn_EfCore()
    {
        var result = Types.InAssembly(typeof(global::SmartExpense.Domain.Expenses.Expense).Assembly)
            .ShouldNot()
            .HaveDependencyOnAny("Microsoft.EntityFrameworkCore")
            .GetResult();

        Assert.True(result.IsSuccessful, Describe(result));
    }

    [Fact]
    public void Application_Should_Not_DependOn_Infrastructure()
    {
        var result = Types.InAssembly(typeof(global::SmartExpense.Application.Expenses.CreateExpenseHandler).Assembly)
            .ShouldNot()
            .HaveDependencyOn("SmartExpense.Infrastructure")
            .GetResult();

        Assert.True(result.IsSuccessful, Describe(result));
    }

    [Fact]
    public void Application_Should_Not_DependOn_AspNetCore()
    {
        var result = Types.InAssembly(typeof(global::SmartExpense.Application.Expenses.CreateExpenseHandler).Assembly)
            .ShouldNot()
            .HaveDependencyOnAny("Microsoft.AspNetCore")
            .GetResult();

        Assert.True(result.IsSuccessful, Describe(result));
    }

    private static string Describe(TestResult result) =>
        result.FailingTypes is null
            ? "Unknown failure"
            : string.Join(", ", result.FailingTypes.Select(t => t.FullName));
}
