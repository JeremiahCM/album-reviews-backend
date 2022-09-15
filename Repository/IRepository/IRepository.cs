using Domain.Models;

namespace Repository.IRepository
{
    public interface IRepository <T> where T : AlbumReview
    {
        Task<List<T>> GetAll ();
        Task<T> Get (Guid Id);
        void Insert(T albumReview);
        void Update (T albumReview);
        void Delete (T albumReview);
        void Remove (T albumReview);
        void SaveChanges();
    }
}
