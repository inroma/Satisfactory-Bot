namespace SatisfactoryBot.Helpers;

using Discord;

internal class ResponseHelper
{
    /// <summary>
    /// Créer un ensemble de bouton "Confirmer" / "Annuler"
    /// </summary>
    public static MessageComponent CreateConfirmCancelButtons(string initialInteractionId)
    {
        try
        {
            var component = new ComponentBuilder()
                .WithButton("Confirm", initialInteractionId + $"-confirm")
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
