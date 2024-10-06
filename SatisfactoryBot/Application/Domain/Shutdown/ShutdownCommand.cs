namespace SatisfactoryBot.Application.Domain.Shutdown;

using MediatR;

public class ShutdownCommand : IRequest<bool>
{
    public ulong EntityId { get; set; }
}
