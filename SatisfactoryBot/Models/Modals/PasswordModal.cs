namespace SatisfactoryBot.Models.Modals;

using Discord.Interactions;

public class PasswordModal : IModal
{
    public bool UserPasswordModal { get; set; } = true;

    public string Title => $"{(UserPasswordModal ? "User" : "Admin")} Password update";

    [InputLabel(nameof(Password))]
    [ModalTextInput("password", maxLength: 80)]
    public string Password { get; set; }

    public PasswordModal()
    {
    }

    public PasswordModal(bool isUserPasswordUpdate = true)
    {
        UserPasswordModal = isUserPasswordUpdate;
    }
}
