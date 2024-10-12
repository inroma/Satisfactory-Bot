namespace SatisfactoryBot.Application.Domain.GetAdvancedGameSettings;

using MediatR;
using SatisfactoryBot.Models.Dtos;

internal class GetAdvancedGameSettingsQuery : IRequest<ServerAdvancedSettingsDto>
{
    public ulong EntityId { get; set; }

    public GetAdvancedGameSettingsQuery(ulong entityId)
    {
        EntityId = entityId;
    }
}
