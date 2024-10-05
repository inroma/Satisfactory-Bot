namespace SatisfactoryBot.Models.Settings;

public class DiscordSettings
{
    public string Token { get; set; }

    public bool RegisterCommandsGlobally { get; set; }

    public ulong? DevServerId { get; set; }

    /// <summary>
    /// Max file size in bytes
    /// </summary>
    public uint MaxFileSize { get; set; }
}
