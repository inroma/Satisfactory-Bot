namespace SatisfactoryBot.Application.Domain.DeleteSave;

using MediatR;

public class DeleteSaveFileCommand : IRequest<bool>
{
    public ulong EntityId { get; set; }

    public string SaveName { get; set; }
}
