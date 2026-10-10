namespace SmartExpense.Application.Common;

/// <summary>
/// Abstraction over "now", so date-dependent validation (e.g. future-date
/// rejection) stays deterministic and testable without a hidden system clock.
/// </summary>
public interface IClock
{
    DateOnly Today { get; }
}
