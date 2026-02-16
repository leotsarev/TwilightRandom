using Microsoft.EntityFrameworkCore;
using Twilight.Domain;

namespace Twilight.Dal;

public class TwilightDbContext(DbContextOptions<TwilightDbContext> options) : Microsoft.EntityFrameworkCore.DbContext(options)
{
    public DbSet<Game> Games { get; set; }
    public DbSet<Player> Players { get; set; }

    public DbSet<Faction> Factions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasPostgresEnum<PlayerColor>();

        modelBuilder.Entity<PlayerSlot>().HasMany(ps => ps.PossibleFactions).WithMany();

        modelBuilder.Entity<Faction>().HasData(DefaultData.Factions);
    }
}
