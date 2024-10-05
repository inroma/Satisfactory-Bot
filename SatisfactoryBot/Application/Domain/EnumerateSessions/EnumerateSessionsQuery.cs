namespace SatisfactoryBot.Application.Domain.EnumerateSessions;

using MediatR;
using SatisfactoryBot.Services.Api.Models.Responses;
using SatisfactoryBot.Services.Api.Models;

internal class EnumerateSessionsQuery : IRequest<BaseResponse<EnumerateSessionsResponse>>
{
    public ulong GuildId { get; set; }

    public EnumerateSessionsQuery(ulong guildId)
    {
        GuildId = guildId;
    }
}