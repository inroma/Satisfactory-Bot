namespace SatisfactoryBot.Application.Domain.RenameServer;

using MediatR;

internal class RenameServerCommand : IRequest<bool>
{
    public ulong GuildId { get; set; }

    public string Name { get; set; }

    public RenameServerCommand(ulong guildId, string newName)
    {
        GuildId = guildId;
        Name = newName;
    }
}
