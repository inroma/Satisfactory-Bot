namespace SatisfactoryBot.Application.Domain.GetOptions;

using MediatR;
using SatisfactoryBot.Services.Api.Models.Responses;
using SatisfactoryBot.Services.Api.Models;

internal record GetOptionsQuery : IRequest<BaseResponse<OptionsResponse>>
{
    public ulong GuildId { get; set; }

    public GetOptionsQuery(ulong guildId)
    {
        GuildId = guildId;
    }
}
