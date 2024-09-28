namespace SatisfactoryBot.Data.Models;

public class DiscordRole : BaseModel
{
    public bool IsAdmin { get; set; }
    
    public virtual DiscordServer DiscordServer { get; set; }
}
