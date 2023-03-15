using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Paylocity_Assessment.Managers
{
    public static class BenefitsManager
    {

        private static string _connectionString;

        public static void SetConfiguration(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public static decimal PreviewBenefitsCost(int employeeId, string name, List<Dependent> dependents)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Update employee record
                using (SqlCommand command = new SqlCommand("UPDATE Employees SET Name = @Name WHERE EmployeeId = @EmployeeId", connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@EmployeeId", employeeId);
                    command.ExecuteNonQuery();
                }

                // Delete existing dependents for employee
                using (SqlCommand command = new SqlCommand("DELETE FROM Dependents WHERE EmployeeId = @EmployeeId", connection))
                {
                    command.Parameters.AddWithValue("@EmployeeId", employeeId);
                    command.ExecuteNonQuery();
                }

                // Insert new dependents for employee
                foreach (Dependent dependent in dependents)
                {
                    using (SqlCommand command = new SqlCommand("INSERT INTO Dependents (EmployeeId, Name) VALUES (@EmployeeId, @Name)", connection))
                    {
                        command.Parameters.AddWithValue("@EmployeeId", employeeId);
                        command.Parameters.AddWithValue("@Name", dependent.Name);
                        command.ExecuteNonQuery();
                    }
                }

                // Preview benefits cost for employee
                using (SqlCommand command = new SqlCommand("SELECT PayrollPreview FROM Employees WHERE EmployeeId = @EmployeeId", connection))
                {
                    command.Parameters.AddWithValue("@EmployeeId", employeeId);
                    return (decimal)command.ExecuteScalar();
                }
            }
        }
    }
}