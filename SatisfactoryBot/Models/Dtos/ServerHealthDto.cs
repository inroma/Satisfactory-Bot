namespace SatisfactoryBot.Models.Dtos;

using SatisfactoryBot.Services.Api.Models.Responses;

internal class ServerHealthDto
{
    public string ServerName { get; set; }

    public HealthResponse HealthResponse { get; set; }
}
