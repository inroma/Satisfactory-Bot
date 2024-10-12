namespace SatisfactoryBot.Services.Api.Models.Responses;


public class HealthResponse
{
    /// <summary>
    /// "healthy" or "slow"
    /// </summary>
    public string Health { get; set; }

    public string ServerCustomData { get; set; }
}
