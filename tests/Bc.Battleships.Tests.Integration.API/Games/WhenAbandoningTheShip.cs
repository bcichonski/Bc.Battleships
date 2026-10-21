using AwesomeAssertions;
using Bc.Battleships.Domain.Events.Ships;
using Bc.Battleships.Domain.ValueObjects.Enums;
using Bc.Battleships.Tests.Common.Builders;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Bc.Battleships.Tests.Integration.API.Games
{
    public class WhenAbandoningTheShip : ApiIntegrationTestBase
    {
        [Fact]
        public async Task ShouldStopGameWhenComputerPlayerHasNoHitPointsLeft()
        {
            var ship = new ShipBuilder()
                .Build();
            var computerPlayer = new ComputerPlayerBuilder()
                .WithFleet([ship])
                .WithSetOfCoordinatesShot(ship.Compartments.Select(x => x.Coordinates).ToHashSet())
                .Build();
            var game = new GameBuilder()
                .WithComputerPlayer(computerPlayer)
                .Build();

            await InScope(async testContext =>
            {
                await testContext.DbContext.AddAsync(game);
                await testContext.DbContext.SaveChangesAsync();
            });

            // The real IBus from the container: Bus -> generated Mediator -> AbandonTheShipDomainEventHandler (Domain assembly).
            await InScope(async testContext =>
            {
                await testContext.Bus.Publish(new AbandonTheShipDomainEvent(game.Id));
            });

            await InScope(async testContext =>
            {
                var storedGame = await testContext.GamesRepository.GetById(game.Id, CancellationToken.None);

                storedGame.Should().NotBeNull();
                storedGame!.ComputerPlayer.CurrentHitPoints.Value.Should().Be(0);
                storedGame.GameState.Should().Be(GameState.Ended);
            });
        }
    }
}
