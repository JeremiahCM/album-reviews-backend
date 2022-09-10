using Domain.Models;

namespace Repository.IRepository
{
    public interface IRepository <T> where T : AlbumReview
    {
        IEnumerable<T> GetAll ();
        T Get (int Id);
        void Insert(T albumReview);
        void Update (T albumReview);
        void Delete (T albumReview);
        void Remove (T albumReview);
        void SaveChanges();
    }
}
