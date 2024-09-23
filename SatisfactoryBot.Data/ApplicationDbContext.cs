namespace SatisfactoryBot.Data;

using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext() : base() { }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        // Créer la Db si elle n'existe pas, et joue les migrations non jouées
        Database.Migrate();
    }

}