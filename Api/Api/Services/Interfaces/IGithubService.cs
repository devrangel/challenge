using Api.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Services.Interfaces
{
    public interface IGithubService
    {
        Task<List<JsonGithubData>> GetLastFiveCSharpRepos();
    }
}
