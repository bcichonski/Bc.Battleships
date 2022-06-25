using Bc.Battleships.Persistence.Providers;
using System.IO;

namespace Bc.Battleships.IntegrationTests
{
    internal class TestConfigurationProvider : IPersistenceConfigurationProvider
    {
        public string ConnectionString => $"Data Source = battleships.tests.{Path.GetRandomFileName()}.db";
    }
}
