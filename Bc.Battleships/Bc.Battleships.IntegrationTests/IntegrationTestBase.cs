using Bc.Battleships.Domain.Events;
using Bc.Battleships.Persistence;
using Bc.Battleships.Persistence.Infrastructure;
using Bc.Battleships.Persistence.Providers;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bc.Battleships.IntegrationTests
{
    public class IntegrationTestBase : IDisposable
    {
        protected ServiceProvider ServiceProvider { get; }
        private IBus _busSubstitute { get; }

        public IntegrationTestBase()
        {
            var services = new ServiceCollection();

            ConfigureServices(services);

            ServiceProvider = services.BuildServiceProvider();
            _busSubstitute = Substitute.For<IBus>();

            DomainEventBus.Use(_busSubstitute);

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
            using (ServiceProvider.CreateAsyncScope())
            {
                var dbContext = ServiceProvider.GetService<BattleshipsDbContext>();
                
                await action(dbContext!);
            }
        }

        public void InScope(Action<BattleshipsDbContext> action)
        {
            using (ServiceProvider.CreateScope())
            {
                var dbContext = ServiceProvider.GetService<BattleshipsDbContext>();

                action(dbContext!);
            }
        }

        public void AssertDomainEventWasPublished<T>(Expression<Predicate<T>> assertExpression) where T:IDomainEvent
        {
            _busSubstitute.Received().Publish(Arg.Is<T>(assertExpression));
        }

        public void AssertDomainEventWasNotPublished<T>() where T : IDomainEvent
        {
            _busSubstitute.Received().Publish(Arg.Any<T>());
        }

        public void Dispose()
            => InScope(dbc => dbc.Database.EnsureDeleted());
    }
}