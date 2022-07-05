using Bc.Battleships.Domain.Events;
using Bc.Battleships.Messages;
using Bc.Battleships.Messages.Commands;
using Bc.Battleships.Messages.CommandsResults;
using Bc.Battleships.Messages.DTOs;
using Bc.Battleships.Messages.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bc.Battleships.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {
        private readonly ILogger<GameController> _logger;
        private readonly IMediator _mediator;

        public GameController(ILogger<GameController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<GameDto> Get()
            => await _mediator.Send(new GetCurrentGameQuery());

        [HttpPut]
        public async Task<ShootResult> Shoot(ShootCommand shootCommand)
            => await _mediator.Send(shootCommand);

        [HttpPost]
        public async Task CreateNewGame()
            => await _mediator.Send(new CreateNewGameCommand());
    }
}