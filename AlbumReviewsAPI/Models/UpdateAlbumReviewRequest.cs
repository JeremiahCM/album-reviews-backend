namespace AlbumReviewsAPI.Models
{
    /// <summary>
    /// Deprecated file. Can support a request body for the API DELETE operation
    /// </summary>
    public class UpdateAlbumReviewRequest
    {
        public string ArtistName { get; set; } = string.Empty;
        public string AlbumName { get; set; } = string.Empty;
        public string Review { get; set; } = string.Empty;
    }
}
