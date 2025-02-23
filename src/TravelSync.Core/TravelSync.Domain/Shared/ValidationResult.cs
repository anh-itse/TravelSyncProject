namespace TravelSync.Domain.Shared;

public sealed class ValidationResult : OperationResult, IValidationResult
{
    private ValidationResult(ErrorDetail[] errors)
        : base(false, IValidationResult.ValidationError) => this.Errors = errors;

    public new IReadOnlyList<ErrorDetail> Errors { get; }

    public static ValidationResult WithErrors(ErrorDetail[] errors) => new (errors);
}

public sealed class ValidationResult<TValue> : OperationResult<TValue>, IValidationResult
    where TValue : class
{
    private ValidationResult(ErrorDetail[] errors)
        : base(default, false, IValidationResult.ValidationError) => this.Errors = errors;

    public new IReadOnlyList<ErrorDetail> Errors { get; }

    public ValidationResult<TValue> WithErrors(ErrorDetail[] errors) => new (errors);
}
