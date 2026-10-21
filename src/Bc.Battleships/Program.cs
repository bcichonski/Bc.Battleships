using Bc.Battleships.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

Services.Configure(builder.Services);

var app = builder.Build();

await App.Initialize(app);

app.Run();
