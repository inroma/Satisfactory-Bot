namespace SatisfactoryBot.Services.Api;

using RestSharp;
using RestSharp.Authenticators;
using SatisfactoryBot.Services.Api.Interfaces;
using SatisfactoryBot.Services.Api.Models;
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

    public async Task<BaseResponse<AuthResponse>> PasswordLessLogin()
    {
        var body = new BaseRequest<PasswordLessLoginRequest>("PasswordlessLogin");
        var request = new RestRequest().AddBody(body);
        var result = await client.PostAsync<BaseResponse<AuthResponse>>(request);
        CheckResponse(result);
        return result;
    }

    public async Task<BaseResponse<AuthResponse>> PasswordLogin(string password)
    {
        var body = new BaseRequest<PasswordLoginRequest>("PasswordLogin");
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
        //CheckResponse(result.Data);
        // Api returns NoContent on token auth success
        if (result.StatusCode != System.Net.HttpStatusCode.NoContent)
        {
            throw new Exception("Token Auth Error");
        }
        return true;
    }

    #endregion Auth

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
        if (!string.IsNullOrEmpty(result.ErrorCode))
        {
            throw new Exception($"{result.ErrorCode}{(string.IsNullOrEmpty(result.ErrorMessage) ? Environment.NewLine : "")}{result.ErrorMessage}");
        }
        return result;
    }

    private static BaseResponse<T> CheckResponse<T>(BaseResponse<T> baseResponse) where T : new()
    {
        if (!string.IsNullOrEmpty(baseResponse.ErrorCode))
        {
            throw new Exception($"{baseResponse.ErrorCode}{(string.IsNullOrEmpty(baseResponse.ErrorMessage) ? Environment.NewLine : "")}{baseResponse.ErrorMessage}");
        }
        return baseResponse;
    }
}
