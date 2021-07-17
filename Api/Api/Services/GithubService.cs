using Api.Entities;
using Api.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Services
{
    public class GithubService : IGithubService
    {
        private readonly IHttpClientFactory _clientFactory;

        public GithubService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        // This method receives a collection as reference and add all C# repositories into it
        private void AddCSharpRepos(ref List<JsonGithubData> outCollection, in List<JsonGithubData> data)
        {
            foreach (var item in data)
            {
                if (item.Language == "C#")
                {
                    outCollection.Add(item);
                }
            }
        }

        // The Github API requires two Headers
        //   Accept: application/vnd.github.v3+json
        //   User-Agent: 
        private HttpRequestMessage CreateGetRequest(string uri)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            request.Headers.Add("Accept", "application/vnd.github.v3+json");
            request.Headers.Add("User-Agent", "ApiChatBotTest");

            return request;
        }

        // It returns the last five C# repositories in ascending order from the URI provided
        //    in the CreateGetRequest method.
        // This method is used especially for the Takenet github.
        // The variable 'numberRepos' receives the total number of repositories from the Takenet github.
        // The Github API provides a pagination of 30 repositories per page. So, it is necessary to 
        //   loop through all pages if it exists. The variable 'pages' holds the number of pages.
        //   1 is add to the expression because of the conversion to int. If the division is less than 1,
        //   the casting of this value will be 0 and has less than 30 repositories, therefore only one page.
        //   If the resulting of the division is for example 3.3, it means that has 3 pages (90 repos)
        //   and 0.3 of a page (9 repos), thus necessary to add 1 to fullfil the number of pages.
        public async Task<List<JsonGithubData>> GetLastFiveCSharpRepos()
        {
            var requestGetNumberRepos = CreateGetRequest("https://api.github.com/users/takenet");

            var client = _clientFactory.CreateClient();

            var responseNumberRepos = await client.SendAsync(requestGetNumberRepos);

            if(responseNumberRepos.IsSuccessStatusCode)
            {
                using var numberRepos = await responseNumberRepos.Content.ReadAsStreamAsync();
                var resultNumberRepos = await JsonSerializer.DeserializeAsync<JsonPublicRepos>(numberRepos);

                int pages = (int)(resultNumberRepos.PublicRepos / 30.0) + 1;

                List<JsonGithubData> collection = new List<JsonGithubData>();

                for (int i = 1; i <= pages; i++)
                {
                    var requestGetAllRepos = CreateGetRequest($"https://api.github.com/users/takenet/repos?page={i}");

                    var responseGetAllRepos = await client.SendAsync(requestGetAllRepos);

                    if (responseGetAllRepos.IsSuccessStatusCode)
                    {
                        using var reposPerPage = await responseGetAllRepos.Content.ReadAsStreamAsync();
                        var resultReposPerPage = await JsonSerializer.DeserializeAsync<List<JsonGithubData>>(reposPerPage);

                        AddCSharpRepos(ref collection, resultReposPerPage);
                    }
                }

                return collection.OrderBy(x => x.CreatedAt).Take(5).ToList();
            }
            else
            {
                return null;
            }
        }
    }
}
