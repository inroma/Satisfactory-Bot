namespace SatisfactoryBot.Application.Domain.GetHealth;

using MediatR;
using SatisfactoryBot.Services.Api.Models;
using SatisfactoryBot.Services.Api.Models.Responses;

internal record GetHealthGuery : IRequest<BaseBody<HealthResponse>>
{
    public string Url { get; set; }

    public string Token { get; set; }

    public GetHealthGuery(string url, string token)
    {
        Url = url;
        Token = token;
    }
}
