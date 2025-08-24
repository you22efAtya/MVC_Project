using Demo.DAL.Presistance.Repositories.Departments;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.DAL.Entities.Departments;
using Demo.BLL.Dtos.Departments;
using Demo.DAL.Presistance.UnitOfWork;

namespace Demo.BLL.Services.Departments
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        //private readonly IDepartmentRepository _departmentRepository;

        //public DepartmentService(IDepartmentRepository departmentRepository)
        //{
        //    _departmentRepository = departmentRepository;
        //}
        public DepartmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<DepartmentToReturnDto>> GetAllDepartmentsAsync()
        {
            var departments = await _unitOfWork.DepartmentRepository.GetAllQueryable().Where(D=> !D.IsDeleted).Select(department => new DepartmentToReturnDto
            {
                Id = department.Id,
                Name = department.Name,
                Description = department.Description,
                Code = department.Code,
                CreationDate = department.CreationDate
            }).AsNoTracking().ToListAsync();
            return departments;
        }

        public async Task<DepartmentDetailsToReturnDto?> GetDepartmentByIdAsync(int id)
        {
            var department = await _unitOfWork.DepartmentRepository.GetByIdAsync(id);
            if (department is not null)
            {
                return new DepartmentDetailsToReturnDto
                {
                    Id = department.Id,
                    CreatedBy = department.CreatedBy,
                    CreatedOn = department.CreatedOn,
                    LastModifiedBy = department.LastModifiedBy,
                    LastModifiedOn = department.LastModifiedOn,
                    IsDeleted = department.IsDeleted,
                    Name = department.Name,
                    Description = department.Description,
                    Code = department.Code,
                    CreationDate = department.CreationDate
                };
            }
            return null!;
        }

        public async Task<int> CreateDepartmentAsync(DepartmentToCreateDto department)
        {
            var newDepartment = new Department
            {
                Name = department.Name,
                Description = department.Description,
                Code = department.Code,
                CreationDate = department.CreationDate,
                LastModifiedBy = 1,
                LastModifiedOn = DateTime.UtcNow,
                CreatedBy = 1,
            };
            _unitOfWork.DepartmentRepository.AddT(newDepartment);
            return await _unitOfWork.CompleteAsync();
        }

        public async Task<int> UpdateDepartmentAsync(DepartmentToUpdateDto department)
        {
            var departmentUdated = new Department
            {
                Id = department.Id,
                Name = department.Name,
                Description = department.Description,
                Code = department.Code,
                CreationDate = department.CreationDate,
                LastModifiedBy = 1,
                LastModifiedOn = DateTime.UtcNow,
                CreatedBy = 1,
            };
            _unitOfWork.DepartmentRepository.UpdateT(departmentUdated);
            return await _unitOfWork.CompleteAsync();
        }

        public async Task<bool> DeleteDepartmentAsync(int id)
        {
            var departmentRepo = _unitOfWork.DepartmentRepository;
            var department = await departmentRepo.GetByIdAsync(id);
            if (department is not null)
            {
                 departmentRepo.DeleteT(department);
            }
            return await _unitOfWork.CompleteAsync() > 0;
        }


    }
}
