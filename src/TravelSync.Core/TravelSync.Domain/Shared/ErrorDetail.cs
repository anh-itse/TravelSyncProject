namespace TravelSync.Domain.Shared;

public sealed class ErrorDetail(string code, string message) : IEquatable<ErrorDetail>
{
    public static readonly ErrorDetail None = new (string.Empty, string.Empty);
    public static readonly ErrorDetail NotFound = new ("Error.NotFound", "The resource was not found.");
    public static readonly ErrorDetail Null = new ("Error.NullValue", "The specified result is null.");
    public static readonly ErrorDetail Unauthorized = new ("Error.Unauthorized", "You are not authorized.");
    public static readonly ErrorDetail Forbidden = new ("Error.Forbidden", "Access is denied.");
    public static readonly ErrorDetail BadRequest = new ("Error.BadRequest", "Invalid request.");
    public static readonly ErrorDetail InternalServerError = new ("Error.Internal", "An internal server error occurred.");
    public static readonly ErrorDetail ValidationError = new ("Error.Validation", "Validation failed.");

    public string Code { get; init; } = code;
    public string Message { get; init; } = message;

    public static implicit operator string(ErrorDetail error) => error?.Code ?? string.Empty;

    public static bool operator ==(ErrorDetail? left, ErrorDetail? right)
    {
        if (ReferenceEquals(left, right)) return true;
        if (left is null || right is null) return false;
        return StringComparer.OrdinalIgnoreCase.Equals(left.Code, right.Code);
    }

    public static bool operator !=(ErrorDetail? left, ErrorDetail? right) => !(left == right);

    public ErrorDetail WithMessage(string message) => new (this.Code, message);

    public bool Equals(ErrorDetail? other) => other is not null && this.Code == other.Code && this.Message == other.Message;

    public override bool Equals(object? obj) => obj is ErrorDetail other && this.Equals(other);

    public override int GetHashCode() => HashCode.Combine(this.Code, this.Message);

    public override string ToString() => string.IsNullOrEmpty(this.Message) ? this.Code : $"{this.Code}: {this.Message}";
}