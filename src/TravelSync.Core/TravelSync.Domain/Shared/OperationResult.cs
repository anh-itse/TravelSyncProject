namespace TravelSync.Domain.Shared;

public class OperationResult
{
    protected internal OperationResult(bool isSuccess, ErrorDetail? errorDetail)
    {
        this.IsSuccess = isSuccess;
        this.ErrorDetail = errorDetail;
        this.Errors = [];
    }

    protected internal OperationResult(bool isSuccess, IReadOnlyCollection<ErrorDetail> errors)
    {
        this.IsSuccess = isSuccess;
        this.Errors = errors ?? [];
        this.ErrorDetail = errors?.FirstOrDefault();
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !this.IsSuccess;

    /// <summary>
    /// Danh sách lỗi xử lý cho những trường hợp hợp cần trả về nhiều lỗi.
    /// </summary>
    public IReadOnlyCollection<ErrorDetail> Errors { get; }

    /// <summary>
    /// Thông tin lỗi hiện tại, nếu có nhiều lỗi thì lấy giá trị đầu tiên.
    /// </summary>
    public ErrorDetail? ErrorDetail { get; }

    public static OperationResult Success() => new (true, ErrorDetail.None);

    public static OperationResult Failure(ErrorDetail appError) => new (false, appError);

    public static OperationResult Failure(IReadOnlyCollection<ErrorDetail> errors) => new (false, errors);
}

public class OperationResult<TValue> : OperationResult
{
    private readonly TValue? _value;

    protected internal OperationResult(TValue? value, bool isSuccess, IReadOnlyCollection<ErrorDetail> errors)
        : base(isSuccess, errors) => this._value = value;

    protected internal OperationResult(TValue? value, bool isSuccess, ErrorDetail? error)
        : base(isSuccess, error) => this._value = value;

    public TValue? Data => this._value;

    public static implicit operator OperationResult<TValue>(TValue? value) => Create(value);

    public static OperationResult<TValue> Create(TValue? value) =>
        value is null
            ? new OperationResult<TValue>(default, false, [ErrorDetail.Null])
            : new OperationResult<TValue>(value, true, []);

    public static OperationResult<TValue> ToOperationResult(TValue? value) =>
        value is null
            ? new OperationResult<TValue>(default, false, [ErrorDetail.Null])
            : new OperationResult<TValue>(value, true, []);

    public static OperationResult<TValue> Success(TValue value) => new (value, true, []);

    public static new OperationResult<TValue> Failure(ErrorDetail error) => new (default, false, [error]);

    public static new OperationResult<TValue> Failure(IReadOnlyCollection<ErrorDetail> errors) => new (default, false, errors);
}