using Bastilia.Rating.Migrator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Twilight.Dal;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Services.AddTwilightDal(builder.Configuration);
builder.Services.AddHostedService<MigrationsLauncher>();
builder.Services.AddScoped<IMigratorService, MigrateEfCoreHostService<TwilightDbContext>>();

builder.Build().Run();
