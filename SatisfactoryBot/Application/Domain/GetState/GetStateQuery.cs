namespace SatisfactoryBot.Application.Domain.GetState;

using MediatR;
using SatisfactoryBot.Models.Dtos;

internal record GetStateQuery : IRequest<ServerStateDto>
{
    public ulong EntityId { get; set; }

    public GetStateQuery(ulong entityId)
    {
        EntityId = entityId;
    }
}
