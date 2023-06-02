using Microsoft.EntityFrameworkCore;

namespace Twilight.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddDbContext<DbContext>(
                options => options.UseNpgsql(builder.Configuration.GetConnectionString("TwilightDb")));

            builder.Services.AddTransient<GameRepository>();
            builder.Services.AddHealthChecks();

            var app = builder.Build();

            using (var Scope = app.Services.CreateScope())
            {
                var context = Scope.ServiceProvider.GetRequiredService<DbContext>();
                context.Database.Migrate();
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.MapHealthChecks("/health/live");
            app.UseAuthorization();

            
            app.MapRazorPages();

            app.Run();
        }
    }
}