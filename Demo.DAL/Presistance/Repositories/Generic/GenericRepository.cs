using Demo.DAL.Entities;
using Demo.DAL.Entities.Departments;
using Demo.DAL.Presistance.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Presistance.Repositories.Generic
{
    public class GenericRepository<T> : IGenericRepository<T> where T : ModelBase
    {
        private readonly ApplicationDbContext _dbContext;
        public GenericRepository(ApplicationDbContext dbContext) // Dependency Injection
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<T>> GetAllAsync(bool AsNoTracking = true)
        {
            if (AsNoTracking)
            {
                return await _dbContext.Set<T>().Where(X=> !X.IsDeleted).AsNoTracking().ToListAsync();
            }

            return await _dbContext.Set<T>().Where(X => !X.IsDeleted).ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            //return _dbContext.Departments.Local.FirstOrDefault(d => d.Id == id);
            return await _dbContext.Set<T>().FindAsync(id);//search locally in case found ==> retrun else{send request to database}
        }
        public void AddT(T entity)
        {
            _dbContext.Set<T>().Add(entity);
        }

        public void UpdateT(T entity)
        {
            _dbContext.Set<T>().Update(entity);
        }

        public void DeleteT(T entity)
        {
            entity.IsDeleted = true;
            _dbContext.Set<T>().Update(entity);
        }

        public IQueryable<T> GetAllQueryable()
        {
            return  _dbContext.Set<T>();
        }
    }
}

