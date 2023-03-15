namespace Paylocity_Assessment
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public bool DiscountApplied { get; set; }
        public decimal PayrollPreview { get; set; }
        public List<Dependent> Dependents { get; set; }
    }
}