namespace SatisfactoryBot.Services.Api.Interfaces;

using SatisfactoryBot.Services.Api.Models;
using SatisfactoryBot.Services.Api.Models.Responses;
using System.Threading.Tasks;

public interface ISatisfactoryClient
{
    Task<BaseResponse<HealthResponse>> GetHealth();

    Task<BaseResponse<AuthResponse>> PasswordLessLogin();

    Task<bool> VerifyAuthenticationToken();

    Task<BaseResponse<AuthResponse>> PasswordLogin(string password);

    Task<BaseResponse<StateResponse>> GetState();

    Task<BaseResponse<OptionsResponse>> GetOptions();

    Task<BaseResponse<AdvancedGameSettingsResponse>> GetAdvancedGameSettings();
}
