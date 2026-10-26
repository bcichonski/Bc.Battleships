using AwesomeAssertions;
using Bc.Battleships.Tests.Common;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Bc.Battleships.Tests.Integration.API;

public class TestDatabaseTests
{
    [Fact]
    public async Task DatabaseFileExistsWhileFactoryInUseAndIsDeletedAfterDispose()
    {
        var factory = new BattleshipsWebApplicationFactory();
        var path = factory.Database.Path;

        using (var client = factory.CreateClient())
        {
            var response = await client.PostAsync("/games", null, TestContext.Current.CancellationToken);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        File.Exists(path).Should().BeTrue();

        factory.Dispose();

        File.Exists(path).Should().BeFalse();
        File.Exists(path + "-wal").Should().BeFalse();
        File.Exists(path + "-shm").Should().BeFalse();
    }
}
