namespace SatisfactoryBot.Application.Domain.UpdateAdminPassword;

using MediatR;

internal class UpdateAdminPasswordCommand : IRequest<bool>
{
    public ulong EntityId { get; set; }

    public string Password { get; set; }
}
