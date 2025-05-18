using Hangfire;
using Hangfire.Storage;
using System.Linq.Expressions;
using System.Reflection;
using static UrlShortener.Framework.Cron.ScheduleAttribute;

namespace UrlShortener.Framework.Cron;

public class CronJobManager
{
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly ILogger<CronJobManager> _logger;

    public CronJobManager(IWebHostEnvironment hostingEnvironment, ILogger<CronJobManager> logger)
    {
        _hostingEnvironment = hostingEnvironment;
        _logger = logger;
    }

    public void RegisterJobs()
    {
        var registeredJobIds = new List<string>();

        foreach (var cronJobType in FindCronJobs())
        {
            var schedules = FindSchedules(cronJobType);
            foreach (var schedule in schedules)
            {
                RegisterJob(cronJobType, schedule, registeredJobIds);
            }
        }

        CleanupRemovedJobs(registeredJobIds);
    }

    private IEnumerable<Type> FindCronJobs()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var types = assembly.GetTypes()
            .Where(t => !t.IsAbstract && typeof(CronJob).IsAssignableFrom(t));
        return types;
    }

    private IEnumerable<ScheduleAttribute> FindSchedules(Type type)
    {
        //return type.GetCustomAttributes(typeof(ScheduleAttribute), true).Cast<ScheduleAttribute>();

        return type.GetCustomAttributes(typeof(ScheduleAttribute), true)
                .Select(attribute => attribute as ScheduleAttribute)
                .Where(IsForCurrentEnvironment);
    }

    private bool IsForCurrentEnvironment(ScheduleAttribute? schedule)
    {
        if (schedule == null)
        {
            return false;
        }

        return _hostingEnvironment.EnvironmentName switch
        {
            "" or "Development" => (schedule.Environment & CronEnvironments.Development) != 0,
            "Staging" => (schedule.Environment & CronEnvironments.Staging) != 0,
            "Production" => (schedule.Environment & CronEnvironments.Production) != 0,
            _ => throw new Exception($"Unknown environment \"{_hostingEnvironment.EnvironmentName}\"."),
        };
    }

    public void RegisterJob(Type type, ScheduleAttribute schedule, List<string> registeredJobIds)
    {
        var jobId = $"{type.Name}:{schedule.CronExpression}";

        var addOrUpdateMethod = GetAddOrUpdateMethod(type);
        var runJobLambda = BuildRunJobLambda(type, schedule.Arguments);
        var invokeArguments = new[] { jobId, runJobLambda, schedule.CronExpression, Type.Missing, Type.Missing };
        addOrUpdateMethod.Invoke(null, invokeArguments);
        registeredJobIds.Add(jobId);
    }

    private MethodInfo GetAddOrUpdateMethod(Type cronJobType)
    {
        return typeof(RecurringJob).GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(m => m.Name == "AddOrUpdate" && m.GetGenericArguments().Length == 1
                     && m.GetParameters().Select(p => p.ParameterType).SequenceEqual(new Type[]
                        {
                            typeof(string),
                            typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(m.GetGenericArguments().First(), typeof(Task))),
                            typeof(string),
                            typeof(TimeZoneInfo),
                            typeof(string)
                        }))
            .Single()
            .MakeGenericMethod(cronJobType);
    }

    private Expression BuildRunJobLambda(Type cronJobType, object[] args)
    {
        var job = Expression.Parameter(cronJobType, "job");
        var method = cronJobType.GetMethod(nameof(CronJob.WrappedExecuteAsync));
        var call = Expression.Call(job, method, Expression.Constant(args));
        return Expression.Lambda(typeof(Func<,>).MakeGenericType(cronJobType, typeof(Task)), call, job);
    }

    private void CleanupRemovedJobs(IEnumerable<string> registeredJobIds)
    {
        using var connection = JobStorage.Current.GetConnection();
        foreach (var job in connection.GetRecurringJobs())
        {
            if (!registeredJobIds.Contains(job.Id))
            {
                RecurringJob.RemoveIfExists(job.Id);
                _logger.LogWarning($"Removed cron job: {job.Id}.");
            }
        }
    }
}