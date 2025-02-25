using TravelSync.Application.Abstractions.Dispatching;

namespace TravelSync.Application.Decorators.AutoRetry;

/// <summary>
/// Attribute để tự động retry khi phương thức gặp lỗi.
/// </summary>
/// <remarks>
/// Attribute này có thể áp dụng cho các phương thức để tự động retry khi có exception xảy ra,
/// dùng cho các thao tác liên quan đến database hoặc HTTP request.
/// </remarks>
/// <param name="retryTimes">Số lần retry tối đa trước khi throw exception. Mặc định là 3.</param>
/// <param name="delayTimes">Thời gian trễ (milliseconds) giữa các lần retry. Mặc định là 300 ms).</param>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
public sealed class AutoRetryAttribute(int retryTimes = 3, int delayTimes = 300) : DecoratorAttribute
{
    public int RetryTimes { get; } = retryTimes;
    public int DelayTimes { get; } = delayTimes;

    public override Type GetDecoratorType(Type handlerType)
    {
        if (handlerType.GetGenericTypeDefinition() == typeof(ICommandHandler<>))
            return typeof(AutoRetryCommanDecorator<>);

        if (handlerType.GetGenericTypeDefinition() == typeof(ICommandHandler<,>))
            return typeof(AutoRetryCommanDecorator<,>);

        if (handlerType.GetGenericTypeDefinition() == typeof(IQueryHandler<,>))
            return typeof(AutoRetryQueryDecorator<,>);

        throw new InvalidOperationException($"Handler type {handlerType.Name} is not supported by {this.GetType().Name}.");
    }
}
