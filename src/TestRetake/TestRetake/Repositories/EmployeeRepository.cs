using TestRetake.Entities;
using System.Data.SqlClient;

namespace TestRetake.Repositories{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly string _connectionString;

        public EmployeeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            var employees = new List<Employee>();
            const string query = "SELECT * FROM Employees";
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        employees.Add(new Employee
                        {
                            EmpID = reader.GetInt32(0),
                            EmpName = reader.GetString(1),
                            JobName = reader.GetString(2),
                            ManagerID = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3),
                            HireDate = reader.GetDateTime(4),
                            Salary = reader.GetDecimal(5),
                            Commission = reader.IsDBNull(6) ? (decimal?)null : reader.GetDecimal(6),
                            DepID = reader.GetInt32(7)
                        });
                    }
                }
            }
            return employees;
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            Employee? employee = null;
            const string query = "SELECT * FROM Employees WHERE EmpID = @Id";
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        employee = new Employee
                        {
                            EmpID = reader.GetInt32(0),
                            EmpName = reader.GetString(1),
                            JobName = reader.GetString(2),
                            ManagerID = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3),
                            HireDate = reader.GetDateTime(4),
                            Salary = reader.GetDecimal(5),
                            Commission = reader.IsDBNull(6) ? (decimal?)null : reader.GetDecimal(6),
                            DepID = reader.GetInt32(7)
                        };
                    }
                }
            }
            return employee;
        }

        public async Task<int> AddAsync(Employee employee)
        {
            const string query = "INSERT INTO Employees (EmpName, JobName, ManagerID, HireDate, Salary, Commission, DepID) OUTPUT INSERTED.EmpID VALUES (@EmpName, @JobName, @ManagerID, @HireDate, @Salary, @Commission, @DepID)";
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@EmpName", employee.EmpName);
                command.Parameters.AddWithValue("@JobName", employee.JobName);
                command.Parameters.AddWithValue("@ManagerID", employee.ManagerID ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@HireDate", employee.HireDate);
                command.Parameters.AddWithValue("@Salary", employee.Salary);
                command.Parameters.AddWithValue("@Commission", employee.Commission ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@DepID", employee.DepID);
                await connection.OpenAsync();
                return (int)await command.ExecuteScalarAsync();
            }
        }

        public async Task<bool> UpdateAsync(Employee employee)
        {
            const string query = "UPDATE Employees SET EmpName = @EmpName, JobName = @JobName, ManagerID = @ManagerID, Salary = @Salary, Commission = @Commission, DepID = @DepID WHERE EmpID = @EmpID";
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@EmpID", employee.EmpID);
                command.Parameters.AddWithValue("@EmpName", employee.EmpName);
                command.Parameters.AddWithValue("@JobName", employee.JobName);
                command.Parameters.AddWithValue("@ManagerID", employee.ManagerID ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Salary", employee.Salary);
                command.Parameters.AddWithValue("@Commission", employee.Commission ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@DepID", employee.DepID);
                await connection.OpenAsync();
                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string query = "DELETE FROM Employees WHERE EmpID = @Id";
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                await connection.OpenAsync();
                return await command.ExecuteNonQueryAsync() > 0;
            }
        }
    }
}
