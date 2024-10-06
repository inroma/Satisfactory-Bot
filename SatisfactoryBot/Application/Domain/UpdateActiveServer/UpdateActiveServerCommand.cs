namespace SatisfactoryBot.Application.Domain.UpdateActiveServer;

using MediatR;

internal class UpdateActiveServerCommand : IRequest<bool>
{
    public ulong EntityId { get; set; }

    public int NewlyActiveServerId { get; set; }

    public UpdateActiveServerCommand(ulong serverId, int newActiveServer)
    {
        EntityId = serverId;
        NewlyActiveServerId = newActiveServer;
    }
}
