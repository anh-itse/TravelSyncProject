namespace TravelSync.Domain.Shared;

public interface IValidationResult
{
    public static readonly ErrorDetail ValidationError = new ("ValidationError", "A validation problem occurred.");

    public IReadOnlyList<ErrorDetail> Errors { get; }
}
