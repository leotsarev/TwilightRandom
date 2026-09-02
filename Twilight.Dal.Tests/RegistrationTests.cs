using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Twilight.Dal.Tests;

public class RegistrationTests
{
    [Fact]
    public void AddTwilightDal_RegistersTwilightDbContext()
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:TwilightDb"] = "Host=localhost;Database=twilight",
            })
            .Build();

        ServiceCollection services = new();
        services.AddTwilightDal(configuration);

        using ServiceProvider provider = services.BuildServiceProvider();

        TwilightDbContext dbContext = provider.GetRequiredService<TwilightDbContext>();

        Assert.NotNull(dbContext);
    }
}
