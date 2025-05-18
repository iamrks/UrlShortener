namespace UrlShortener.Extensions
{
    public static class ConfigurationExtensions
    {
        public static bool Enabled(this IConfiguration configuration, string featureName, bool defaultValue = false)
        {
            return configuration.GetValue<bool?>($"{featureName}:Enabled") ?? defaultValue;
        }
    }
}
