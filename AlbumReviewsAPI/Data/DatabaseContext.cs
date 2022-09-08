using AlbumReviewsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AlbumReviewsAPI.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options): base(options)
        {

        }

        public DbSet<AlbumReview> AlbumReviews => Set<AlbumReview>();
    }
}
