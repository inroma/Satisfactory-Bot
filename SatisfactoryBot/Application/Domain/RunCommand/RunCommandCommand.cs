namespace SatisfactoryBot.Application.Domain.RunCommand;

using MediatR;
using SatisfactoryBot.Services.Api.Models;
using SatisfactoryBot.Services.Api.Models.Responses;

internal class RunCommandCommand : IRequest<BaseResponse<CommandResponse>>
{
    public ulong GuildId { get; set; }

    public string CommandName { get; set; }

    public dynamic Value { get; set; } = null;
}
