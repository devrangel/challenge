using System.Text.Json.Serialization;

namespace Api.Entities
{
    public class JsonPublicRepos
    {
        [JsonPropertyName("public_repos")]
        public int PublicRepos { get; set; }
    }
}
