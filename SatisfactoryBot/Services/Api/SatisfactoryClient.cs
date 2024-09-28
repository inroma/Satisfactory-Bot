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

    public SatisfactoryClient(string baseUrl, string token)
    {
        if (!baseUrl.ToLower().EndsWith("/api/v1") && !baseUrl.ToLower().EndsWith("/api/v1/"))
        {
            baseUrl += "/api/v1";
        }
        var options = new RestClientOptions(baseUrl)
        {
            Authenticator = new JwtAuthenticator(token),
            RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
        };
        client = new RestClient(options);
    }

    #endregion Public Constructor

    public async Task<BaseBody<HealthResponse>> GetHealth()
    {
        var body = new BaseRequest<HealthRequest>("HealthCheck");
        var request = new RestRequest().AddBody(System.Text.Json.JsonSerializer.Serialize(body));
        return await client.PostAsync<BaseBody<HealthResponse>>(request);
    }

    public async Task<BaseBody<StateResponse>> GetState()
    {
        var body = new BaseRequest<object>("QueryServerState");
        var request = new RestRequest().AddBody(System.Text.Json.JsonSerializer.Serialize(body));
        return await client.PostAsync<BaseBody<StateResponse>>(request);
    }
}
