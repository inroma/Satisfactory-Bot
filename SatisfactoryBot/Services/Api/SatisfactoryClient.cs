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

    public async Task<BaseResponse<HealthResponse>> GetHealth()
    {
        var body = new BaseRequest<HealthRequest>("HealthCheck");
        var request = new RestRequest().AddBody(body);
        return await client.PostAsync<BaseResponse<HealthResponse>>(request);
    }

    public async Task<BaseResponse<AuthResponse>> PasswordLessLogin()
    {
        var body = new BaseRequest<PasswordLessLoginRequest>("PasswordlessLogin");
        var request = new RestRequest().AddBody(body);
        return await client.PostAsync<BaseResponse<AuthResponse>>(request);
    }

    public async Task<BaseResponse<AuthResponse>> PasswordLogin(string password)
    {
        var body = new BaseRequest<PasswordLoginRequest>("PasswordLogin");
        var request = new RestRequest().AddBody(body);
        return await client.PostAsync<BaseResponse<AuthResponse>>(request);
    }

    public async Task<bool> VerifyAuthenticationToken()
    {
        var request = new RestRequest();
        var result = await client.PostAsync(request);
        // Api returns NoContent on token auth success
        if (result.StatusCode != System.Net.HttpStatusCode.NoContent)
        {
            throw new Exception("Token Auth Error");
        }
        return true;
    }
    
}
