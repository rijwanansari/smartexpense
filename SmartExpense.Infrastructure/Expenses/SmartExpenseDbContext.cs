using Microsoft.EntityFrameworkCore;
using SmartExpense.Domain.Expenses;

namespace SmartExpense.Infrastructure.Expenses;

public sealed class SmartExpenseDbContext(DbContextOptions<SmartExpenseDbContext> options) : DbContext(options)
{
    public DbSet<Expense> Expenses => Set<Expense>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Expense>(builder =>
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Amount).IsRequired();
            builder.Property(e => e.Category).IsRequired();
            builder.Property(e => e.ExpenseDate).IsRequired();
            builder.Property(e => e.Description).UsePropertyAccessMode(PropertyAccessMode.Field);
        });
    }
}
