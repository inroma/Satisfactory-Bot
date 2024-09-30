namespace SatisfactoryBot.Application.Domain.EnumerateSessions;

using MediatR;
using SatisfactoryBot.Services.Api.Models.Responses;
using SatisfactoryBot.Services.Api.Models;

internal class EnumerateSessionsCommand : IRequest<BaseResponse<EnumerateSessionsResponse>>
{
    public ulong GuildId { get; set; }

    public EnumerateSessionsCommand(ulong guildId)
    {
        GuildId = guildId;
    }
}