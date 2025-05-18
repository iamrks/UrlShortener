namespace UrlShortener.Framework.Cron;

public abstract class CronJob
{
    public abstract Task ExecuteAsync(object[] args);

    public async Task WrappedExecuteAsync(object[] args)
    {
        try
        {
            await ExecuteAsync(args);
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}