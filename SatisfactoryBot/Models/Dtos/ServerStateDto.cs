namespace SatisfactoryBot.Models.Dtos;

using SatisfactoryBot.Services.Api.Models.Responses;

internal class ServerStateDto
{
    public string ServerName { get; set; }

    public StateResponse StateResponse { get; set; }
}
