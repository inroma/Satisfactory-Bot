namespace SatisfactoryBot.Application.Domain.SetAutoLoadSessionName;

using MediatR;

internal class SetAutoLoadSessionNameCommand : IRequest<bool>
{
    public ulong EntityId { get; set; }
    
    public string SessionName { get; set; }

    public SetAutoLoadSessionNameCommand(ulong entityId, string newSessionName)
    {
        EntityId = entityId;
        SessionName = newSessionName;
    }
}
