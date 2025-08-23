using Microsoft.Extensions.Configuration;

namespace DriverApp
{
    public static class ApiConfig
    {
        public static ApiEnvironment CurrentEnvironment { get; private set; } = ApiEnvironment.Production;

            public static string BaseUrl =>
            CurrentEnvironment switch
            {
                ApiEnvironment.Development => "https://app.hadmservices.com/api/",
                ApiEnvironment.Staging => "https://app.hadmservices.com/api/",
                ApiEnvironment.Production => "https://app.hadmservices.com/api/",
                _ => throw new ArgumentOutOfRangeException(nameof(CurrentEnvironment), $"Unhandled environment: {CurrentEnvironment}")
            };


        public static void SetEnvironment(ApiEnvironment environment)
        {
            CurrentEnvironment = environment;
            //SetEnvironment(ApiEnvironment.Development); // for testing
            
        }
        
    }
}