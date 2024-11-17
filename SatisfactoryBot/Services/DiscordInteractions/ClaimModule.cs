namespace SatisfactoryBot.Services.DiscordInteractions;

using Discord.Interactions;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SatisfactoryBot.Application.Domain.ClaimSatisfactoryServer;
using SatisfactoryBot.Helpers;
using SatisfactoryBot.Models.Enums;
using SatisfactoryBot.Models.Modals;
using SatisfactoryBot.Models.Settings;

public partial class ClaimModule : InteractionModuleBase<SocketInteractionContext>
{
    #region Private Properties

    private readonly ILogger<ClaimModule> logger;
    private readonly ISender mediatr;
    private readonly GlobalSettings globalSettings;

    #endregion Private Properties

    #region Public Constructor

    public ClaimModule(ILogger<ClaimModule> logger, ISender sender, IOptions<GlobalSettings> options)
    {
        this.logger = logger;
        mediatr = sender;
        globalSettings = options.Value;
    }

    #endregion Public Constructor

    [SlashCommand("claim", "Claims a Satisfactory server")]
    public async Task Claim([Summary(description: "Defines how to authenticate to the server")] ClaimEnum claimMethod)
    {
        try
        {
            logger.LogInformation("{User} starts claiming a server with {ClaimMethod}", Context.User.Id, claimMethod);
            await RespondWithModalAsync<ClaimModal>("claim-modal", modifyModal: (m) => {
                if (claimMethod == ClaimEnum.PasswordLess)
                {
                    m.RemoveComponent("token");
                }
                else if (claimMethod == ClaimEnum.Token)
                {
                    m.RemoveComponent("password");
                    m.RemoveComponent("name");
                }
                m.Components.ActionRows = m.Components.ActionRows.Where(r => r.Components.Count > 0).ToList();
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "error: {Message}", ex.Message);
        }
    }

    [ModalInteraction("claim-modal")]
    public async Task StartClaiming(ClaimModal claim)
    {
        logger.LogInformation("{User} confirmed claim", Context.User.Id);
        if (!globalSettings.EnableLocalIPs && RegexHelper.LocalIpRegex().IsMatch(claim.Url))
        {
            await RespondAsync("Server address cannot be a local IP, you can enable it in the bot config file");
            return;
        }
        try
        {
            await DeferAsync();
            var result = await mediatr.Send(new ClaimSatisfactoryServerCommand()
            {
                ServerName = claim.ServerName,
                Url = claim.Url,
                Password = claim.Password,
                Token = claim.Token,
                EntityId = Context.GetContextEntityId(),
                UserId = Context.User.Id,
            });
            await FollowupAsync(result ? "Server claimed successfully !" : "Error claiming the server");
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error during claim: {ExMessage}", e.Message);
            await FollowupAsync($"Error during claim: {e.InnerException?.Message ?? e.Message}");
        }
    }
}
