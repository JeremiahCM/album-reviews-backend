using Newtonsoft.Json.Linq;
using Service.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
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

        public async Task<JObject?> GetAlbumFromDeezer(string artistName, string albumName)
        {
            var idRequest = new HttpRequestMessage(HttpMethod.Get,
                $"search?q=artist:\"{artistName}\"album:\"{albumName}\"");

            HttpResponseMessage idResponse = await _client.SendAsync(idRequest);

            if (idResponse.IsSuccessStatusCode)
            {
                string idContent = await idResponse.Content.ReadAsStringAsync();

                var idJson = JObject.Parse(idContent);

                var id = (int)idJson["data"]![0]!["album"]!["id"]!;

                var detailsRequest = new HttpRequestMessage(HttpMethod.Get,
                    $"album/{id}");

                HttpResponseMessage detailsResponse = await _client.SendAsync(detailsRequest);

                if (detailsResponse.IsSuccessStatusCode)
                {
                    var detailsContent = await detailsResponse.Content.ReadAsStringAsync();
                    var detailsJson = JObject.Parse(detailsContent);

                    return detailsJson;
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
