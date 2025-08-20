using Demo.BLL.Dtos.Employees;
using Demo.DAL.Entities.Employees;
using Demo.DAL.Presistance.Repositories.Employees;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Services.Employees
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        public int CreateEmployee(EmployeeDto EmployeeDto)
        {
            Employee employee = new Employee()
            {
                Name = EmployeeDto.Name,
                Age = EmployeeDto.Age,
                Address = EmployeeDto.Address,
                IsActive = EmployeeDto.IsActive,
                Salary = EmployeeDto.Salary,
                Email = EmployeeDto.Email,
                PhoneNumber = EmployeeDto.PhoneNumber,
                HiringDate = EmployeeDto.HiringDate,
                Gender = EmployeeDto.Gender, 
                EmployeeType = EmployeeDto.EmployeeType,
                CreatedBy = 1, 
                LastModifiedBy = 1, 
                LastModifiedOn = DateTime.UtcNow,
                DepartmentId = EmployeeDto.DepartmentId
            };
            return _employeeRepository.AddT(employee);
        }

        public bool DeleteEmployee(int id)
        {
            var employee = _employeeRepository.GetById(id);

            if (employee is not null)
            {
                int rowsAffected = _employeeRepository.DeleteT(employee);
                return rowsAffected > 0;
            }

            return false;
        }

        public IEnumerable<EmployeeToReturnDto> GetAllEmployees()
        {
            return _employeeRepository
            .GetAllQueryable()
            .Include(E => E.Department)
            .Where(E => !E.IsDeleted)
            .Select(employee => new EmployeeToReturnDto
            {
                Id = employee.Id,
                Name = employee.Name,
                Age = employee.Age,
                Email = employee.Email,
                Salary = employee.Salary,
                Gender = employee.Gender.ToString(),
                EmployeeType = employee.EmployeeType.ToString(),
                IsActive = employee.IsActive,
                Department = employee.Department.Name // eager loading // cuz of include
            });


        }

        public EmployeeDetailsToReturnDto? GetEmployeeById(int id)
        {
            var employee = _employeeRepository.GetById(id);

            if (employee is not null)
            {
                return new EmployeeDetailsToReturnDto()
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Age = employee.Age,
                    Email = employee.Email,
                    Salary = employee.Salary,
                    Address = employee.Address,
                    HiringDate = employee.HiringDate,
                    PhoneNumber = employee.PhoneNumber,
                    Gender = employee.Gender.ToString(),
                    EmployeeType = employee.EmployeeType.ToString(),
                    IsActive = employee.IsActive,
                    CreatedBy = employee.CreatedBy,
                    CreatedOn = employee.CreatedOn,
                    Department = employee.Department.Name,// lazy loading
                    DepartmentId = employee.DepartmentId,
                };
            }

            return null!;
        }

        public int UpdateEmployee(EmployeeDto EmployeeDto)
        {
            var employee = new Employee()
            {
                Id = EmployeeDto.Id,
                Name = EmployeeDto.Name,
                Age = EmployeeDto.Age,
                Address = EmployeeDto.Address,
                IsActive = EmployeeDto.IsActive,
                Salary = EmployeeDto.Salary,
                Email = EmployeeDto.Email,
                PhoneNumber = EmployeeDto.PhoneNumber,
                HiringDate = EmployeeDto.HiringDate,
                Gender = EmployeeDto.Gender,
                EmployeeType = EmployeeDto.EmployeeType,
                CreatedBy = 1,
                LastModifiedBy = 1,
                LastModifiedOn = DateTime.UtcNow,
                DepartmentId = EmployeeDto.DepartmentId
            };
            return _employeeRepository.UpdateT(employee);
        }
    }
}
