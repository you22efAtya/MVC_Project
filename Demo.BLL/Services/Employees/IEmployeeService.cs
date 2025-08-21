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
        IEnumerable<EmployeeToReturnDto> GetAllEmployees(string SearchValue);
        EmployeeDetailsToReturnDto? GetEmployeeById(int id);
        int CreateEmployee(EmployeeDto emplyee);
        int UpdateEmployee(EmployeeDto emplyee);
        bool DeleteEmployee(int id);
    }
}
