namespace SmartExpense.Domain.Common;

/// <summary>
/// Raised when a domain invariant is violated. Carries every violation found,
/// not just the first, so callers can surface all errors at once.
/// </summary>
public sealed class DomainValidationException : Exception
{
    public IReadOnlyCollection<string> Errors { get; }

    public DomainValidationException(IReadOnlyCollection<string> errors)
        : base(string.Join("; ", errors))
    {
        if (errors.Count == 0)
        {
            throw new ArgumentException("At least one error is required.", nameof(errors));
        }

        Errors = errors;
    }
}
