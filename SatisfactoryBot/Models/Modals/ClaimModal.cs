namespace SatisfactoryBot.Models.Modals;

using Discord;
using Discord.Interactions;

public class ClaimModal : IModal
{
    public string Title => "Server Claim";

    [InputLabel(nameof(Url))]
    [ModalTextInput("url", maxLength: 200, initValue: "https://")]
    public string Url { get; set; }

    [InputLabel("Admin Password")]
    [ModalTextInput("password", placeholder: "init password", maxLength: 80)]
    public string Password { get; set; }

    [InputLabel(nameof(Token))]
    [ModalTextInput("token", TextInputStyle.Paragraph, maxLength: 500)]
    public string Token { get; set; }
}
