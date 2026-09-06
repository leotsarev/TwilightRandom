using JoinRpg.Common.WebInfrastructure.DataProtection;
using JoinRpg.Common.WebInfrastructure.EfCoreMigration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Twilight.Dal;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

// DataProtection uses the same physical database as TwilightDb, just under its own
// connection-string name, so its health check doesn't collide with TwilightDb's.
builder.Configuration["ConnectionStrings:DataProtection"] = builder.Configuration.GetConnectionString("TwilightDb");

builder.Services.AddMigrationsLauncher();
builder.Services.RegisterMigrator<TwilightDbContext>(builder.Configuration, builder.Environment, "TwilightDb");
builder.Services.RegisterMigrator<DataProtectionDbContext>(builder.Configuration, builder.Environment, "DataProtection");

builder.Build().Run();
