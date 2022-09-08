using AlbumReviewsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AlbumReviewsAPI.Data
{
    /// <summary>
    /// Allows for an in-memory database to be created and used for the API
    /// </summary>
    public class AlbumReviewsAPIDbContext : DbContext
    {
        public AlbumReviewsAPIDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<AlbumReview> AlbumReviews { get; set; }
    }
}
