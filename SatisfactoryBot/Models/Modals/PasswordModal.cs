namespace SatisfactoryBot.Models.Modals;

using Discord.Interactions;

public class ClientPasswordModal : IModal
{
    public string Title => "User Password Update";

    [InputLabel(nameof(Password))]
    [RequiredInput(false)]
    [ModalTextInput("password", maxLength: 200, placeholder: "Empty to remove password")]
    public string Password { get; set; }
}

public class AdminPasswordModal : IModal
{
    public string Title => "Admin Password Update";

    [InputLabel(nameof(Password))]
    [ModalTextInput("password", maxLength: 200, minLength: 10)]
    public string Password { get; set; }
}
