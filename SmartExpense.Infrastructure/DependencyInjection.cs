using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SmartExpense.Application.Common;
using SmartExpense.Application.Expenses;
using SmartExpense.Infrastructure.Expenses;
using SmartExpense.Infrastructure.Time;

namespace SmartExpense.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<SmartExpenseDbContext>(options => options.UseInMemoryDatabase("SmartExpense"));
        services.AddScoped<IExpenseRepository, EfExpenseRepository>();
        services.AddSingleton<IClock, SystemClock>();
        return services;
    }
}
