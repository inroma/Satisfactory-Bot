namespace SatisfactoryBot.Application.Domain.Admin.TransferOwnership;

using MediatR;

internal class TransferOwnershipCommand : IRequest<bool>
{
    public ulong GuildId { get; set; }

    public ulong NewOwnerId { get; set; }
}
