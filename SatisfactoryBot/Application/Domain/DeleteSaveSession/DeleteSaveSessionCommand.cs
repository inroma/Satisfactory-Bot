namespace SatisfactoryBot.Application.Domain.DeleteSaveSession;

using MediatR;

internal class DeleteSaveSessionCommand : IRequest<bool>
{
    public ulong GuildId { get; set; }

    public string SessionName { get; set; }
}
