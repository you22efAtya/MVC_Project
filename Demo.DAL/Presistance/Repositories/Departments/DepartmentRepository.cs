using Demo.DAL.Entities.Departments;
using Demo.DAL.Presistance.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Presistance.Repositories.Departments
{
    //Database <== Repository <== Service <== Controller
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public DepartmentRepository(ApplicationDbContext dbContext) // Dependency Injection
        {
            _dbContext = dbContext;   
        }   
        public IEnumerable<Department> GetAll(bool AsNoTracking = true)
        {
            if (AsNoTracking)
            {
                return _dbContext.Departments.AsNoTracking().ToList();
            }

            return _dbContext.Departments.ToList();
        }

        public Department? GetById(int id)
        {
            //return _dbContext.Departments.Local.FirstOrDefault(d => d.Id == id);
            return _dbContext.Departments.Find(id);//search locally in case found ==> retrun else{send request to database}
        }
        public int AddDepartment(Department department)
        {
            _dbContext.Departments.Add(department);
            return _dbContext.SaveChanges();
        }

        public int UpdateDepartment(Department department)
        {
            _dbContext.Departments.Update(department);
            return _dbContext.SaveChanges();
        }

        public int DeleteDepartment(Department department)
        {
            _dbContext.Departments.Remove(department);
            return _dbContext.SaveChanges();
        }

        public IQueryable<Department> GetAllQueryable()
        {
            return _dbContext.Departments;
        }
    }
}
