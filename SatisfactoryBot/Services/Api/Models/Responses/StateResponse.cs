namespace SatisfactoryBot.Services.Api.Models.Responses;

using System.Text.Json.Serialization;

public class StateResponse
{
    [JsonPropertyName("serverGameState")]
    public ServerGameState ServerGameState { get; set; }
}

public class ServerGameState
{
    [JsonPropertyName("activeSessionName")]
    public string ActiveSessionName { get; set; }

    [JsonPropertyName("numConnectedPlayers")]
    public int NumConnectedPlayers { get; set; }

    [JsonPropertyName("playerLimit")]
    public int PlayerLimit { get; set; }

    [JsonPropertyName("techTier")]
    public int TechTier { get; set; }

    [JsonPropertyName("activeSchematic")]
    public string ActiveSchematic { get; set; }

    [JsonPropertyName("gamePhase")]
    public string GamePhase { get; set; }

    [JsonPropertyName("isGameRunning")]
    public bool IsGameRunning { get; set; }

    [JsonPropertyName("totalGameDuration")]
    public long TotalGameDuration { get; set; }

    [JsonPropertyName("isGamePaused")]
    public bool IsGamePaused { get; set; }

    [JsonPropertyName("averageTickRate")]
    public double AverageTickRate { get; set; }

    [JsonPropertyName("autoLoadSessionName")]
    public string AutoLoadSessionName { get; set; }
}
