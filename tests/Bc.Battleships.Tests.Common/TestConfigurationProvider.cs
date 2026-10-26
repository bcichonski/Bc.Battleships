using Bc.Battleships.Infrastructure.Providers;

namespace Bc.Battleships.Tests.Common;

public class TestConfigurationProvider : IPersistenceConfigurationProvider
{
    public string ConnectionString => $"Data Source = battleships.tests.{Path.GetRandomFileName()}.db";
}
