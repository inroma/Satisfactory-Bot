namespace SatisfactoryBot.Data.Models;

public class SatisfactoryServer : BaseModel
{
    public string Name { get; set; }

    public ulong Owner { get; set; }

    public string Token { get; set; }

    public virtual ICollection<DiscordServer> DiscordServers { get; set; }
}
