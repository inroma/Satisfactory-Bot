namespace SatisfactoryBot.Application.Domain.GetHealth;

using MediatR;
using SatisfactoryBot.Models.Dtos;

internal record GetHealthQuery : IRequest<ServerHealthDto>
{
    public ulong GuildId { get; set; }

    public GetHealthQuery(ulong guildId)
    {
        GuildId = guildId;
    }
}
