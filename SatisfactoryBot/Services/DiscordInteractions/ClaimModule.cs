﻿namespace SatisfactoryBot.Services.DiscordInteractions;

using Discord.Interactions;
using MediatR;
using Microsoft.Extensions.Logging;
using SatisfactoryBot.Application.Domain.ClaimSatisfactoryServer;
using SatisfactoryBot.Helpers;
using SatisfactoryBot.Models.Enums;
using SatisfactoryBot.Models.Modals;

public partial class ClaimModule : InteractionModuleBase<SocketInteractionContext>
{
    #region Private Properties

    private readonly ILogger<ClaimModule> logger;
    private readonly ISender mediatr;

    #endregion Private Properties

    #region Public Constructor

    public ClaimModule(ILogger<ClaimModule> logger, ISender sender)
    {
        this.logger = logger;
        mediatr = sender;
    }

    #endregion Public Constructor

    [SlashCommand("claim", "Claims a Satisfactory server")]
    public async Task Claim([Summary(description: "Defines how to authenticate to the server")] ClaimEnum claimMethod)
    {
        try
        {
            logger.LogInformation("{User} starts claiming a server with {ClaimMethod}", Context.User.Id, claimMethod);
            await RespondWithModalAsync<ClaimModal>("claim-modal", modifyModal: (m) => {
                _ = claimMethod switch
                {
                    ClaimEnum.Token => m.RemoveComponent("password"),
                    ClaimEnum.Password => m.RemoveComponent("token"),
                    ClaimEnum.PasswordLess => m.RemoveComponent("token").RemoveComponent("password"),
                    _ => throw new NotImplementedException(),
                };
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
        if (RegexHelper.LocalIpRegex().IsMatch(claim.Url))
        {
            await RespondAsync("Server address cannot be a local IP");
            return;
        }
        try
        {
            var result = await mediatr.Send(new ClaimSatisfactoryServerCommand(claim.Url, claim.Password, claim.Token, Context.User.Id));
            await RespondAsync(result ? "Server claimed successfully !" : "Error claiming the server");
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error during claim, server responded: {ExMessage}", e.Message);
            await RespondAsync($"Error during claim, server responded: {e.Message}");
        }
    }
}
