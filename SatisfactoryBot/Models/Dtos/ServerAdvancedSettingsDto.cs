namespace SatisfactoryBot.Models.Dtos;

using SatisfactoryBot.Services.Api.Models.Responses;

internal class ServerAdvancedSettingsDto
{
    public string ServerName { get; set; }

    public AdvancedGameSettingsResponse AdvancedGameSettings { get; set; }
}
