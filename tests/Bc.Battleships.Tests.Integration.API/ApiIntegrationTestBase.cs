using Bc.Battleships.Domain.AggregateRoots.Games;
using Bc.Battleships.Persistence.Providers;
using Bc.Battleships.Tests.Common;
using Bc.Battleships.Tests.Common.Builders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using HandlersServices = Bc.Battleships.Server.Infrastructure.Services;
using PersistenceServices = Bc.Battleships.Persistence.Infrastructure.Services;

namespace Bc.Battleships.Tests.Integration.API
{
    public class ApiIntegrationTestBase
    {
        private readonly IServiceProvider _serviceProvider;

        public ApiIntegrationTestBase()
        {
            var services = new ServiceCollection();

            ConfigureServices(services);

            _serviceProvider = services.BuildServiceProvider();

            InitializeDatabase().Wait();
        }

        private Task InitializeDatabase()
            => InScope(async testContext =>
            {
                await testContext.DbContext.Database.EnsureDeletedAsync();
                await testContext.DbContext.Database.EnsureCreatedAsync();
            });

        public virtual void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IPersistenceConfigurationProvider, TestConfigurationProvider>();

            PersistenceServices.Configure(serviceCollection);
            HandlersServices.Configure(serviceCollection);
        }

        public async Task InScope(Func<ApiTestContext, Task> action)
        {
            using var scope = _serviceProvider.CreateAsyncScope();
            using var testContext = new ApiTestContext(scope.ServiceProvider);

            await action(testContext);
        }

        public void InScope(Action<ApiTestContext> action)
        {
            using var scope = _serviceProvider.CreateScope();
            using var testContext = new ApiTestContext(scope.ServiceProvider);

            action(testContext);
        }

        protected async Task<Game> AssumeGameInDb()
        {
            Game? game = null;
            await InScope(async testContext =>
            {
                await testContext.DbContext.AddAsync(game = new GameBuilder()
                    .Build());

                await testContext.DbContext.SaveChangesAsync();
            });
            return game!;
        }

        public void Dispose()
            => InScope(dbc => dbc.DbContext.Database.EnsureDeleted());
    }
}