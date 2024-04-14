using LaunchDarkly.Sdk;

namespace UrlShortener.Services.FeatureFlag;

public interface ILdContextService
{
    Context GetContext(string contextKey, string name);
}

public class LdContextService : ILdContextService
{
    public Context GetContext(string contextKey, string name)
    {
        return Context.Builder(contextKey).Name(name).Build();
    }
}
