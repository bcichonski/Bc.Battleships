using Bc.Battleships.Messages;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Bc.Battleships.Server.Infrastructure
{
    public static class Services
    {
        public static void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddMediatR(typeof(Services).Assembly);

            serviceCollection.AddSingleton<IBus, Bus>();
        }
    }
}
