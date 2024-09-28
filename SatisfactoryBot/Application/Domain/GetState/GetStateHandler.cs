namespace SatisfactoryBot.Application.Domain.GetState;

using MediatR;
using Microsoft.Extensions.Logging;
using SatisfactoryBot.Services.Api;
using SatisfactoryBot.Services.Api.Models;
using SatisfactoryBot.Services.Api.Models.Responses;
using System.Threading;
using System.Threading.Tasks;

internal class GetStateHandler : IRequestHandler<GetStateQuery, BaseBody<StateResponse>>
{
    private readonly ILogger<GetStateHandler> logger;

    public GetStateHandler(ILogger<GetStateHandler> logger)
    {
        this.logger = logger;
    }

    public async Task<BaseBody<StateResponse>> Handle(GetStateQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving server state");

        var client = new SatisfactoryClient(request.Url, request.Token);

        return await client.GetState();
    }
}
