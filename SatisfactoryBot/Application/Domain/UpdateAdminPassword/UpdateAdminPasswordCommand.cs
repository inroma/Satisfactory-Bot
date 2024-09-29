namespace SatisfactoryBot.Application.Domain.UpdateAdminPassword;

using MediatR;

internal class UpdateAdminPasswordCommand : IRequest<bool>
{
    public ulong GuildId { get; set; }

    public string Password { get; set; }
}
