using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class AlbumReview
    {
        public Guid Id { get; set; }
        public string ArtistName { get; set; } = string.Empty;
        public string AlbumName { get; set; } = string.Empty;
        public string ReleaseDate { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public int NumTracks { get; set; } = 0; 
        public string Review { get; set; } = string.Empty;
    }
}
