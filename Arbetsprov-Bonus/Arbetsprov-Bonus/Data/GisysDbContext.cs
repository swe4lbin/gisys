using Arbetsprov_Bonus.Entities;
using Microsoft.EntityFrameworkCore;

namespace Arbetsprov_Bonus.Data;

public class GisysDbContext : DbContext
{
    public GisysDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Starting consultants
        modelBuilder.Entity<Consultant>().HasKey(c => c.Id);

        modelBuilder.Entity<Consultant>().Property(c => c.Id).ValueGeneratedOnAdd();

        // Starting bonuses
        modelBuilder.Entity<BonusForConsultant>().HasKey(c => c.Id);

        modelBuilder.Entity<BonusForConsultant>().Property(c => c.Id).ValueGeneratedOnAdd();

        // Starting bonus periods
        modelBuilder.Entity<BonusPeriod>().HasKey(c => c.Id);

        modelBuilder.Entity<BonusPeriod>().Property(c => c.Id).ValueGeneratedOnAdd();
    }

    public DbSet<Consultant> Consultants => Set<Consultant>();
    public DbSet<BonusForConsultant> BonusesForConsultants => Set<BonusForConsultant>();
    public DbSet<BonusPeriod> BonusPeriods => Set<BonusPeriod>();
}
