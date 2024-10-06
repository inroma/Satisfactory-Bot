namespace SatisfactoryBot.Helpers;

using Discord;
using SatisfactoryBot.Models;
using SatisfactoryBot.Models.Dtos;
using SatisfactoryBot.Services.Api.Models.Responses;

internal class ResponseHelper
{
    /// <summary>
    /// Créer un ensemble de bouton "Confirmer" / "Annuler"
    /// Si un UserId est passé, seul lui pourra confirmer.
    /// </summary>
    public static MessageComponent CreateConfirmCancelButtons(string initialInteractionId, ulong? userId)
    {
        try
        {
            var idCheck = userId != null ? $":{userId}" : "";
            var component = new ComponentBuilder()
                .WithButton("Confirm", initialInteractionId + $"-confirm" + idCheck)
                .WithButton("Cancel", "delete-interaction", ButtonStyle.Secondary)
                .Build();
            return component;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    /// <summary>
    /// Créer un Embed pour le HealthCheck
    /// </summary>
    public static Embed CreateEmbedHealthCheck(HealthResponse health, string serverName)
    {
        var isHealthy = health.Health == ApiConstants.Healthy;
        var embedBuilder = new EmbedBuilder()
        {
            Title = $"{serverName} Health Status",
            Description = $"Server is {health.Health}",
            Color = isHealthy ? Color.Green : Color.Orange,
            Timestamp = DateTime.UtcNow,
            Footer = new EmbedFooterBuilder() { Text = serverName }
        };
        if (!string.IsNullOrEmpty(health.ServerCustomData))
        {
            embedBuilder.AddField(new EmbedFieldBuilder() { Name = "Server Custom Data", Value = health.ServerCustomData });
        }
        return embedBuilder.Build();
    }

    /// <summary>
    /// Créer un Embed pour le Server State
    /// </summary>
    public static Embed CreateEmbedServerState(ServerStateDto state)
    {
        var gameState = state.StateResponse.ServerGameState;
        var time = TimeSpan.FromSeconds(gameState.TotalGameDuration);
        var embedBuilder = new EmbedBuilder()
        {
            Title = $"{state.ServerName} State",
            Timestamp = DateTime.UtcNow,
            Color = GetStateEmbedColor(state),
            Fields = [
                new EmbedFieldBuilder() { IsInline = true, Name = "Active Session Name", Value = gameState.ActiveSessionName },
                new EmbedFieldBuilder() { IsInline = true, Name = "Tech tier", Value = gameState.TechTier },
                new EmbedFieldBuilder() { IsInline = true, Name = "Active Schematic", Value = GetActiveSchematic(gameState.ActiveSchematic) },

                new EmbedFieldBuilder() { IsInline = true, Name = "Players", Value = $"{gameState.NumConnectedPlayers} / {gameState.PlayerLimit}" },
                new EmbedFieldBuilder() { IsInline = true, Name = "Average Tick Rate", Value = Math.Round(gameState.AverageTickRate, 2) },
                new EmbedFieldBuilder() { IsInline = true, Name = "\u2800", Value = "\u2800" },

                new EmbedFieldBuilder() { IsInline = true, Name = "Game Duration", Value = $"{(time.Days > 0 ? (time.Days + " days ") : "")}{time:hh\\:mm\\:ss}" },
                new EmbedFieldBuilder() { IsInline = true, Name = "Game Running", Value = gameState.IsGameRunning },
                new EmbedFieldBuilder() { IsInline = true, Name = "Game Paused", Value = gameState.IsGamePaused },

                new EmbedFieldBuilder() { IsInline = true, Name = "AutoLoadSessionName", Value = gameState.AutoLoadSessionName },
                new EmbedFieldBuilder() { IsInline = true, Name = "Game Phase", Value = GetPhaseDescription(gameState.GamePhase) },
            ],
            Footer = new EmbedFooterBuilder() { Text = state.ServerName }
        };
        return embedBuilder.Build();
    }

    /// <summary>
    /// Créer un Embed pour les Server Options
    /// </summary>
    public static Embed CreateEmbedServerOptions(ServerOptionsDto options)
    {
        var embedBuilder = new EmbedBuilder()
        {
            Title = $"{options.ServerName} Options",
            Timestamp = DateTime.UtcNow,
            Footer = new EmbedFooterBuilder() { Text = options.ServerName }
        };
        foreach (var item in options.OptionsResponse.ServerOptions)
        {
            embedBuilder.AddField(item.Key, item.Value, true);
        }
        foreach (var item in options.OptionsResponse.PendingServerOptions)
        {
            embedBuilder.AddField("Pending: "+item.Key, item.Value, true);
        }
        return embedBuilder.Build();
    }

    /// <summary>
    /// Créer un Embed pour les AdvancedSettings
    /// </summary>
    public static Embed CreateEmbedAdvancedSettings(ServerAdvancedSettingsDto advancedSettings)
    {
        var embedBuilder = new EmbedBuilder()
        {
            Title = $"{advancedSettings.ServerName} Advanced Settings",
            Timestamp = DateTime.UtcNow,
            Fields = [
                new() { Name = "Creative Mode", Value = advancedSettings.AdvancedGameSettings.CreativeModeEnabled },
            ],
            Footer = new EmbedFooterBuilder() { Text = advancedSettings.ServerName }
        };
        foreach (var item in advancedSettings.AdvancedGameSettings.AdvancedGameSettings)
        {
            embedBuilder.AddField(item.Key, item.Value, true);
        }
        return embedBuilder.Build();
    }

    public static string GetCommandResponse(CommandResponse response)
    {
        return (response.ReturnValue ? $"Command executed successfully: " : $"Failed to execute Command: ") + response.CommandResult;
    }

    #region Private Methods

    private static Color GetStateEmbedColor(ServerStateDto stateDto)
    {
        if (stateDto.StateResponse.ServerGameState.IsGamePaused)
        {
            return Color.LightGrey;
        }
        if (!stateDto.StateResponse.ServerGameState.IsGameRunning)
        {
            return Color.Purple;
        }
        if (stateDto.StateResponse.ServerGameState.NumConnectedPlayers == 0)
        {
            return Color.DarkBlue;
        }
        if (stateDto.StateResponse.ServerGameState.AverageTickRate < 10)
        {
            return Color.Red;
        }
        else if (stateDto.StateResponse.ServerGameState.AverageTickRate < 30)
        {
            return Color.Orange;
        }
        else
        {
            return Color.Green;
        }
    }

    private static string GetPhaseDescription(string gamePhase)
    {
        var parsed = int.TryParse(gamePhase.Split('_')[^1].Trim('\''), out var result);
        if (!parsed)
        {
            return gamePhase;
        }
        return result switch
        {
            0 => "Game Init",
            1 => "Distribution Platform",
            2 => "Construction Dock",
            3 => "Main Body",
            4 => "Propulsion",
            5 => "Assembly",
            _ => gamePhase
        };
    }

    private static string GetActiveSchematic(string schematic)
    {
        if (schematic == ApiConstants.RailwaySignallingSchematic)
        {
            return "Railway Signalling";
        }
        if (schematic == ApiConstants.LogisticsMk5Schematic)
        {
            return "Logistics Mk.5";
        }
        return schematic;
    }

    #endregion Private Methods
}
