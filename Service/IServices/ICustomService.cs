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
        Task<List<T>> GetAll();
        Task<T> Get(Guid Id);
        void Insert(T albumReview);
        void Update(T albumReview);
        void Delete(T albumReview);
        void Remove(T albumReview);
    }
}
