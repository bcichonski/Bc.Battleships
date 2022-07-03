using FluentAssertions;
using Bc.Battleships.Domain.AggregateRoots.Games;
using Bc.Battleships.Domain.ValueObjects;
using Bc.Battleships.Messages.Commands;
using Bc.Battleships.Tests.Common.Builders;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Bc.Battleships.Tests.Integration.API.Games
{
    public class WhenShooting : ApiIntegrationTestBase
    {
        [Fact]
        public async Task ShouldReturnHit()
        {
            var game = await AssumeGameInDb();
            var currentHitPoints = game.ComputerPlayer.CurrentHitPoints.Value;
            var expectedCoordinates = new Coordinates(2, 1);
            var shootCommand = new ShootCommand(expectedCoordinates.Column, expectedCoordinates.Row);

            await InScope(async testContext =>
            {
                var result = await testContext.Mediator.Send(shootCommand, CancellationToken.None);

                result.Hit.Should().BeTrue();
                result.CurrentHitPoints.Should().Be(currentHitPoints - 1);
            });
        }

        [Fact]
        public async Task ShouldReturnMiss()
        {
            var game = await AssumeGameInDb();
            var currentHitPoints = game.ComputerPlayer.CurrentHitPoints.Value;
            var expectedCoordinates = new Coordinates(3, 3);
            var shootCommand = new ShootCommand(expectedCoordinates.Column, expectedCoordinates.Row);

            await InScope(async testContext =>
            {
                var result = await testContext.Mediator.Send(shootCommand, CancellationToken.None);

                result.Hit.Should().BeFalse();
                result.CurrentHitPoints.Should().Be(currentHitPoints);
            });
        }
    }
}
