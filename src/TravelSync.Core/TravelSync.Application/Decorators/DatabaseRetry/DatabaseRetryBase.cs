namespace TravelSync.Application.Decorators.DatabaseRetry;

public abstract class DatabaseRetryBase(DatabaseRetryAttribute retryOptions)
{
    protected DatabaseRetryAttribute? RetryOptions { get; set; } = retryOptions;

    protected void WrapExecution(Action action)
    {
        int executedTimes = 0;
        ArgumentNullException.ThrowIfNull(action);

        while (true)
        {
            try
            {
                executedTimes++;
                action();
                return;
            }
            catch (Exception ex)
            {
                if (executedTimes >= this.RetryOptions?.RetryTimes || !IsDatabaseException(ex)) throw;
            }
        }
    }

    protected async Task WrapExecutionAsync(Func<Task> action)
    {
        int executedTimes = 0;
        ArgumentNullException.ThrowIfNull(action);

        while (true)
        {
            try
            {
                executedTimes++;
                await action();
                return;
            }
            catch (Exception ex)
            {
                if (executedTimes >= this.RetryOptions?.RetryTimes || !IsDatabaseException(ex)) throw;
            }
        }
    }

    protected async Task<TResult> WrapExecutionAsync<TResult>(Func<Task<TResult>> action)
    {
        int executedTimes = 0;
        ArgumentNullException.ThrowIfNull(action);

        while (true)
        {
            try
            {
                executedTimes++;
                return await action();
            }
            catch (Exception ex)
            {
                if (executedTimes >= this.RetryOptions?.RetryTimes || !IsDatabaseException(ex)) throw;
                await Task.Delay(this.RetryOptions?.DelayTimes ?? 0);
            }
        }
    }

    private static bool IsDatabaseException(Exception exception)
    {
        string message = exception?.InnerException?.Message ?? string.Empty;

        if (string.IsNullOrEmpty(message)) return false;

        return message.Contains("The connection is broken and recovery is not possible", StringComparison.OrdinalIgnoreCase)
            || message.Contains("error occurred while establishing a connection", StringComparison.OrdinalIgnoreCase);
    }
}
