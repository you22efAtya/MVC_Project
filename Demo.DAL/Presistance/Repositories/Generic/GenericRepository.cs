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
        public IEnumerable<T> GetAll(bool AsNoTracking = true)
        {
            if (AsNoTracking)
            {
                return _dbContext.Set<T>().Where(X=> !X.IsDeleted).AsNoTracking().ToList();
            }

            return _dbContext.Set<T>().Where(X => !X.IsDeleted).ToList();
        }

        public T? GetById(int id)
        {
            //return _dbContext.Departments.Local.FirstOrDefault(d => d.Id == id);
            return _dbContext.Set<T>().Find(id);//search locally in case found ==> retrun else{send request to database}
        }
        public int AddT(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            return _dbContext.SaveChanges();
        }

        public int UpdateT(T entity)
        {
            _dbContext.Set<T>().Update(entity);
            return _dbContext.SaveChanges();
        }

        public int DeleteT(T entity)
        {
            entity.IsDeleted = true;
            _dbContext.Set<T>().Update(entity);
            return _dbContext.SaveChanges();
        }

        public IQueryable<T> GetAllQueryable()
        {
            return _dbContext.Set<T>();
        }
    }
}

