namespace SatisfactoryBot.Application.Domain.SaveGame;

using MediatR;

public class SaveGameCommand : IRequest<bool>
{
    public ulong GuildId { get; set; }

    public string SaveName { get; set; }
}
