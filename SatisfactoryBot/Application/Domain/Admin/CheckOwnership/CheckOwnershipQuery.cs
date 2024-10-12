namespace SatisfactoryBot.Application.Domain.Admin.CheckOwnership;

using MediatR;
using SatisfactoryBot.Models.Dtos.Admin;

internal class CheckOwnershipQuery : IRequest<CheckOwnershipDto>
{
    public ulong GuildId { get; set; }

    public ulong ContextUserId { get; set; }
}
