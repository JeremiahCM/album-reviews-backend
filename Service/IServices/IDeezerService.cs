namespace Service.IServices
{
    /// <summary>
    /// Interface for the Deezer Service
    /// </summary>
    public interface IDeezerService
    {
        Task<string?> GetAlbumFromDeezer(string artistName, string albumName);
    }
}
