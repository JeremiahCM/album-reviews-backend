using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.IServices
{
    public interface ICustomService <T> where T: class 
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> Get(Guid id);
        void Insert(T albumReview);
        void Update(T albumReview);
        void Delete(T albumReview);
        void Remove(T albumReview);
    }
}
