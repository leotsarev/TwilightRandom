using JoinRpg.Common.WebInfrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Twilight.Domain;

namespace Twilight.Dal
{
    public static class Registration
    {
        public static void AddTwilightDal(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            services.AddJoinEfCoreDbContext<TwilightDbContext>(configuration, environment, "TwilightDb");

            services.AddTransient<IGameRepository, GameRepository>();
        }
    }
}
