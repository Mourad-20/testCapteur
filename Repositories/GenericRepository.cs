using Microsoft.EntityFrameworkCore;
using Model;
using Repositories.IRepo;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ContextProject _dbContext;

        protected GenericRepository(ContextProject context)
        {
            _dbContext = context;
        }
        public async Task<T> GetById(int id)
        {
            try
            {
                var entity = await _dbContext.Set<T>().FindAsync(id);
                if (entity == null)
                {
                    // Handle the case where the entity is not found, if necessary
                    throw new KeyNotFoundException($"Entity with ID {id} not found.");
                }
                return entity;
            }
            catch (Exception e)
            {
                return null;
            }

        }

        public async Task<ICollection<T>> GetAll()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<int> Add(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            return _dbContext.SaveChanges();
        }

        public bool Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            return _dbContext.SaveChanges() == 1;
        }

        public  bool Update(T entity)
        {
            _dbContext.Set<T>().Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
            return _dbContext.SaveChanges()==1;
        }

    }

}
