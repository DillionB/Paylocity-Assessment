using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Paylocity_Assessment.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class PayrollController : ControllerBase
    {
        private readonly EmployeeRepository _employeeRepository;

        public PayrollController(EmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        // GET: /Payroll
        [HttpGet]
        [Produces("application/json")]
        public ActionResult<IEnumerable<Employee>> GetPayroll()
        {
            // Retrieve all employees from the database
            List<Employee> employees = _employeeRepository.GetEmployees();

            // Calculate payroll preview for each employee
            foreach (Employee employee in employees)
            {
                employee.PayrollPreview = BenefitsCalculator.CalculateBenefitsCost(employee);
            }

            // Calculate net pay for each employee
            decimal grossPay = 2000m;
            foreach (Employee employee in employees)
            {
                decimal deductions = employee.PayrollPreview / 26m;
                employee.PayrollPreview = grossPay - deductions;
            }

            return employees;
        }
    }
}