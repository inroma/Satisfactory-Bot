﻿namespace SatisfactoryBot.Services.DiscordInteractions;

using Discord;
using Discord.Interactions;
using MediatR;
using Microsoft.Extensions.Logging;
using SatisfactoryBot.Application.Domain.ListServers;
using SatisfactoryBot.Application.Domain.UpdateActiveServer;
using SatisfactoryBot.Data.Models;
using SatisfactoryBot.Helpers;
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
