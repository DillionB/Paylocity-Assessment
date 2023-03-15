using System;

namespace Paylocity_Assessment
{
    public class BenefitsCalculator
    {
        public static decimal CalculateBenefitsCost(Employee employee)
        {
            decimal totalCost = 1000; // Base cost for employee

            // Add cost for each dependent
            foreach (Dependent dependent in employee.Dependents)
            {
                totalCost += 500;
            }

            // Apply discount if name starts with 'A'
            if (employee.Name.StartsWith("A", StringComparison.InvariantCultureIgnoreCase))
            {
                totalCost *= 0.9m; // 10% discount
                employee.DiscountApplied = true;
            }

            return totalCost;
        }
    }
}