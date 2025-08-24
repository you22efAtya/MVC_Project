
using Demo.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Presistance.Repositories.Generic
{
    public interface IGenericRepository<T> where T : ModelBase
    {
        Task<IEnumerable<T>> GetAllAsync(bool AsNoTracking = true);
        IQueryable<T> GetAllQueryable();
        Task<T?> GetByIdAsync(int id);
        void AddT(T entity);
        void UpdateT(T entity);
        void DeleteT(T entity);
    }
}
