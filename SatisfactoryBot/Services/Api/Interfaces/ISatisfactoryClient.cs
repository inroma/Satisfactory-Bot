namespace SatisfactoryBot.Services.Api.Interfaces;

using SatisfactoryBot.Services.Api.Models;
using SatisfactoryBot.Services.Api.Models.Responses;
using System.Threading.Tasks;

public interface ISatisfactoryClient
{
    Task<BaseBody<HealthResponse>> GetHealth();
}
