namespace SatisfactoryBot.Application.Domain.ListServers;

using MediatR;
using SatisfactoryBot.Data.Models;
using System.Collections.Generic;

internal class ListServersQuery : IRequest<List<SatisfactoryServer>>
{
    public ulong GuildId { get; set; }

    public ListServersQuery(ulong guildId)
    {
        GuildId = guildId;
    }
}
