using Demo.BLL.Dtos.Departments;
using Demo.BLL.Dtos.Employees;
using Demo.BLL.Services.Employees;
using Demo.DAL.Entities.Common.Enums;
using Demo.DAL.Entities.Departments;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
        public async Task<IActionResult> Index(String SearchValue)
        {
            var employee = await _EmployeeService.GetAllEmployeesAsync(SearchValue);
            return View(employee);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeDto employeeDto)
        {
            if (!ModelState.IsValid)
            {
                return View(employeeDto);
            }
            var message = string.Empty;
            try
            {
                var result = await _EmployeeService.CreateEmployeeAsync(employeeDto);
                //log the result
                _logger.LogInformation("Employee created with ID: {EmployeeId}", result);
                if (result > 0)
                {
                    TempData["message"] = "Employee Created successfully.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    message = "Failed to create Employee.";
                    TempData["message"] = message;
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
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
            {
                return BadRequest();//400
            }
            var employee = await _EmployeeService.GetEmployeeByIdAsync(id.Value);
            if (employee is null)
            {
                return NotFound();//404
            }
            return View(employee);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null)
            {
                return BadRequest();//400
            }
            var employee = await _EmployeeService.GetEmployeeByIdAsync(id.Value);
            if (employee is null)
            {
                return NotFound();//404
            }
            return View(new EmployeeDto()
            {
                Id = employee.Id,
                Name = employee.Name,
                Age = employee.Age,
                Salary = employee.Salary,
                IsActive = employee.IsActive,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                Address = employee.Address,
                HiringDate = employee.HiringDate,
                Gender = Enum.TryParse<Gender>(employee.Gender,out var gender) ? gender : default,
                EmployeeType = Enum.TryParse<EmployeeType>(employee.EmployeeType, out var empType) ? empType : default,
                DepartmentId = employee.DepartmentId,

            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EmployeeDto employeeDto)
        {
            if (!ModelState.IsValid)
            {
                return View(employeeDto);
            }
            var message = string.Empty;
            try
            {
                var result = await _EmployeeService.UpdateEmployeeAsync(employeeDto);
                if (result > 0)
                {
                    TempData["message"] = "Employee Updated successfully.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    
                    message = "Failed to update Employee.";
                    TempData["message"] = message;
                    ModelState.AddModelError(string.Empty, message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                message = _env.IsDevelopment() ? ex.Message : "Employee can not be updated";
            }
            return View(employeeDto);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var message = string.Empty;
            try
            {
                var employee = await _EmployeeService.GetEmployeeByIdAsync(id);
                if (employee is null)
                {
                    return NotFound();//404
                }
                var result = await _EmployeeService.DeleteEmployeeAsync(id);
                if (result)
                {
                    TempData["message"] = "Employee Deleted successfully.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    message = "Failed to delete Employee.";
                    TempData["message"] = message;
                    ModelState.AddModelError(string.Empty, message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                message = _env.IsDevelopment() ? ex.Message : "Employee can not be deleted";
            }
            return View(nameof(Index));
        }

    }
}
