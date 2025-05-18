using UrlShortener.Framework.Cron;
using static UrlShortener.Framework.Cron.ScheduleAttribute;

namespace UrlShortener.CronJobs
{
    [Schedule("*/2 * * * *", CronEnvironments.Development)]
    [Schedule("*/5 * * * *", CronEnvironments.Staging)]
    [Schedule("*/5 * * * *", CronEnvironments.Production)]
    public class ReportJob(ILogger<ReportJob> logger) : CronJob
    {
        public override async Task ExecuteAsync(object[] args)
        {
            await Task.Delay(1000);
            // Login to Prepare Report and Send over the email
            logger.LogInformation($"Report Generated and share over the email at {DateTime.Now}");
        }
    }
}
