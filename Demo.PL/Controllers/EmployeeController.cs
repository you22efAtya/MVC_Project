using Demo.BLL.Dtos.Employees;
using Demo.BLL.Services.Employees;
using Demo.DAL.Entities.Common.Enums;
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
        [ValidateAntiForgeryToken]
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
                    message = "Failed to create Employee.";
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
        public IActionResult Edit(int? id)
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
            return View(new EmployeeToUpdateDto()
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
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, EmployeeToUpdateDto employeeDto)
        {
            if (!ModelState.IsValid)
            {
                return View(employeeDto);
            }
            var message = string.Empty;
            try
            {
                var result = _EmployeeService.UpdateEmployee(employeeDto);
                if (result > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    message = "Failed to update Employee.";
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
                    message = "Failed to delete Employee.";
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
