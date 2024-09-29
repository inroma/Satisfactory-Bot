namespace SatisfactoryBot.Data.Models;

using System.Text.Json.Serialization;

public class DiscordServer : BaseModel
{
    public ulong GuildId { get; set; }

    [JsonIgnore]
    public virtual ICollection<SatisfactoryServer> SatisfactoryServers { get; set; }

    [JsonIgnore]
    public virtual ICollection<DiscordRole> DiscordRoles { get; set; }
}
