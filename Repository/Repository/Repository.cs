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

        public T Get(int Id)
        {
            return albumReviews.SingleOrDefault(c => c.Id.Equals(Id))!;
        }

        public IEnumerable<T> GetAll()
        {
            return albumReviews.AsEnumerable();
        }

        public void Insert(T albumReview)
        {
            if (albumReview == null)
            {
                throw new ArgumentNullException("album review");
            }
            albumReviews.Add(albumReview);
            dbContext.SaveChanges();
        }

        public void Remove(T albumReview)
        {
            if (albumReview == null)
            {
                throw new ArgumentNullException("album review");
            }
            albumReviews.Remove(albumReview);
        }

        public void SaveChanges()
        {
            dbContext.SaveChanges();
        }

        public void Update(T albumReview)
        {
            if (albumReview == null)
            {
                throw new ArgumentNullException("album review");
            }
            albumReviews.Update(albumReview);
            dbContext.SaveChanges();
        }
    }
}
