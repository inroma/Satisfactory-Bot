namespace SatisfactoryBot.Services.Api.Interfaces;

using SatisfactoryBot.Services.Api.Models;
using SatisfactoryBot.Services.Api.Models.Misc;
using SatisfactoryBot.Services.Api.Models.Responses;
using System.Net;
using System.Threading.Tasks;

public interface ISatisfactoryClient
{
    Task<BaseResponse<HealthResponse>> GetHealth();

    Task<BaseResponse<AuthResponse>> PasswordLessLogin(ApiPrivilegeLevel apiPrivilege = ApiPrivilegeLevel.Administrator);

    Task<bool> VerifyAuthenticationToken();

    Task<BaseResponse<AuthResponse>> ClaimServer(string servName, string adminPwd);

    Task<BaseResponse<StateResponse>> GetState();

    Task<BaseResponse<OptionsResponse>> GetOptions();

    Task<BaseResponse<AdvancedGameSettingsResponse>> GetAdvancedGameSettings();

    Task<bool> RenameServer(string newName);

    Task<bool> UpdateClientPassword(string password);

    Task<bool> UpdateAdminPassword(string password, string token);

    Task<string> GetServerNameWithUdp(IPEndPoint remoteAddress);

    Task<BaseResponse<CommandResponse>> RunServerCommand(string command);

    Task<BaseResponse<object>> Shutdown();

    Task<BaseResponse<object>> CreateNewGame(string sessionName, string startLocation, bool skipOnboarding);

    Task<BaseResponse<object>> SaveGame(string saveName);

    Task<BaseResponse<object>> DeleteSaveFile(string fileName);

    Task<BaseResponse<object>> DeleteSessionSave(string sessionName);

    Task<BaseResponse<object>> LoadGame(string saveName, bool enableAdvancedFeatures);

    Task<byte[]> DownloadSave(string saveName);

    Task<BaseResponse<object>> UploadSave(byte[] data, string saveName, bool load, bool enableAdvancedSettings);
}
