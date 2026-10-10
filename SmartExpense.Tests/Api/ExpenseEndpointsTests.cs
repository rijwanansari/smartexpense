using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using SmartExpense.Api.Expenses;

namespace SmartExpense.Tests.Api;

public class ExpenseEndpointsTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task PostExpenses_WithValidRequest_Returns201WithLocation()
    {
        var client = factory.CreateClient();
        var request = new CreateExpenseRequest(25.5m, "Travel", DateOnly.FromDateTime(DateTime.UtcNow), "Taxi");

        var response = await client.PostAsJsonAsync("/expenses", request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<ExpenseResponse>();
        Assert.NotNull(body);
        Assert.Equal(25.5m, body!.Amount);
        Assert.NotNull(response.Headers.Location);
    }

    [Fact]
    public async Task PostExpenses_WithInvalidAmount_ReturnsValidationProblem()
    {
        var client = factory.CreateClient();
        var request = new CreateExpenseRequest(0m, "Travel", DateOnly.FromDateTime(DateTime.UtcNow), null);

        var response = await client.PostAsJsonAsync("/expenses", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        Assert.NotNull(problem);
        Assert.Contains(problem!.Errors, kvp => kvp.Value.Contains("Amount must be greater than zero."));
    }

    [Fact]
    public async Task PostExpenses_WithFutureDate_ReturnsValidationProblem()
    {
        var client = factory.CreateClient();
        var futureDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(2);
        var request = new CreateExpenseRequest(10m, "Travel", futureDate, null);

        var response = await client.PostAsJsonAsync("/expenses", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
