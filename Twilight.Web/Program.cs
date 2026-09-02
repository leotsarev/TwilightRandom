using JoinRpg.Common.WebInfrastructure;
using Twilight.Dal;

namespace Twilight.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseJoinSerilog("Twilight.Web");

            // Add services to the container.
            builder.Services.AddRazorPages();

            builder.Services.AddTwilightDal(builder.Configuration, builder.Environment);

            builder.Services.AddJoinWebPlatform(
                configuration: builder.Configuration,
                environment: builder.Environment,
                appName: "Twilight.Web",
                dataProtectionConnectionStringName: "DataProtection",
                telemetryServiceNames: ["Twilight.Web"]);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                app.UseForwardedHeaders();
            }

            app.UseOpenTelemetryPrometheusScrapingEndpoint();
            app.UseJoinRequestLogging();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthorization();

            app.MapRazorPages();
            app.MapJoinHealthChecks();

            app.Run();
        }
    }
}
