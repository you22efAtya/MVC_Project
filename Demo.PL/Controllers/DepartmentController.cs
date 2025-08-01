using Demo.BLL.Dtos;
using Demo.BLL.Services.Departments;
using Microsoft.AspNetCore.Mvc;

namespace Demo.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;
        private readonly ILogger<DepartmentController> _logger;
        private readonly IWebHostEnvironment _env;

        public DepartmentController(IDepartmentService departmentService, ILogger<DepartmentController> logger,IWebHostEnvironment env)
        {
            this._departmentService = departmentService;
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
        public IActionResult Create(DepartmentToCreateDto departmentDto)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var message = string.Empty;
            try
            {
                var result = _departmentService.CreateDepartment(departmentDto);
                if(result > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    message = "Failed to create department.";
                    ModelState.AddModelError(string.Empty, message);
                    return View(departmentDto);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                if(_env.IsDevelopment())
                {
                    message = ex.Message;
                    return View(departmentDto);
                }
                else
                {
                    message = "Department can not be created";
                    return View("Error",message);
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

    }
}
