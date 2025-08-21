using Demo.DAL.Presistance.Data;
using Demo.DAL.Presistance.Repositories.Departments;
using Demo.DAL.Presistance.Repositories.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Presistance.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dpContext;

        public UnitOfWork(ApplicationDbContext dpContext)
        {
            _dpContext = dpContext;
        }
        public IEmployeeRepository EmployeeRepository => new EmployeeRepository(_dpContext);

        public IDepartmentRepository DepartmentRepository => new DepartmentRepository(_dpContext);

        public int Complete()
        {
            return _dpContext.SaveChanges();
        }

        public void Dispose()
        {
             _dpContext.Dispose();
        }
    }
}
