using Newtonsoft.Json.Linq;

namespace AlbumReviewsAPI.Controllers
{
    /// <summary>
    /// Interface for the Deezer Service
    /// </summary>
    public interface IDeezerService
    {
        Task<string> GetAlbumFromDeezer(string artistName, string albumName);
    }

    /// <summary>
    /// The DeezerService holds the function for calling the Deezer API
    /// It calls twice:
    ///  - First to get the album's Deezer ID from a track on the album
    ///  - Second to use the Deezer ID to get full details about the album
    /// </summary>
    public class DeezerService : IDeezerService
    {
        private HttpClient _client;

        public DeezerService(HttpClient client)
        {
            _client = client;
        }

        public async Task<string?> GetAlbumFromDeezer(string artistName, string albumName)
        {
            var idRequest = new HttpRequestMessage(HttpMethod.Get,
                $"search?q=artist:\"{artistName}\"album:\"{albumName}\"");

            HttpResponseMessage idResponse = await _client.SendAsync(idRequest);

            if (idResponse.IsSuccessStatusCode)
            {
                string idContent = await idResponse.Content.ReadAsStringAsync();

                var idJson = JObject.Parse(idContent);

                var id = (int)idJson["data"][0]["album"]["id"];

                var detailsRequest = new HttpRequestMessage(HttpMethod.Get,
                    $"album/{id}");

                HttpResponseMessage detailsResponse = await _client.SendAsync(detailsRequest);

                if (detailsResponse.IsSuccessStatusCode)
                {
                    return await detailsResponse.Content.ReadAsStringAsync();
                }

                else
                {
                    _ = $"There was an error searching for the album's details in Deezer: {idResponse.ReasonPhrase}";
                }
            }
            else
            {
                _ = $"There was an error searching for the album's Deezer ID: {idResponse.ReasonPhrase}";
            }

            return null;
        }
    }
}
