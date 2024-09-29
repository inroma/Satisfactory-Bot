namespace SatisfactoryBot.Application.Domain.UpdateActiveServer;

using MediatR;

internal class UpdateActiveServerCommand : IRequest<bool>
{
    public ulong GuildId { get; set; }

    public int NewlyActiveServerId { get; set; }

    public UpdateActiveServerCommand(ulong serverId, int newActiveServer)
    {
        GuildId = serverId;
        NewlyActiveServerId = newActiveServer;
    }
}
