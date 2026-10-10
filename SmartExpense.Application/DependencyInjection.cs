using Microsoft.Extensions.DependencyInjection;

namespace SmartExpense.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<Expenses.CreateExpenseHandler>();
        return services;
    }
}
