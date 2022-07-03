using Bc.Battleships.Domain.Events;
using Bc.Battleships.Persistence;
using PersistenceServices = Bc.Battleships.Persistence.Infrastructure.Services;
using HandlersServices = Bc.Battleships.Server.Infrastructure.Services;
using Bc.Battleships.Persistence.Providers;
using Bc.Battleships.Tests.Common;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;
using Bc.Battleships.Messages;

namespace Bc.Battleships.Tests.Integration.API
{
    public class ApiIntegrationTestBase
    {
        protected IServiceProvider ServiceProvider { get; }
        private IBus _busSubstitute { get; }

        public ApiIntegrationTestBase()
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

        public virtual void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IPersistenceConfigurationProvider, TestConfigurationProvider>();

            PersistenceServices.Configure(serviceCollection);
            HandlersServices.Configure(serviceCollection);
        }

        public async Task InScope(Func<BattleshipsDbContext, Task> action)
        {
            using (var scope = ServiceProvider.CreateAsyncScope())
            {
                var dbContext = scope.ServiceProvider.GetService<BattleshipsDbContext>();

                await action(dbContext!);
            }
        }

        public void InScope(Action<BattleshipsDbContext> action)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<BattleshipsDbContext>();

                action(dbContext!);
            }
        }

        public void AssertDomainEventWasPublished<T>(Expression<Predicate<T>> assertExpression) where T : IDomainEvent
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