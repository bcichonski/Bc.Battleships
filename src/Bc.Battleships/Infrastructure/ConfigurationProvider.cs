using Bc.Battleships.Persistence.Providers;

namespace Bc.Battleships.Infrastructure
{
    public class ConfigurationProvider : IPersistenceConfigurationProvider
    {
        public string ConnectionString { get; }

        private readonly IConfiguration _configuration;

        public ConfigurationProvider(IConfiguration configuration)
        {
            _configuration = configuration;

            ConnectionString = GetValue(nameof(ConnectionString));
        }

        private string GetValue(string propertyName)
            => Environment.GetEnvironmentVariable(propertyName) 
            ?? _configuration.GetConnectionString(propertyName) 
            ?? throw new ArgumentNullException($"Cannot find value for configuration property {propertyName}");
        
    }
}
