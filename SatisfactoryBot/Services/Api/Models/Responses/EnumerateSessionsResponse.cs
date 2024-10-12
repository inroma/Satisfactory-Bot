namespace SatisfactoryBot.Services.Api.Models.Responses;

using System.Text.Json.Serialization;

public class EnumerateSessionsResponse
{
    [JsonPropertyName("Sessions")]
    public List<SessionSaveStruct> Sessions { get; set; }

    [JsonPropertyName("CurrentSessionIndex")]
    public int CurrentSessionIndex { get; set; }
}

public class SessionSaveStruct
{
    [JsonPropertyName("SessionName")]
    public string SessionName { get; set; }

    [JsonPropertyName("SaveHeaders")]
    public List<SaveHeader> SaveHeaders { get; set; }
}

public class SaveHeader
{
    [JsonPropertyName("SaveVersion")]
    public int SaveVersion { get; set; }

    [JsonPropertyName("BuildVersion")]
    public int BuildVersion { get; set; }

    [JsonPropertyName("SaveName")]
    public string SaveName { get; set; }

    [JsonPropertyName("MapName")]
    public string MapName { get; set; }

    [JsonPropertyName("MapOptions")]
    public string MapOptions { get; set; }

    [JsonPropertyName("SessionName")]
    public string SessionName { get; set; }

    [JsonPropertyName("PlayDurationSeconds")]
    public int PlayDurationSeconds { get; set; }

    [JsonPropertyName("SaveDateTime")]
    public string SaveDateTime { get; set; }

    [JsonPropertyName("IsModdedSave")]
    public bool IsModdedSave { get; set; }

    [JsonPropertyName("IsEditedSave")]
    public bool IsEditedSave { get; set; }

    [JsonPropertyName("IsCreativeModeEnabled")]
    public bool IsCreativeModeEnabled { get; set; }
}
