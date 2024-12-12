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
using Microsoft.Extensions.Logging;
using System.Net;
using SatisfactoryBot.Helpers;
using SatisfactoryBot.Constants;

public class ClaimSatisfactoryServerHandler : IRequestHandler<ClaimSatisfactoryServerCommand, bool>
{
    #region Private Properties

    private readonly IUnitOfWork<ApplicationDbContext> unitOfWork;
    private ISatisfactoryClient client;
    private readonly IDiscordServerRepository discordRepository;
    private readonly ILogger<ClaimSatisfactoryServerHandler> logger;

    #endregion Private Properties

    #region Public Constructor

    public ClaimSatisfactoryServerHandler(IUnitOfWork<ApplicationDbContext> unitOfWork, IDiscordServerRepository discordServerRepository,
        ILogger<ClaimSatisfactoryServerHandler> logger)
    {
        this.unitOfWork = unitOfWork;
        discordRepository = discordServerRepository;
        this.logger = logger;
    }

    #endregion Public Constructor

    public async Task<bool> Handle(ClaimSatisfactoryServerCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var url = HandleUrlPort(request.Url);
            if (string.IsNullOrEmpty(request.Token) && !string.IsNullOrEmpty(request.Password))
            {
                var token = (await PasswordLessLogin(url, ApiPrivilegeLevel.InitialAdmin)).AuthenticationToken;
                request.Token = (await ClaimServer(url, token, request.ServerName, request.Password)).AuthenticationToken;
            }
            else if (!string.IsNullOrEmpty(request.Token))
            {
                await TokenAuthToSatisfactoryServer(url, request.Token);
                // if adding server with token, we retrieve the server name via Udp
                var ip = GetRemoteAddressFromUrl(url);
                request.ServerName = await client.GetServerNameWithUdp(ip);
            }
            if (request.Token != null)
            {
                var discordServer = discordRepository.GetOrCreateDiscordEntity(request.EntityId);

                if (discordServer.Id != default && unitOfWork.GetRepository<SatisfactoryServer>().GetFirstOrDefault(s =>
                    s.DiscordEntityId == discordServer.Id && s.Token == request.Token) != null)
                {
                    throw new Exception("Satisfactory server already registered on this Discord. Operation canceled.");
                }

                var isDefault = discordServer.SatisfactoryServers == null || discordServer.SatisfactoryServers?.Count(s => s.IsDefaultServer) == 0;
                var satServer = new SatisfactoryServer()
                {
                    Owner = request.UserId,
                    Token = request.Token,
                    Url = url,
                    Name = request.ServerName,
                    DiscordEntity = discordServer,
                    IsDefaultServer = isDefault,
                };

                unitOfWork.GetRepository<SatisfactoryServer>().Add(satServer);
                var result = unitOfWork.Save();
                return await Task.FromResult(result > 0);
            }
        }
        catch
        {
            throw;
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

    private async Task<AuthResponse> ClaimServer(string url, string token, string servName, string initAdminPwd)
    {
        try
        {
            client = new SatisfactoryClient(url, token);
            var result = await client.ClaimServer(servName, initAdminPwd);
            return result.Data;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error claiming server. {Ex}", ex.Message);
            throw;
        }
    }

    private static IPEndPoint GetRemoteAddressFromUrl(string url)
    {
        var myUri = new Uri(url);
        return new(Dns.GetHostAddresses(myUri.Host)[0], myUri.Port);
    }

    /// <summary>
    /// Reformate l'Url pour être viable
    /// </summary>
    /// <param name="url"></param>
    private static string HandleUrlPort(string url)
    {
        if (!RegexHelper.UrlEndingWithPort().IsMatch(url))
        {
            if (!url.StartsWith("https://"))
            {
                url = "https://" + url;
            }
            if (url.EndsWith('/'))
            {
                url = url[..^1];
            }
            url += $":{SatisfactoryConstants.SatisfactoryPort}";
        }
        return url;
    }
}
