using Newtonsoft.Json.Linq;

namespace Service.IServices
{
    /// <summary>
    /// Interface for the Deezer Service
    /// </summary>
    public interface IDeezerService
    {
        Task<JObject?> GetAlbumFromDeezer(string artistName, string albumName);
    }
}
