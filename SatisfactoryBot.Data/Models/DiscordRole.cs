namespace SatisfactoryBot.Data.Models;

public class DiscordRole : BaseModel
{
    public bool IsAdmin { get; set; }
    
    public virtual DiscordEntity DiscordEntity { get; set; }
}
