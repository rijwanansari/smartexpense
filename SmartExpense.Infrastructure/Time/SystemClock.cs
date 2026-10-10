using SmartExpense.Application.Common;

namespace SmartExpense.Infrastructure.Time;

public sealed class SystemClock : IClock
{
    public DateOnly Today => DateOnly.FromDateTime(DateTime.UtcNow);
}
