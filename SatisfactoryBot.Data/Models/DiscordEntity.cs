namespace SatisfactoryBot.Data.Models;

using System.Text.Json.Serialization;

public class DiscordEntity : BaseModel
{
    public ulong EntityId { get; set; }

    [JsonIgnore]
    public virtual ICollection<SatisfactoryServer> SatisfactoryServers { get; set; }

    [JsonIgnore]
    public virtual ICollection<DiscordRole> DiscordRoles { get; set; }
}
