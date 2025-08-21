using AutoMapper;
using Demo.BLL.Dtos;
using Demo.BLL.Dtos.Departments;
using Demo.BLL.Services.Departments;
using Demo.PL.ViewModels.Departments;
using Microsoft.AspNetCore.Mvc;

namespace Demo.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;
        private readonly IMapper _mapper;
        private readonly ILogger<DepartmentController> _logger;
        private readonly IWebHostEnvironment _env;

        public DepartmentController(IDepartmentService departmentService,IMapper mapper, ILogger<DepartmentController> logger, IWebHostEnvironment env)
        {
            this._departmentService = departmentService;
            this._mapper = mapper;
            this._logger = logger;
            this._env = env;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var departments = _departmentService.GetAllDepartments();
            return View(departments);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DepartmentViewModel departmentVM)
        {
            if (!ModelState.IsValid)
            {
                return View(departmentVM);
            }
            var message = string.Empty;
            try
            {
                var DepartmentToCreate = _mapper.Map<DepartmentToCreateDto>(departmentVM);
                var result = _departmentService.CreateDepartment(DepartmentToCreate);
                if (result > 0)
                {
                    TempData["message"] = "Department created successfully.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    
                    message = "Failed to create department.";
                    TempData["message"] = message;
                    ModelState.AddModelError(string.Empty, message);
                    return View(departmentVM);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                if (_env.IsDevelopment())
                {
                    message = ex.Message;
                    return View(departmentVM);
                }
                else
                {
                    message = "Department can not be created";
                    return View("Error", message);
                }

            }
        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id is null)
            {
                return BadRequest();//400
            }
            var department = _departmentService.GetDepartmentById(id.Value);
            if (department is null)
            {
                return NotFound();//404
            }
            return View(department);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id is null)
            {
                return BadRequest();//400
            }
            var department = _departmentService.GetDepartmentById(id.Value);
            if (department is null)
            {
                return NotFound();//404
            }
            
            return View(new DepartmentViewModel()
            {
                Name = department.Name,
                Description = department.Description,
                Code = department.Code,
                CreationDate = department.CreationDate
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, DepartmentViewModel departmentVM)
        {
            if (!ModelState.IsValid)
            {
                return View(departmentVM);
            }
            var message = string.Empty;
            try
            {
                var departmentDto = new DepartmentToUpdateDto()
                {
                    Id = id,
                    Name = departmentVM.Name,
                    Description = departmentVM.Description,
                    Code = departmentVM.Code,
                    CreationDate = departmentVM.CreationDate
                };
                var result = _departmentService.UpdateDepartment(departmentDto);
                if (result > 0)
                {
                    TempData["message"] = "Department Uodated successfully.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    message = "Failed to update department.";
                    TempData["message"] = message;
                    ModelState.AddModelError(string.Empty, message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                message = _env.IsDevelopment() ? ex.Message : "Department can not be updated";
            }
            return View(departmentVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var message = string.Empty;
            try
            {
                var department = _departmentService.GetDepartmentById(id);
                if (department is null)
                {
                    return NotFound();//404
                }
                var result = _departmentService.DeleteDepartment(id);
                if (result)
                {
                    TempData["message"] = "Department deleted successfully.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    message = "Failed to delete department.";
                    ModelState.AddModelError(string.Empty, message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                message = _env.IsDevelopment() ? ex.Message : "Department can not be deleted";
            }
            return View(nameof(Index));
        }

    }
}
