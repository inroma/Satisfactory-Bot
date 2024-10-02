namespace SatisfactoryBot.Application.Domain.DeleteSave;

using MediatR;

public class DeleteSaveCommand : IRequest<bool>
{
    public ulong GuildId { get; set; }

    public string SaveName { get; set; }
}
