namespace SatisfactoryBot.Models.Dtos;

using SatisfactoryBot.Services.Api.Models.Responses;

internal class ServerOptionsDto
{
    public string ServerName { get; set; }

    public OptionsResponse OptionsResponse { get; set; }
}
