using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Paylocity_Assessment.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeRepository _employeeRepository;

        public EmployeeController(EmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        // Add New Employees to Database
        [HttpPost]
        public IActionResult AddEmployee(Employee employee)
        {
            _employeeRepository.AddEmployee(employee);
            return Ok();
        }

        // GET: /Employee/{employeeId}
        [HttpGet("{employeeId}")]
        public ActionResult<Employee> GetEmployee(int employeeId)
        {
            // Retrieve employee from the database
            Employee employee = _employeeRepository.GetEmployeeById(employeeId);

            if (employee == null)
            {
                return NotFound();
            }

            // Calculate benefits cost
            employee.PayrollPreview = BenefitsCalculator.CalculateBenefitsCost(employee);

            return employee;
        }

        // PUT: /Employee/{employeeId}
        [HttpPut("{employeeId}")]
        public IActionResult UpdateEmployee(int employeeId, Employee employee)
        {
            // Retrieve employee from the database
            Employee existingEmployee = _employeeRepository.GetEmployeeById(employeeId);

            if (existingEmployee == null)
            {
                return NotFound();
            }

            // Update employee data
            existingEmployee.Name = employee.Name;
            existingEmployee.Dependents = employee.Dependents;

            // Save changes to the database
            _employeeRepository.UpdateEmployee(existingEmployee);

            // Calculate benefits cost
            existingEmployee.PayrollPreview = BenefitsCalculator.CalculateBenefitsCost(existingEmployee);

            return NoContent();
        }
    }
}
