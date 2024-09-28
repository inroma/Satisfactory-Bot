namespace SatisfactoryBot.Application.Domain.GetState;

using MediatR;
using SatisfactoryBot.Services.Api.Models;
using SatisfactoryBot.Services.Api.Models.Responses;

internal record GetStateQuery : IRequest<BaseResponse<StateResponse>>
{
    public string Url { get; set; }

    public string Token { get; set; }

    public GetStateQuery(string url, string token)
    {
        Url = url;
        Token = token;
    }
}
