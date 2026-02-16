using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Twilight.Domain;

namespace Twilight.Dal
{
    public static class Registration
    {
        public static void AddTwilightDal(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DbContext>(
    options => options.UseNpgsql(configuration.GetConnectionString("TwilightDb")));

            services.AddTransient<IGameRepository, GameRepository>();
        }
    }
}
