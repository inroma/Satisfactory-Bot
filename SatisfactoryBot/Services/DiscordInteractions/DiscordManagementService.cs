namespace SatisfactoryBot.Services.DiscordInteractions;

using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using MediatR;
using Microsoft.Extensions.Logging;
using SatisfactoryBot.Application.Domain.Admin.CheckOwnership;
using SatisfactoryBot.Application.Domain.Admin.TransferOwnership;
using SatisfactoryBot.Application.Domain.ListServers;
using SatisfactoryBot.Application.Domain.UpdateActiveServer;
using SatisfactoryBot.Data.Models;
using SatisfactoryBot.Helpers;
using SatisfactoryBot.Services.DiscordInteractions.Attributes;
using System.Threading.Tasks;

public class DiscordManagementService : InteractionModuleBase<SocketInteractionContext>
{
    #region Private Properties

    private readonly ILogger<DiscordManagementService> logger;
    private readonly ISender mediatr;

    #endregion Private Properties

    #region Public Constructor

    public DiscordManagementService(ILogger<DiscordManagementService> logger, ISender sender)
    {
        this.logger = logger;
        mediatr = sender;
    }

    #endregion Public Constructor

    #region Public Methods

    [SlashCommand("list", "List all Satisfactory server registered")]
    public async Task ListServers()
    {
        logger.LogInformation("ListServers command started");
        try
        {
            var result = await mediatr.Send(new ListServersQuery(Context.GetContextEntityId()));
            var menu = CreateSelectMenu(result);
            var builder = new ComponentBuilder().WithSelectMenu(menu);

            await RespondAsync("Satisfactory servers list", components: builder.Build());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting Satisfactory servers: {Ex}", ex.Message);
            await RespondAsync($"Error getting Satisfactory servers: {ex.Message}", ephemeral: true);
        }
    }

    [ComponentInteraction("active-server")]
    [DefaultMemberPermissions(GuildPermission.Administrator)]
    public async Task UpdateActiveServer(string[] selectedValues)
    {
        try
        {
            logger.LogInformation("updating active server {EntityId}", Context.GetContextEntityId());
            await DeferAsync();
            var result = await mediatr.Send(new UpdateActiveServerCommand(Context.GetContextEntityId(), Convert.ToInt32(selectedValues[0])));
            await Task.FromResult(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error changing default server. {Ex}", ex.Message);
        }
    }

    [SlashCommand("owner", "transfer ownership to another User")]
    [DefaultMemberPermissions(GuildPermission.Administrator)]
    [CommandContextType(InteractionContextType.Guild)]
    public async Task CheckTransferOwnership(IGuildUser newOwner)
    {
        logger.LogInformation("CheckOwnership command started");
        try
        {
            if (newOwner.IsBot)
            {
                await RespondAsync("You can't transfer ownership to a bot !", ephemeral: true);
                return;
            }
            if (newOwner == Context.User)
            {
                await RespondAsync("You can't transfer ownership to yourself !", ephemeral: true);
                return;
            }
            var result = await mediatr.Send(new CheckOwnershipQuery()
            {
                GuildId = Context.Guild.Id,
                ContextUserId = Context.User.Id,
            });
            if (!result.CheckOk)
            {
                await RespondAsync("You're not the current Owner of the active server, use /list to change the active server.", ephemeral: true);
                return;
            }
            var component = ResponseHelper.CreateConfirmCancelButtons(
                ((SocketCommandBase)Context.Interaction).CommandName, Context.User.Id, newOwner.Id.ToString());
            await RespondAsync($"Do you really want to transfer ownership of \"{result.ServerName}\" to {newOwner.GlobalName} ?", components: component);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error checking ownership: {Ex}", ex.Message);
            await RespondAsync($"Error checking ownership: {ex.Message}", ephemeral: true);
        }
    }

    [ComponentInteraction("owner-confirm-*:*")]
    [DoUserCheck]
    [CommandContextType(InteractionContextType.Guild)]
    public async Task TransferOwnership(string newOwner)
    {
        logger.LogInformation("TransferOwnership command started");
        try
        {
            var result = await mediatr.Send(new TransferOwnershipCommand()
            {
                GuildId = Context.Guild.Id,
                NewOwnerId = Convert.ToUInt64(newOwner),
            });
            await RespondAsync(result ? "New owner defined" : "Failed to transfer ownership");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error transfering ownership: {Ex}", ex.Message);
            await RespondAsync($"Error transfering ownership: {ex.Message}", ephemeral: true);
        }
    }

    #endregion Public Methods

    private static SelectMenuBuilder CreateSelectMenu(List<SatisfactoryServer> servers)
    {
        var menuBuilder = new SelectMenuBuilder()
        {
            CustomId = "active-server",
            MaxValues = 1,
            MinValues = 1
        };
        servers.ForEach(server => {
            menuBuilder.AddOption(new()
            {
                Label = server.Name ?? server.Url,
                Description = server.Url,
                Value = server.Id.ToString(),
                IsDefault = server.IsDefaultServer,
            });
        });
        return menuBuilder;
    }
}
