namespace SatisfactoryBot.Application.Domain.GetOptions;

using MediatR;
using SatisfactoryBot.Models.Dtos;

internal record GetOptionsQuery : IRequest<ServerOptionsDto>
{
    public ulong GuildId { get; set; }

    public GetOptionsQuery(ulong guildId)
    {
        GuildId = guildId;
    }
}
