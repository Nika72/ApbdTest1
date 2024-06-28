using TestRetake.Entities;

namespace TestRetake.Repositories{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<Department>> GetAllAsync();
        Task<Department?> GetByIdAsync(int id);
        Task<int> AddAsync(Department department);
    }
}
