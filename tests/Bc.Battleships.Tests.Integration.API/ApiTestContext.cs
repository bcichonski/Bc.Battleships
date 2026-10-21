using Bc.Battleships.Domain.Repositories;
using Bc.Battleships.Messages;
using Bc.Battleships.Persistence;
using Mediator;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Bc.Battleships.Tests.Integration.API
{
    public class ApiTestContext : IDisposable
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly Lazy<BattleshipsDbContext> _dbContext;
        public BattleshipsDbContext DbContext => _dbContext.Value;

        private readonly Lazy<IMediator> _mediator;
        public IMediator Mediator => _mediator.Value;

        private readonly Lazy<IGamesRepository> _gamesRepository;
        public IGamesRepository GamesRepository => _gamesRepository.Value;

        private readonly Lazy<IBus> _bus;
        public IBus Bus => _bus.Value;

        public ApiTestContext(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            _dbContext = new Lazy<BattleshipsDbContext>(() => _serviceProvider.GetRequiredService<BattleshipsDbContext>());
            _mediator = new Lazy<IMediator>(() => _serviceProvider.GetRequiredService<IMediator>());
            _gamesRepository = new Lazy<IGamesRepository>(() => _serviceProvider.GetRequiredService<IGamesRepository>());
            _bus = new Lazy<IBus>(() => _serviceProvider.GetRequiredService<IBus>());
        }

        public void Dispose()
        {
            DbContext.SaveChanges();

            DbContext.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}
