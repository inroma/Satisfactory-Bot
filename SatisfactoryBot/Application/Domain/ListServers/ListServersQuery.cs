namespace SatisfactoryBot.Application.Domain.ListServers;

using MediatR;
using SatisfactoryBot.Data.Models;
using System.Collections.Generic;

internal class ListServersQuery : IRequest<List<SatisfactoryServer>>
{
    public ulong EntityId { get; set; }

    public ListServersQuery(ulong entityId)
    {
        EntityId = entityId;
    }
}
