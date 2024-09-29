namespace SatisfactoryBot.Services.DiscordInteractions;

using Discord.Interactions;
using MediatR;
using Microsoft.Extensions.Logging;
using SatisfactoryBot.Application.Domain.ClaimSatisfactoryServer;
using SatisfactoryBot.Models.Enums;
using SatisfactoryBot.Models.Modals;

public partial class PasswordModule : InteractionModuleBase<SocketInteractionContext>
{
    #region Private Properties

    private readonly ILogger<PasswordModule> logger;
    private readonly ISender mediatr;

    #endregion Private Properties

    #region Public Constructor

    public PasswordModule(ILogger<PasswordModule> logger, ISender sender)
    {
        this.logger = logger;
        mediatr = sender;
    }

    #endregion Public Constructor

    [SlashCommand("user-password", "Update user server password")]
    public async Task ShowModalUpdateUserPassword()
    {
        try
        {
            logger.LogInformation("{User} starts updating UserPassword", Context.User.Id);
            var modal = new PasswordModal();
            await RespondWithModalAsync("password-modal", modal);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "error: {Message}", ex.Message);
        }
    }

    [SlashCommand("admin-password", "Update the Admin server password")]
    public async Task ShowModalUpdateAdminPassword()
    {
        try
        {
            logger.LogInformation("{User} starts updating AdminPassword", Context.User.Id);
            var modal = new PasswordModal(false);
            await RespondWithModalAsync("password-modal", modal);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "error: {Message}", ex.Message);
        }
    }

    [ModalInteraction("password-modal")]
    public async Task StartClaiming(PasswordModal claim)
    {
        logger.LogInformation("{User} confirmed new {Type} password", Context.User.Id, claim.UserPasswordModal ? "User" : "Admin");
        try
        {
            var result = false;
            if (claim.UserPasswordModal)
            {
                //result = await mediatr.Send(new UpdateUserPassword(claim.Url, claim.Password, claim.Token, Context.Guild.Id, Context.User.Id));
            }
            else
            {
                //result = await mediatr.Send(new ClaimSatisfactoryServerCommand(claim.Url, claim.Password, claim.Token, Context.Guild.Id, Context.User.Id));
            }
            await RespondAsync(result ? "Server claimed successfully !" : "Error claiming the server");
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error during claim, server responded: {ExMessage}", e.Message);
            await RespondAsync($"Error during claim, server responded: {e.Message}");
        }
    }
}
