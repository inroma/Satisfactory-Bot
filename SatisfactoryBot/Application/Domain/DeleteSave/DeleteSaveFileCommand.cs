namespace SatisfactoryBot.Application.Domain.DeleteSave;

using MediatR;

public class DeleteSaveFileCommand : IRequest<bool>
{
    public ulong GuildId { get; set; }

    public string SaveName { get; set; }
}
