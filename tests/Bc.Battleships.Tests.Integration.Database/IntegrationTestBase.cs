using Bc.Battleships.Infrastructure;
using Bc.Battleships.Infrastructure.Infrastructure;
using Bc.Battleships.Infrastructure.Providers;
using Bc.Battleships.Tests.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Bc.Battleships.Tests.Integration.Database;

public class IntegrationTestBase : IDisposable
{
    private readonly TestDatabase _database = new("db");

    protected IServiceProvider ServiceProvider { get; }

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
              await dbc.Database.MigrateAsync();
          });

    public virtual void ConfigureServices(ServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IPersistenceConfigurationProvider>(new FixedConfigurationProvider(_database.ConnectionString));
        
        Bc.Battleships.Infrastructure.Infrastructure.Services.Configure(serviceCollection);
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

    public void Dispose()
    {
        (ServiceProvider as IDisposable)?.Dispose();
        _database.Dispose();
        GC.SuppressFinalize(this);
    }

    private sealed class FixedConfigurationProvider(string connectionString) : IPersistenceConfigurationProvider
    {
        public string ConnectionString { get; } = connectionString;
    }
}
