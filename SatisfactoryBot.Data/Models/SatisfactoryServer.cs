namespace SatisfactoryBot.Data.Models;

using System.ComponentModel.DataAnnotations;

public class SatisfactoryServer : BaseModel
{
    public string Name { get; set; }

    public ulong Owner { get; set; }

    [MaxLength(200)]
    public string Url { get; set; }

    public string Token { get; set; }

    public virtual ICollection<DiscordServer> DiscordServers { get; set; }
}
