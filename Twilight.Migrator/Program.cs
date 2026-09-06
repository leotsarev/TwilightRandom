using JoinRpg.Common.WebInfrastructure.DataProtection;
using JoinRpg.Common.WebInfrastructure.EfCoreMigration;
using Microsoft.Extensions.Hosting;
using Twilight.Dal;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Services.AddMigrationsLauncher();
builder.Services.RegisterMigrator<TwilightDbContext>(builder.Configuration, builder.Environment, "TwilightDb");
builder.Services.RegisterMigrator<DataProtectionDbContext>(builder.Configuration, builder.Environment, "TwilightDb");

builder.Build().Run();
