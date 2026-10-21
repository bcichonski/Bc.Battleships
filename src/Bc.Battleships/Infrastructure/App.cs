using Scalar.AspNetCore;

namespace Bc.Battleships.Infrastructure
{
    public static class App
    {
        public static async Task Initialize(WebApplication app)
        {
            await Services.InitializeAsync(app.Services);

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.MapGameEndpoints();

            app.MapFallbackToFile("index.html");
        }
    }
}
