namespace SatisfactoryBot.Application.Domain.GetOptions;

using MediatR;
using SatisfactoryBot.Models.Dtos;

internal record GetOptionsQuery : IRequest<ServerOptionsDto>
{
    public ulong EntityId { get; set; }

    public GetOptionsQuery(ulong entityId)
    {
        EntityId = entityId;
    }
}
