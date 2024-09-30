namespace SatisfactoryBot.Data;

using Microsoft.EntityFrameworkCore;
using SatisfactoryBot.Data.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext() : base() { }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        // Créer la Db si elle n'existe pas
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<DiscordServer>()
            .HasMany(a => a.SatisfactoryServers)
            .WithOne(t => t.DiscordServer)
            .HasForeignKey(a => a.DiscordServerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<DiscordRole>()
            .HasOne(a => a.DiscordServer)
            .WithMany(t => t.DiscordRoles)
            .HasForeignKey(a => a.Id)
            .OnDelete(DeleteBehavior.NoAction);
    }
}