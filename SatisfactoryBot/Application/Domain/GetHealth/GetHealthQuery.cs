namespace SatisfactoryBot.Application.Domain.GetHealth;

using MediatR;
using SatisfactoryBot.Services.Api.Models;
using SatisfactoryBot.Services.Api.Models.Responses;

internal record GetHealthQuery : IRequest<BaseResponse<HealthResponse>>
{
    public string Url { get; set; }

    public string Token { get; set; }

    public GetHealthQuery(string url, string token)
    {
        Url = url;
        Token = token;
    }
}
