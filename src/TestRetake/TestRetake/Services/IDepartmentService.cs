using TestRetake.Entities;

namespace TestRetake.Services{
    public interface IDepartmentService
    {
        Task<IEnumerable<Department>> GetAllAsync();
        Task<Department?> GetByIdAsync(int id);
        Task<int> AddAsync(Department department);
    }
}
