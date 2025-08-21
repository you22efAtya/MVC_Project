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
        public IEnumerable<DepartmentToReturnDto> GetAllDepartments()
        {
            var departments = _unitOfWork.DepartmentRepository.GetAllQueryable().Where(D=> !D.IsDeleted).Select(department => new DepartmentToReturnDto
            {
                Id = department.Id,
                Name = department.Name,
                Description = department.Description,
                Code = department.Code,
                CreationDate = department.CreationDate
            }).AsNoTracking().ToList();
            return departments;
        }

        public DepartmentDetailsToReturnDto? GetDepartmentById(int id)
        {
            var department = _unitOfWork.DepartmentRepository.GetById(id);
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

        public int CreateDepartment(DepartmentToCreateDto department)
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
            return _unitOfWork.Complete();
        }

        public int UpdateDepartment(DepartmentToUpdateDto department)
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
            return _unitOfWork.Complete();
        }

        public bool DeleteDepartment(int id)
        {
            var departmentRepo = _unitOfWork.DepartmentRepository;
            var department = departmentRepo.GetById(id);
            if (department is not null)
            {
                 departmentRepo.DeleteT(department);
            }
            return _unitOfWork.Complete() > 0;
        }


    }
}
