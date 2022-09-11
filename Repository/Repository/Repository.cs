using Domain.Data;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;

namespace Repository.Repository
{
    public class Repository <T> : IRepository <T> where T : AlbumReview
    {
        private readonly DatabaseContext dbContext;
        private DbSet<T> albumReviews;

        public Repository(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
            this.albumReviews = this.dbContext.Set<T>();
        }

        public void Delete(T albumReview)
        {
            if (albumReview == null)
            {
                throw new ArgumentNullException("album review");
            }
            albumReviews.Remove(albumReview);
            dbContext.SaveChanges();
        }

        public async Task<T> Get(Guid Id)
        {
            return await albumReviews.FirstAsync(ar => ar.Id.Equals(Id));
        }

        public async Task<List<T>> GetAll()
        {
            return await albumReviews.ToListAsync();
        }

        public void Insert(T albumReview)
        {
            if (albumReview == null)
            {
                throw new ArgumentNullException("album review");
            }
            albumReviews.Add(albumReview);
            dbContext.SaveChangesAsync();
        }

        public void Remove(T albumReview)
        {
            if (albumReview == null)
            {
                throw new ArgumentNullException("album review");
            }
            albumReviews.Remove(albumReview);
            dbContext.SaveChangesAsync();
        }

        public void SaveChanges()
        {
            dbContext.SaveChangesAsync();
        }

        public void Update(T albumReview)
        {
            if (albumReview == null)
            {
                throw new ArgumentNullException("album review");
            }
            albumReviews.Update(albumReview);
            dbContext.SaveChangesAsync();
        }
    }
}
