namespace SatisfactoryBot.Application.Domain.RenameServer;

using MediatR;

internal class RenameServerCommand : IRequest<bool>
{
    public ulong EntityId { get; set; }

    public string Name { get; set; }

    public RenameServerCommand(ulong entityId, string newName)
    {
        EntityId = entityId;
        Name = newName;
    }
}
