namespace SatisfactoryBot.Helpers;

using Discord;

internal class ResponseHelper
{
    /// <summary>
    /// Créer un ensemble de bouton "Confirmer" / "Annuler"
    /// Si un UserId est passé, seul lui pourra confirmer.
    /// </summary>
    public static MessageComponent CreateConfirmCancelButtons(string initialInteractionId, ulong? userId)
    {
        try
        {
            var idCheck = userId != null ? $":{userId}" : "";
            var component = new ComponentBuilder()
                .WithButton("Confirm", initialInteractionId + $"-confirm" + idCheck)
                .WithButton("Cancel", "delete-interaction", ButtonStyle.Secondary)
                .Build();
            return component;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
}
