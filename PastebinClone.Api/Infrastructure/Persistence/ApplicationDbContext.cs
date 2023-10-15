using Microsoft.EntityFrameworkCore;
using PastebinClone.Api.Application;
using PastebinClone.Api.Domain.Entities;

namespace PastebinClone.Api.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IUnitOfWork
{
    public DbSet<AliasEntity> Aliases => Set<AliasEntity>();
    public DbSet<Paste> Pastes => Set<Paste>();

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<AliasEntity>()
            .HasOne(alias => alias.Paste)
            .WithOne(paste => paste.Alias)
            .HasForeignKey<Paste>(paste => paste.AliasId)
            .IsRequired();

        modelBuilder
            .Entity<AliasEntity>()
            .HasIndex(alias => alias.Alias)
            .IsUnique();

        modelBuilder
            .Entity<AliasEntity>()
            .ToTable("Alias");

        modelBuilder
            .Entity<Paste>()
            .ToTable("Paste");
    }
}
