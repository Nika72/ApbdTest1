using TestRetake.Entities;
using System.Data.SqlClient;

namespace TestRetake.Repositories{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly string _connectionString;

        public DepartmentRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Department>> GetAllAsync()
        {
            var departments = new List<Department>();
            const string query = "SELECT * FROM Department";
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        departments.Add(new Department
                        {
                            DepID = reader.GetInt32(0),
                            DepName = reader.GetString(1),
                            DepLocation = reader.GetString(2)
                        });
                    }
                }
            }
            return departments;
        }

        public async Task<Department?> GetByIdAsync(int id)
        {
            Department? department = null;
            const string query = "SELECT * FROM Department WHERE DepID = @Id";
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        department = new Department
                        {
                            DepID = reader.GetInt32(0),
                            DepName = reader.GetString(1),
                            DepLocation = reader.GetString(2)
                        };
                    }
                }
            }
            return department;
        }

        public async Task<int> AddAsync(Department department)
        {
            const string query = "INSERT INTO Department (DepName, DepLocation) OUTPUT INSERTED.DepID VALUES (@DepName, @DepLocation)";
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@DepName", department.DepName);
                command.Parameters.AddWithValue("@DepLocation", department.DepLocation);
                await connection.OpenAsync();
                return (int)await command.ExecuteScalarAsync();
            }
        }
    }
}
