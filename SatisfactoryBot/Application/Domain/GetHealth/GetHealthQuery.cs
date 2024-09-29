namespace SatisfactoryBot.Application.Domain.GetHealth;

using MediatR;
using SatisfactoryBot.Services.Api.Models;
using SatisfactoryBot.Services.Api.Models.Responses;

internal record GetHealthQuery : IRequest<BaseResponse<HealthResponse>>
{
    public ulong GuildId { get; set; }

    public GetHealthQuery(ulong guildId)
    {
        GuildId = guildId;
    }
}
