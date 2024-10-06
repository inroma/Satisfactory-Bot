namespace SatisfactoryBot.Services.DiscordInteractions;

using Discord;
using Discord.Interactions;
using MediatR;
using Microsoft.Extensions.Logging;
using SatisfactoryBot.Application.Domain.UpdateAdminPassword;
using SatisfactoryBot.Application.Domain.UpdateClientPassword;
using SatisfactoryBot.Helpers;
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

    [SlashCommand("user-password", "Update client password to access server")]
    [DefaultMemberPermissions(GuildPermission.Administrator)]
    public async Task ShowModalUpdateClientPassword()
    {
        try
        {
            logger.LogInformation("{User} starts updating ClientPassword", Context.User.Id);
            await RespondWithModalAsync<ClientPasswordModal>("user-password-modal");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "error: {Message}", ex.Message);
            await RespondAsync($"Error updating client password: {ex.Message}", ephemeral: true);
        }
    }

    [SlashCommand("admin-password", "Update the Admin server password")]
    [DefaultMemberPermissions(GuildPermission.Administrator)]
    public async Task ShowModalUpdateAdminPassword()
    {
        try
        {
            logger.LogInformation("{User} starts updating AdminPassword", Context.User.Id);
            await RespondWithModalAsync<AdminPasswordModal>("admin-password-modal");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "error: {Message}", ex.Message);
            await RespondAsync($"Error updating admin password: {ex.Message}", ephemeral: true);
        }
    }

    [ModalInteraction("user-password-modal")]
    public async Task StartClaiming(ClientPasswordModal modal)
    {
        logger.LogInformation("{User} confirmed new Client password", Context.User.Id);
        try
        {
            var result = false;
            result = await mediatr.Send(new UpdateClientPasswordCommand()
            {
                Password = modal.Password,
                EntityId = Context.GetContextEntityId()
            });
            await RespondAsync(result ? "Client password updated successfully !" : "Error updating client password");
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error updating client password: {ExMessage}", e.Message);
            await RespondAsync($"Error updating client password: {e.Message}");
        }
    }

    [ModalInteraction("admin-password-modal")]
    public async Task StartClaiming(AdminPasswordModal modal)
    {
        logger.LogInformation("{User} confirmed new Admin password", Context.User.Id);
        try
        {
            var result = false;
            result = await mediatr.Send(new UpdateAdminPasswordCommand()
            {
                Password = modal.Password,
                EntityId = Context.GetContextEntityId()
            });
            await RespondAsync(result ? "Admin password updated successfully !" : "Error updating Admin password");
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error updating Admin password: {ExMessage}", e.Message);
            await RespondAsync($"Error updating Admin password: {e.Message}");
        }
    }
}
