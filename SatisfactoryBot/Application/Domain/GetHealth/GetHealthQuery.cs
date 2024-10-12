namespace SatisfactoryBot.Application.Domain.GetHealth;

using MediatR;
using SatisfactoryBot.Models.Dtos;

internal record GetHealthQuery : IRequest<ServerHealthDto>
{
    public ulong EntityId { get; set; }

    public GetHealthQuery(ulong entityId)
    {
        EntityId = entityId;
    }
}
