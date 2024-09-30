namespace SatisfactoryBot.Application.Domain.SetAutoLoadSessionName;

using MediatR;

internal class SetAutoLoadSessionNameCommand : IRequest<bool>
{
    public ulong GuildId { get; set; }
    
    public string SessionName { get; set; }

    public SetAutoLoadSessionNameCommand(ulong guildId, string newSessionName)
    {
        GuildId = guildId;
        SessionName = newSessionName;
    }
}
