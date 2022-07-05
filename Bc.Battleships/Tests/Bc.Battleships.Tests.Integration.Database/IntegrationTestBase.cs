using Bc.Battleships.Domain.Events;
using Bc.Battleships.Messages;
using Bc.Battleships.Persistence;
using Bc.Battleships.Persistence.Infrastructure;
using Bc.Battleships.Persistence.Providers;
using Bc.Battleships.Tests.Common;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bc.Battleships.Tests.Integration.Database
{
    public class IntegrationTestBase : IDisposable
    {
        protected IServiceProvider ServiceProvider { get; }
        private readonly IBus _busSubstitute;

        public IntegrationTestBase()
        {
            var services = new ServiceCollection();

            ConfigureServices(services);

            ServiceProvider = services.BuildServiceProvider();

            _busSubstitute = Substitute.For<IBus>();
            DomainEventBus.GetBus = () => _busSubstitute;

            InitializeDatabase().Wait();
        }

        private Task InitializeDatabase()
            => InScope(async dbc =>
              {
                  await dbc!.Database.EnsureDeletedAsync();
                  await dbc!.Database.EnsureCreatedAsync();
              });

        public virtual void ConfigureServices(ServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IPersistenceConfigurationProvider, TestConfigurationProvider>();
            
            Services.Configure(serviceCollection);
        }

        public async Task InScope(Func<BattleshipsDbContext, Task> action)
        {
            using var scope = ServiceProvider.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetService<BattleshipsDbContext>();

            await action(dbContext!);
        }

        public void InScope(Action<BattleshipsDbContext> action)
        {
            using var scope = ServiceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetService<BattleshipsDbContext>();

            action(dbContext!);
        }

        public void AssertEventWasPublished<T>(Expression<Predicate<T>> assertExpression) where T:IEvent
        {
            _busSubstitute.Received().Publish(Arg.Is<T>(assertExpression));
        }

        public void AssertEventWasNotPublished<T>() where T : IEvent
        {
            _busSubstitute.Received().Publish(Arg.Any<T>());
        }

        public void Dispose()
        {
            InScope(dbc => dbc.Database.EnsureDeleted());
            GC.SuppressFinalize(this);
        }
    }
}