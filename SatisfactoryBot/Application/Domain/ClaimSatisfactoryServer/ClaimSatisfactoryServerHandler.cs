namespace SatisfactoryBot.Application.Domain.ClaimSatisfactoryServer;

using MediatR;
using SatisfactoryBot.Data.UnitOfWork;
using SatisfactoryBot.Data;
using System.Threading;
using System.Threading.Tasks;
using SatisfactoryBot.Data.Models;
using SatisfactoryBot.Services.Api;
using SatisfactoryBot.Services.Api.Models.Responses;
using SatisfactoryBot.Services.Api.Interfaces;
using SatisfactoryBot.Services.Api.Models.Misc;
using SatisfactoryBot.Data.Repositories.Interfaces;

public class ClaimSatisfactoryServerHandler : IRequestHandler<ClaimSatisfactoryServerCommand, bool>
{
    #region Private Properties

    private readonly IUnitOfWork<ApplicationDbContext> unitOfWork;
    private ISatisfactoryClient client;
    private readonly IDiscordServerRepository discordRepository;

    #endregion Private Properties

    #region Public Constructor

    public ClaimSatisfactoryServerHandler(IUnitOfWork<ApplicationDbContext> unitOfWork, IDiscordServerRepository discordServerRepository)
    {
        this.unitOfWork = unitOfWork;
        discordRepository = discordServerRepository;
    }

    #endregion Public Constructor

    public async Task<bool> Handle(ClaimSatisfactoryServerCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Token) && !string.IsNullOrEmpty(request.Password))
        {
            var token = (await PasswordLessLogin(request.Url, ApiPrivilegeLevel.InitialAdmin)).AuthenticationToken;
            request.Token = (await ClaimServer(request.Url, token, request.Password)).AuthenticationToken;
        }
        else if (!string.IsNullOrEmpty(request.Token))
        {
            await TokenAuthToSatisfactoryServer(request.Url, request.Token);
        }
        if (request.Token != null)
        {
            var discordServer = discordRepository.GetOrCreateDiscordServer(request.GuildId);
            var isDefault = discordServer.SatisfactoryServers?.Count == 0;
            var satServer = new SatisfactoryServer()
            {
                Owner = request.UserId,
                Token = request.Token,
                Url = request.Url,
                DiscordServer = discordServer,
                IsDefaultServer = isDefault,
            };
            if (discordServer.Id != default && unitOfWork.GetRepository<SatisfactoryServer>().GetFirstOrDefault(s =>
                s.DiscordServerId == discordServer.Id && s.Token == satServer.Token) != null)
            {
                throw new Exception("Satisfactory server already registered on this Discord. Operation canceled.");
            }

            unitOfWork.GetRepository<SatisfactoryServer>().Add(satServer);
            var result = unitOfWork.Save();
            return await Task.FromResult(result > 0);
        }
        return false;
    }

    private async Task<bool> TokenAuthToSatisfactoryServer(string url, string token)
    {
        client = new SatisfactoryClient(url, token);
        return await client.VerifyAuthenticationToken();
    }

    private async Task<AuthResponse> PasswordLessLogin(string url, ApiPrivilegeLevel apiPrivilege)
    {
        client = new SatisfactoryClient(url);
        var result = await client.PasswordLessLogin(apiPrivilege);
        return result.Data;
    }

    private async Task<AuthResponse> ClaimServer(string url, string token, string initAdminPwd)
    {
        client = new SatisfactoryClient(url, token);
        var result = await client.ClaimServer(initAdminPwd);
        return result.Data;
    }
}
