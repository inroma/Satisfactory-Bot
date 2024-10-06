namespace SatisfactoryBot.Helpers;

using Discord.Interactions;

internal static class ContextHelper
{
    /// <summary>
    /// Get the Id of the User or the Guild based on the context
    /// </summary>
    public static ulong GetContextEntityId(this SocketInteractionContext context)
    {
        if (context.Guild == null)
        {
            return context.User.Id;
        }
        else
        {
            return context.Guild.Id;
        }
    }
}
