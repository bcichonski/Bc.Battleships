using AwesomeAssertions;
using Bc.Battleships.Tests.Common;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using static Bc.Battleships.Tests.Integration.API.GameApi;

namespace Bc.Battleships.Tests.Integration.API;

public class HealthCheckTests
{
    [Fact]
    public async Task GetHealthz_Returns200WithHealthyBody()
    {
        using var factory = new BattleshipsWebApplicationFactory();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/healthz", Ct);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadAsStringAsync(Ct)).Should().Be("Healthy");
    }
}
