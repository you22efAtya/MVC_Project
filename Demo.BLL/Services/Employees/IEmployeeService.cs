using Demo.BLL.Dtos.Employees;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Services.Employees
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeToReturnDto>> GetAllEmployeesAsync(string SearchValue);
        Task<EmployeeDetailsToReturnDto?> GetEmployeeByIdAsync(int id);
        Task<int> CreateEmployeeAsync(EmployeeDto emplyee);
        Task<int> UpdateEmployeeAsync(EmployeeDto emplyee);
        Task<bool> DeleteEmployeeAsync(int id);
    }
}
