using Scalar.AspNetCore;

namespace Bc.Battleships.Infrastructure;

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
            // Not in Development: the Vite dev proxy forwards to the HTTP URL, and a redirect to HTTPS
            // would send the browser to another origin.
            app.UseHttpsRedirection();
        }

        app.UseStaticFiles();
        app.UseRouting();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }

        app.MapGameEndpoints();
        // Mapped before the SPA fallback so the probe never resolves to index.html.
        app.MapHealthChecks("/healthz");

        app.MapFallbackToFile("index.html");
    }
}
