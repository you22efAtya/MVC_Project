using Demo.BLL.Dtos.Departments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Services.Departments
{
    public interface IDepartmentService
    {
        
        Task<IEnumerable<DepartmentToReturnDto>> GetAllDepartmentsAsync();
        Task<DepartmentDetailsToReturnDto?> GetDepartmentByIdAsync(int id);
        Task<int> CreateDepartmentAsync(DepartmentToCreateDto department);
        Task<int> UpdateDepartmentAsync(DepartmentToUpdateDto department);
        Task<bool> DeleteDepartmentAsync(int id);
    }
}
