namespace SatisfactoryBot.Application.Domain.GetState;

using MediatR;
using SatisfactoryBot.Services.Api.Models;
using SatisfactoryBot.Services.Api.Models.Responses;

internal record GetStateQuery : IRequest<BaseResponse<StateResponse>>
{
    public ulong GuildId { get; set; }

    public GetStateQuery(ulong guildId)
    {
        GuildId = guildId;
    }
}
