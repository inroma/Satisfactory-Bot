namespace SatisfactoryBot.Data;

using Microsoft.EntityFrameworkCore;
using SatisfactoryBot.Data.Models;
using SatisfactoryBot.Data.Models.Relations;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext() : base() { }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        // Créer la Db si elle n'existe pas, et joue les migrations non jouées
        Database.Migrate();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<SatisfactoryServer>()
            .HasMany(a => a.DiscordServers)
            .WithMany(t => t.SatisfactoryServers)
            .UsingEntity<SatisfactoryToDiscord>();

        builder.Entity<DiscordRole>()
            .HasOne(a => a.DiscordServer)
            .WithMany(t => t.DiscordRoles)
            .HasForeignKey(a => a.Id)
            .OnDelete(DeleteBehavior.NoAction);
    }
}