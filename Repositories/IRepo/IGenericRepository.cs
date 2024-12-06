using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepo
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetById(int id);
        Task<ICollection<T>> GetAll();
        Task <int>Add(T entity);
        bool Delete(T entity);
        bool Update(T entity);
    }
}
