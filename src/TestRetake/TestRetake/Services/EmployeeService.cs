using TestRetake.Entities;
using TestRetake.Repositories;

namespace TestRetake.Services{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;

        public EmployeeService(IEmployeeRepository employeeRepository, IDepartmentRepository departmentRepository)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _employeeRepository.GetAllAsync();
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await _employeeRepository.GetByIdAsync(id);
        }

        public async Task<int> AddAsync(Employee employee)
        {
            var department = await _departmentRepository.GetByIdAsync(employee.DepID);
            if (department == null)
            {
                throw new ArgumentException("Department not found");
            }

            return await _employeeRepository.AddAsync(employee);
        }

        public async Task<bool> UpdateAsync(Employee employee)
        {
            var department = await _departmentRepository.GetByIdAsync(employee.DepID);
            if (department == null)
            {
                throw new ArgumentException("Department not found");
            }

            return await _employeeRepository.UpdateAsync(employee);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _employeeRepository.DeleteAsync(id);
        }
    }
}
