using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Paylocity_Assessment
{
    public class EmployeeRepository
    {
        private readonly string _connectionString;

        public EmployeeRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public List<Employee> GetEmployees()
        {
            List<Employee> employees = new List<Employee>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT EmployeeId, Name, PayrollPreview, DiscountApplied FROM Employees", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Employee employee = new Employee
                            {
                                EmployeeId = (int)reader["EmployeeId"],
                                Name = (string)reader["Name"],
                                PayrollPreview = (decimal)reader["PayrollPreview"],
                                DiscountApplied = (bool)reader["DiscountApplied"],
                                Dependents = new List<Dependent>()
                            };

                            employees.Add(employee);
                        }
                    }
                }

                foreach (Employee employee in employees)
                {
                    using (SqlCommand dependentCommand = new SqlCommand("SELECT DependentId, Name FROM Dependents WHERE EmployeeId = @EmployeeId", connection))
                    {
                        dependentCommand.Parameters.AddWithValue("@EmployeeId", employee.EmployeeId);
                        using (SqlDataReader dependentReader = dependentCommand.ExecuteReader())
                        {
                            while (dependentReader.Read())
                            {
                                employee.Dependents.Add(new Dependent
                                {
                                    DependentId = (int)dependentReader["DependentId"],
                                    Name = (string)dependentReader["Name"],
                                    EmployeeId = employee.EmployeeId
                                });
                            }
                        }
                    }
                }
            }

            return employees;
        }

        public Employee GetEmployeeById(int employeeId)
        {
            Employee employee = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT EmployeeId, Name, PayrollPreview, DiscountApplied FROM Employees WHERE EmployeeId = @EmployeeId", connection))
                {
                    command.Parameters.AddWithValue("@EmployeeId", employeeId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            employee = new Employee
                            {
                                EmployeeId = (int)reader["EmployeeId"],
                                Name = (string)reader["Name"],
                                PayrollPreview = (decimal)reader["PayrollPreview"],
                                DiscountApplied = (bool)reader["DiscountApplied"],
                                Dependents = new List<Dependent>()
                            };
                        }
                    }
                }

                if (employee != null)
                {
                    using (SqlCommand dependentCommand = new SqlCommand("SELECT DependentId, Name FROM Dependents WHERE EmployeeId = @EmployeeId", connection))
                    {
                        dependentCommand.Parameters.AddWithValue("@EmployeeId", employee.EmployeeId);

                        using (SqlDataReader dependentReader = dependentCommand.ExecuteReader())
                        {
                            while (dependentReader.Read())
                            {
                                employee.Dependents.Add(new Dependent
                                {
                                    DependentId = (int)dependentReader["DependentId"],
                                    Name = (string)dependentReader["Name"],
                                    EmployeeId = employee.EmployeeId
                                });
                            }
                        }
                    }
                }
            }

            return employee;
        }

        public void UpdateEmployee(Employee employee)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("UPDATE Employees SET Name = @Name, PayrollPreview = @PayrollPreview, DiscountApplied = @DiscountApplied WHERE EmployeeId = @EmployeeId", connection))
                {
                    command.Parameters.AddWithValue("@EmployeeId", employee.EmployeeId);
                    command.Parameters.AddWithValue("@Name", employee.Name);
                    command.Parameters.AddWithValue("@PayrollPreview", employee.PayrollPreview);
                    command.Parameters.AddWithValue("@DiscountApplied", employee.DiscountApplied);

                    command.ExecuteNonQuery();
                }

                using (SqlCommand deleteDependentsCommand = new SqlCommand("DELETE FROM Dependents WHERE EmployeeId = @EmployeeId", connection))
                {
                    deleteDependentsCommand.Parameters.AddWithValue("@EmployeeId", employee.EmployeeId);
                    deleteDependentsCommand.ExecuteNonQuery();
                }

                foreach (Dependent dependent in employee.Dependents)
                {
                    using (SqlCommand insertDependentCommand = new SqlCommand("INSERT INTO Dependents (EmployeeId, Name) VALUES (@EmployeeId, @Name)", connection))
                    {
                        insertDependentCommand.Parameters.AddWithValue("@EmployeeId", employee.EmployeeId);
                        insertDependentCommand.Parameters.AddWithValue("@Name", dependent.Name);

                        insertDependentCommand.ExecuteNonQuery();
                    }
                }
            }
        }

        public void AddEmployee(Employee employee)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Insert the new employee into the Employees table
                using (SqlCommand command = new SqlCommand("INSERT INTO Employees (Name, PayrollPreview, DiscountApplied) VALUES (@Name, @PayrollPreview, @DiscountApplied)", connection))
                {
                    command.Parameters.AddWithValue("@Name", employee.Name);
                    command.Parameters.AddWithValue("@PayrollPreview", employee.PayrollPreview);
                    command.Parameters.AddWithValue("@DiscountApplied", employee.DiscountApplied);

                    command.ExecuteNonQuery();
                }

                // Get the ID of the newly inserted employee
                int employeeId;
                using (SqlCommand command = new SqlCommand("SELECT SCOPE_IDENTITY()", connection))
                {
                    employeeId = Convert.ToInt32(command.ExecuteScalar());
                }

                // Insert the new employee's dependents into the Dependents table
                foreach (Dependent dependent in employee.Dependents)
                {
                    using (SqlCommand insertDependentCommand = new SqlCommand("INSERT INTO Dependents (EmployeeId, Name) VALUES (@EmployeeId, @Name)", connection))
                    {
                        insertDependentCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
                        insertDependentCommand.Parameters.AddWithValue("@Name", dependent.Name);

                        insertDependentCommand.ExecuteNonQuery();
                    }
                }
            }
        }

        public void RemoveEmployee(int employeeId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand deleteDependentsCommand = new SqlCommand("DELETE FROM Dependents WHERE EmployeeId = @EmployeeId", connection))
                {
                    deleteDependentsCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
                    deleteDependentsCommand.ExecuteNonQuery();
                }

                using (SqlCommand deleteEmployeeCommand = new SqlCommand("DELETE FROM Employees WHERE EmployeeId = @EmployeeId", connection))
                {
                    deleteEmployeeCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
                    deleteEmployeeCommand.ExecuteNonQuery();
                }
            }
        }
    }
}

