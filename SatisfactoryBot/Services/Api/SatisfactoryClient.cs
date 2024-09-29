namespace SatisfactoryBot.Services.Api;

using RestSharp;
using RestSharp.Authenticators;
using SatisfactoryBot.Services.Api.Interfaces;
using SatisfactoryBot.Services.Api.Models;
using SatisfactoryBot.Services.Api.Models.Misc;
using SatisfactoryBot.Services.Api.Models.Requests;
using SatisfactoryBot.Services.Api.Models.Responses;

public class SatisfactoryClient : ISatisfactoryClient
{
    #region Private Fields

    readonly RestClient client;

    #endregion Private Fields

    #region Public Constructor

    public SatisfactoryClient(string baseUrl, string token = null)
    {
        if (!baseUrl.ToLower().EndsWith("/api/v1") && !baseUrl.ToLower().EndsWith("/api/v1/"))
        {
            baseUrl += "/api/v1";
        }
        var options = new RestClientOptions(baseUrl)
        {
            RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
        };
        if (!string.IsNullOrEmpty(token))
        {
            options.Authenticator = new JwtAuthenticator(token);
        }
        client = new RestClient(options);
    }

    #endregion Public Constructor

    #region Auth

    public async Task<BaseResponse<AuthResponse>> PasswordLessLogin(ApiPrivilegeLevel apiPrivilege = ApiPrivilegeLevel.Administrator)
    {
        var body = new BaseRequest<PasswordLessLoginRequest>("PasswordlessLogin")
        {
            Data = new() { MinimumPrivilegeLevel = apiPrivilege },
        };
        var request = new RestRequest().AddBody(body);
        var result = await client.PostAsync<BaseResponse<AuthResponse>>(request);
        CheckResponse(result);
        return result;
    }

    public async Task<bool> VerifyAuthenticationToken()
    {
        var body = new BaseRequest<object>("VerifyAuthenticationToken");
        var request = new RestRequest().AddBody(body);
        var result = await client.PostAsync(request);
        // Api returns NoContent on token auth success
        if (result.StatusCode != System.Net.HttpStatusCode.NoContent)
        {
            throw new Exception("Token Auth Error");
        }
        return true;
    }

    #endregion Auth

    #region Public Methods

    public async Task<BaseResponse<AuthResponse>> ClaimServer(string servName, string adminPwd)
    {
        var body = new BaseRequest<ClaimServerRequest>("ClaimServer")
        {
            Data = new()
            {
                ServerName = servName,
                AdminPassword = adminPwd
            }
        };
        var request = new RestRequest().AddBody(body);
        var result = await client.PostAsync<BaseResponse<AuthResponse>>(request);
        CheckResponse(result);
        return result;
    }

    public async Task<BaseResponse<HealthResponse>> GetHealth()
    {
        var body = new BaseRequest<HealthRequest>("HealthCheck");
        var request = new RestRequest().AddBody(body);
        var result = await client.PostAsync<BaseResponse<HealthResponse>>(request);
        CheckResponse(result);
        return result;
    }

    public async Task<BaseResponse<StateResponse>> GetState()
    {
        var body = new BaseRequest<object>("QueryServerState");
        var request = new RestRequest().AddBody(body);
        var result = await client.PostAsync<BaseResponse<StateResponse>>(request);
        CheckResponse(result);
        return result;
    }

    public async Task<BaseResponse<OptionsResponse>> GetOptions()
    {
        var body = new BaseRequest<object>("GetServerOptions");
        var request = new RestRequest().AddBody(body);
        var result = await client.PostAsync<BaseResponse<OptionsResponse>>(request);
        CheckResponse(result);
        return result;
    }

    public async Task<BaseResponse<AdvancedGameSettingsResponse>> GetAdvancedGameSettings()
    {
        var body = new BaseRequest<object>("GetAdvancedGameSettings");
        var request = new RestRequest().AddBody(body);
        var result = await client.PostAsync<BaseResponse<AdvancedGameSettingsResponse>>(request);
        CheckResponse(result);
        return result;
    }

    public async Task<bool> RenameServer(string newName)
    {
        var body = new BaseRequest<RenameRequest>("RenameServer")
        {
            Data = new() { ServerName = newName }
        };
        var request = new RestRequest().AddBody(body);
        var result = await client.PostAsync<BaseResponse<object>>(request);
        CheckResponse(result);
        return true;
    }

    public async Task<bool> UpdateClientPassword(string password)
    {
        var body = new BaseRequest<PasswordRequest>("SetClientPassword")
        {
            Data = new() { Password = password }
        };
        var request = new RestRequest().AddBody(body);
        var result = await client.PostAsync<BaseResponse<object>>(request);
        CheckResponse(result);
        return true;
    }

    public async Task<bool> UpdateAdminPassword(string password, string token)
    {
        var body = new BaseRequest<AdminPasswordRequest>("SetAdminPassword")
        {
            Data = new() { Password = password, AuthenticationToken = token }
        };
        var request = new RestRequest().AddBody(body);
        var result = await client.PostAsync<BaseResponse<object>>(request);
        CheckResponse(result);
        return true;
    }

    #endregion Public Methods

    #region Private Methods

    private static BaseResponse<T> CheckResponse<T>(BaseResponse<T> baseResponse) where T : new()
    {
        if (!string.IsNullOrEmpty(baseResponse?.ErrorCode))
        {
            throw new Exception($"{baseResponse.ErrorCode}. {baseResponse.ErrorMessage}".Trim());
        }
        return baseResponse;
    }

    #endregion Private Methods
}
