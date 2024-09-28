namespace SatisfactoryBot.Data.Models;

public class DiscordServer : BaseModel
{
    public virtual ICollection<SatisfactoryServer> SatisfactoryServers { get; set; }

    public virtual ICollection<DiscordRole> DiscordRoles { get; set; }
}
