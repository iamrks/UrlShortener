namespace UrlShortener.Framework.Cron;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class ScheduleAttribute : Attribute
{
    public string CronExpression { get; set; }
    public CronEnvironments Environment { get; set; }
    public object[] Arguments { get; set; }

    public ScheduleAttribute(string cronExpression, CronEnvironments environment, params object[] args)
    {
        CronExpression = cronExpression;
        Environment = environment;
        Arguments = args;
    }

    public enum CronEnvironments
    {
        Development = 1,
        Staging = 2,
        Production = 3,
    }
}
