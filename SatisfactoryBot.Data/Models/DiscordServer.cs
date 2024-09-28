namespace SatisfactoryBot.Data.Models;

public class DiscordServer : BaseModel
{
    public ulong GuildId { get; set; }

    public virtual ICollection<SatisfactoryServer> SatisfactoryServers { get; set; }

    public virtual ICollection<DiscordRole> DiscordRoles { get; set; }
}
