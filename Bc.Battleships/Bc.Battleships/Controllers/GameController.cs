using Bc.Battleships.Server.DTOs;
using Bc.Battleships.Server.Queries;
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
            =>  await _mediator.Send(new GetCurrentGameQuery());
    }
}