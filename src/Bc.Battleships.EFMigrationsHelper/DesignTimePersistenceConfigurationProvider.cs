using Bc.Battleships.Persistence.Providers;

namespace Bc.Battleships.EFMigrationsHelper
{
    internal class DesignTimePersistenceConfigurationProvider : IPersistenceConfigurationProvider
    {
        public string ConnectionString => "battleships.db";
    }
}