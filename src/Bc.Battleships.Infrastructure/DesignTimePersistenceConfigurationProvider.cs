using Bc.Battleships.Infrastructure.Providers;

namespace Bc.Battleships.Infrastructure;

internal class DesignTimePersistenceConfigurationProvider : IPersistenceConfigurationProvider
{
    public string ConnectionString => "battleships.db";
}