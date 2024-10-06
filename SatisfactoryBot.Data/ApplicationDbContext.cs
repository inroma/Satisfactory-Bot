namespace SatisfactoryBot.Data;

using Microsoft.EntityFrameworkCore;
using SatisfactoryBot.Data.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext() : base() { }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<DiscordEntity>()
            .HasMany(a => a.SatisfactoryServers)
            .WithOne(t => t.DiscordEntity)
            .HasForeignKey(a => a.DiscordEntityId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<DiscordRole>()
            .HasOne(a => a.DiscordEntity)
            .WithMany(t => t.DiscordRoles)
            .HasForeignKey(a => a.Id)
            .OnDelete(DeleteBehavior.NoAction);
    }
}