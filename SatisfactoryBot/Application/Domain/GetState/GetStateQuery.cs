namespace SatisfactoryBot.Application.Domain.GetState;

using MediatR;
using SatisfactoryBot.Models.Dtos;

internal record GetStateQuery : IRequest<ServerStateDto>
{
    public ulong GuildId { get; set; }

    public GetStateQuery(ulong guildId)
    {
        GuildId = guildId;
    }
}
