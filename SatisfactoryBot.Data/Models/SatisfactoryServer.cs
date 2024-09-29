namespace SatisfactoryBot.Data.Models;

using System.ComponentModel.DataAnnotations;

public class SatisfactoryServer : BaseModel
{
    public string Name { get; set; }

    public ulong Owner { get; set; }

    [MaxLength(200)]
    public string Url { get; set; }

    public string Token { get; set; }

    public bool IsDefaultServer { get; set; }

    public int DiscordServerId { get; set; }

    public virtual DiscordServer DiscordServer { get; set; }
}
