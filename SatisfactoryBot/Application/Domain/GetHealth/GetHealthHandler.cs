namespace SatisfactoryBot.Application.Domain.GetHealth;

using MediatR;
using Microsoft.Extensions.Logging;
using SatisfactoryBot.Services.Api;
using SatisfactoryBot.Services.Api.Models;
using SatisfactoryBot.Services.Api.Models.Responses;
using System.Threading;
using System.Threading.Tasks;

internal class GetHealthHandler : IRequestHandler<GetHealthGuery, BaseBody<HealthResponse>>
{
    private readonly ILogger<GetHealthHandler> logger;

    public GetHealthHandler(ILogger<GetHealthHandler> logger)
    {
        this.logger = logger;
    }

    public async Task<BaseBody<HealthResponse>> Handle(GetHealthGuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving server health");

        var client = new SatisfactoryClient(request.Url, request.Token);

        return await client.GetHealth();
    }
}
