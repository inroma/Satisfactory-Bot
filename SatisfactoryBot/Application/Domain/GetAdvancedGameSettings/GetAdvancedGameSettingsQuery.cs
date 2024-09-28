namespace SatisfactoryBot.Application.Domain.GetAdvancedGameSettings;

using MediatR;
using SatisfactoryBot.Services.Api.Models;
using SatisfactoryBot.Services.Api.Models.Responses;

internal class GetAdvancedGameSettingsQuery : IRequest<BaseResponse<AdvancedGameSettingsResponse>>
{
    public ulong GuildId { get; set; }

    public GetAdvancedGameSettingsQuery(ulong guildId)
    {
        GuildId = guildId;
    }
}
