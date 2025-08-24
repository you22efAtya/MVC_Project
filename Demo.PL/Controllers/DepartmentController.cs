using AutoMapper;
using Demo.BLL.Dtos;
using Demo.BLL.Dtos.Departments;
using Demo.BLL.Services.Departments;
using Demo.PL.ViewModels.Departments;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
        public async Task<IActionResult> Index()
        {
            var departments =  await _departmentService.GetAllDepartmentsAsync();
            return View(departments);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DepartmentViewModel departmentVM)
        {
            if (!ModelState.IsValid)
            {
                return View(departmentVM);
            }
            var message = string.Empty;
            try
            {
                var DepartmentToCreate = _mapper.Map<DepartmentToCreateDto>(departmentVM);
                var result = await _departmentService.CreateDepartmentAsync(DepartmentToCreate);
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
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
            {
                return BadRequest();//400
            }
            var department = await _departmentService.GetDepartmentByIdAsync(id.Value);
            if (department is null)
            {
                return NotFound();//404
            }
            return View(department);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null)
            {
                return BadRequest();//400
            }
            var department = await _departmentService.GetDepartmentByIdAsync(id.Value);
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
        public async Task<IActionResult> Edit(int id, DepartmentViewModel departmentVM)
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
                var result = await _departmentService.UpdateDepartmentAsync(departmentDto);
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
        public async Task<IActionResult> Delete(int id)
        {
            var message = string.Empty;
            try
            {
                var department = await _departmentService.GetDepartmentByIdAsync(id);
                if (department is null)
                {
                    return NotFound();//404
                }
                var result = await _departmentService.DeleteDepartmentAsync(id);
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
