using Microsoft.AspNetCore.Mvc;
using SmartExpense.Application.Expenses;

namespace SmartExpense.Api.Expenses;

/// <summary>
/// HTTP endpoints for expenses. No business logic lives here; this layer
/// only maps HTTP requests to Application commands and maps results back
/// to HTTP responses (success or ProblemDetails).
/// </summary>
public static class ExpenseEndpoints
{
    public static IEndpointRouteBuilder MapExpenseEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/expenses", async (
            CreateExpenseRequest request,
            CreateExpenseHandler handler,
            CancellationToken cancellationToken) =>
        {
            var command = new CreateExpenseCommand(request.Amount, request.Category, request.ExpenseDate, request.Description);
            var result = await handler.HandleAsync(command, cancellationToken);

            if (!result.IsSuccess)
            {
                return Results.ValidationProblem(new Dictionary<string, string[]>
                {
                    ["request"] = result.Errors.ToArray(),
                });
            }

            var value = result.Value!;
            var response = new ExpenseResponse(value.Id, value.Amount, value.Category, value.ExpenseDate, value.Description);
            return Results.Created($"/expenses/{response.Id}", response);
        })
        .WithName("CreateExpense")
        .Produces<ExpenseResponse>(StatusCodes.Status201Created)
        .ProducesValidationProblem();

        return app;
    }
}
