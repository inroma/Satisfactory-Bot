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

public class ClaimSatisfactoryServerHandler : IRequestHandler<ClaimSatisfactoryServerCommand, bool>
{
    #region Private Properties

    private readonly IUnitOfWork<ApplicationDbContext> unitOfWork;
    private ISatisfactoryClient client;

    #endregion Private Properties

    #region Public Constructor

    public ClaimSatisfactoryServerHandler(IUnitOfWork<ApplicationDbContext> unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    #endregion Public Constructor

    public async Task<bool> Handle(ClaimSatisfactoryServerCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Token) && string.IsNullOrEmpty(request.Password))
        {
            var token = (await PasswordLessLogin(request.Url, ApiPrivilegeLevel.InitialAdmin)).AuthenticationToken;
            request.Token = (await ClaimServer(request.Url, token)).AuthenticationToken;
        }
        else if (!string.IsNullOrEmpty(request.Token))
        {
            await TokenAuthToSatisfactoryServer(request.Url, request.Token);
        }
        else if (!string.IsNullOrEmpty(request.Password))
        {
            request.Token = (await PasswordLogin(request.Url, request.Password)).AuthenticationToken;
        }
        if (request.Token != null)
        {
            var discordServer = GetOrCreateDiscordServer(request.GuildId);
            var satServer = new SatisfactoryServer()
            {
                Owner = request.UserId,
                Token = request.Token,
                Url = request.Url,
                DiscordServer = discordServer
            };

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

    private async Task<AuthResponse> ClaimServer(string url, string token)
    {
        client = new SatisfactoryClient(url, token);
        var result = await client.ClaimServer();
        return result.Data;
    }

    private async Task<AuthResponse> PasswordLogin(string url, string password)
    {
        client = new SatisfactoryClient(url);
        var result = await client.PasswordLogin(password);
        return result.Data;
    }

    private DiscordServer GetOrCreateDiscordServer(ulong guildId)
    {
        var discordRepository = unitOfWork.GetRepository<DiscordServer>();
        var discordServer = discordRepository.GetFirstOrDefault(d => d.GuildId == guildId);
        if (discordServer == null)
        {
            discordServer = new DiscordServer()
            {
                GuildId = guildId
            };
            discordRepository.Add(discordServer);
        }
        return discordServer;
    }
}
