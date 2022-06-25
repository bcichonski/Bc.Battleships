using Bc.Battleships.Persistence;
using Bc.Battleships.Persistence.Infrastructure;
using Bc.Battleships.Persistence.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Bc.Battleships.IntegrationTests
{
    public class IntegrationTestBase : IAsyncDisposable
    {
        protected ServiceProvider ServiceProvider { get; }

        public IntegrationTestBase()
        {
            var services = new ServiceCollection();

            ConfigureServices(services);

            ServiceProvider = services.BuildServiceProvider();

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
                var dbContextFactory = ServiceProvider.GetService<IDbContextFactory<BattleshipsDbContext>>();
                using(var dbContext = dbContextFactory!.CreateDbContext())
                {
                    await action(dbContext);
                }
            }
        } 

        public async ValueTask DisposeAsync()
            => await InScope(dbc => dbc.Database.EnsureDeletedAsync());
    }
}