namespace TravelSync.Application.Decorators.AutoRetry;

public abstract class AutoRetryBase(AutoRetryAttribute retryOptions)
{
    protected AutoRetryAttribute? RetryOptions { get; set; } = retryOptions;

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
                bool shouldRetry = IsDatabaseException(ex) || IsHttpClientException(ex);
                if (executedTimes >= this.RetryOptions?.RetryTimes || !shouldRetry) throw;
                Task.Delay(this.RetryOptions?.DelayTimes ?? 300).Wait();
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
                bool shouldRetry = IsDatabaseException(ex) || IsHttpClientException(ex);
                if (executedTimes >= this.RetryOptions?.RetryTimes || !shouldRetry) throw;
                await Task.Delay(this.RetryOptions?.DelayTimes ?? 300);
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
                bool shouldRetry = IsDatabaseException(ex) || IsHttpClientException(ex);
                if (executedTimes >= this.RetryOptions?.RetryTimes || !shouldRetry) throw;
                await Task.Delay(this.RetryOptions?.DelayTimes ?? 300);
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

    private static bool IsHttpClientException(Exception ex) => ex is HttpRequestException || ex is TimeoutException;
}
