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
                app.UseSwagger();
            }

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action=Index}/{id?}");

            app.MapFallbackToFile("index.html");

            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger", "v1");
                    options.RoutePrefix = "";
                });
            }
        }
    }
}
