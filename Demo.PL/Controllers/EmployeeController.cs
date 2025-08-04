using Demo.BLL.Dtos;
using Demo.BLL.Dtos.Departments;
using Demo.BLL.Dtos.Employees;
using Demo.BLL.Services.Departments;
using Demo.BLL.Services.Employees;
using Demo.PL.ViewModels.Departments;
using Microsoft.AspNetCore.Mvc;

namespace Demo.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _EmployeeService;
        private readonly ILogger<EmployeeController> _logger;
        private readonly IWebHostEnvironment _env;

        public EmployeeController(IEmployeeService EmployeeService, ILogger<EmployeeController> logger,IWebHostEnvironment env)
        {
            this._EmployeeService = EmployeeService;
            this._logger = logger;
            this._env = env;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var employee = _EmployeeService.GetAllEmployees();
            return View(employee);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(EmployeeToCreateDto employeeDto)
        {
            if (!ModelState.IsValid)
            {
                return View(employeeDto);
            }
            var message = string.Empty;
            try
            {
                var result = _EmployeeService.CreateEmployee(employeeDto);
                //log the result
                _logger.LogInformation("Employee created with ID: {EmployeeId}", result);
                if (result > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    message = "Failed to create department.";
                    ModelState.AddModelError(string.Empty, message);
                    return View(employeeDto);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                if(_env.IsDevelopment())
                {
                    message = ex.Message;
                    return View(employeeDto);
                }
                else
                {
                    message = "Employee can not be created";
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
            var employee = _EmployeeService.GetEmployeeById(id.Value);
            if (employee is null)
            {
                return NotFound();//404
            }
            return View(employee);
        }

        [HttpGet]
        //public IActionResult Edit(int? id)
        //{
        //    //if (id is null)
        //    //{
        //    //    return BadRequest();//400
        //    //}
        //    //var employee = _EmployeeService.GetEmployeeById(id.Value);
        //    //if (employee is null)
        //    //{
        //    //    return NotFound();//404
        //    //}
        //    //return View(new DepartmentEditViewModel()
        //    //{
        //    //    Name = employee.Name,
        //    //    Description = employee.Description,
        //    //    Code = employee.Code,
        //    //    CreationDate = employee.CreationDate
        //    //});
        //}
        //[HttpPost]
        //public IActionResult Edit(int id,DepartmentEditViewModel departmentVM)
        //{
        //    //if(!ModelState.IsValid)
        //    //{
        //    //    return View(departmentVM);
        //    //}
        //    //var message = string.Empty;
        //    //try
        //    //{
        //    //    var departmentDto = new DepartmentToUpdateDto()
        //    //    {
        //    //        Id = id,
        //    //        Name = departmentVM.Name,
        //    //        Description = departmentVM.Description,
        //    //        Code = departmentVM.Code,
        //    //        CreationDate = departmentVM.CreationDate
        //    //    };
        //    //    var result = _departmentService.UpdateDepartment(departmentDto);
        //    //    if (result > 0)
        //    //    {
        //    //        return RedirectToAction(nameof(Index));
        //    //    }
        //    //    else
        //    //    {
        //    //        message = "Failed to update department.";
        //    //        ModelState.AddModelError(string.Empty, message);
        //    //    }
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    _logger.LogError(ex, ex.Message);
        //    //    message = _env.IsDevelopment() ? ex.Message : "Department can not be updated";
        //    //}
        //    //return View(departmentVM);
        //}
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var message = string.Empty;
            try
            {
                var employee = _EmployeeService.GetEmployeeById(id);
                if (employee is null)
                {
                    return NotFound();//404
                }
                var result = _EmployeeService.DeleteEmployee(id);
                if (result)
                {
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
