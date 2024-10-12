namespace SatisfactoryBot.Application.Domain.UpdateClientPassword;

using MediatR;

internal class UpdateClientPasswordCommand : IRequest<bool>
{
    public ulong EntityId { get; set; }

    public string Password { get; set; }
}
