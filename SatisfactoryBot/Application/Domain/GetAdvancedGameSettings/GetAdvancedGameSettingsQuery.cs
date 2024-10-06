namespace SatisfactoryBot.Application.Domain.GetAdvancedGameSettings;

using MediatR;
using SatisfactoryBot.Models.Dtos;

internal class GetAdvancedGameSettingsQuery : IRequest<ServerAdvancedSettingsDto>
{
    public ulong GuildId { get; set; }

    public GetAdvancedGameSettingsQuery(ulong guildId)
    {
        GuildId = guildId;
    }
}
