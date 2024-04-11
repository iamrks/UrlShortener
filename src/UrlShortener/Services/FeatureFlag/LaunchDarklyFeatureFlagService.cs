using LaunchDarkly.Sdk.Server;

namespace UrlShortener.Services.FeatureFlag
{
    public interface IFeatureFlagService
    {
        bool IsFeatureEnabled(string featureKey, bool defaultValue);
        bool IsFeatureEnabled(string featureKey, bool defaultValue, string context, string userKey);
    }

    public class LaunchDarklyFeatureFlagService : IFeatureFlagService
    {
        private const string DEFAULT_CONTEXT = "ExampleContext";
        private const string DEFAULT_NAME = "Shorten";

        private readonly LdClient _ldClient;
        private readonly ILdContextService _ldContextService;

        public LaunchDarklyFeatureFlagService(
            LdClient ldClient,
            ILdContextService ldContextService)
        {
            _ldClient = ldClient;
            _ldContextService = ldContextService;
        }

        public bool IsFeatureEnabled(string featureKey, bool defaultValue)
        {
            return IsFeatureEnabled(featureKey, defaultValue, DEFAULT_CONTEXT, DEFAULT_NAME);
        }

        public bool IsFeatureEnabled(string featureKey, bool defaultValue, string context, string userKey)
        {
            var ctx = _ldContextService.GetContext(context, userKey);

            return _ldClient.BoolVariation(featureKey, ctx, false);
        }
    }
}
