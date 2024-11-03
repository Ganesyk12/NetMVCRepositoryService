using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using new_pages.Models;

namespace new_pages.Repositories
{
    public interface IProductRepository
    {
        IQueryable<User> GetAllAsync();
        Task<User?> GetByIdAsync(string hdrid);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(string hdrid);
    }

    public interface IRoleRepository
    {
        IQueryable<Role> GetAllAsync();
        Task<List<Role>> GetAllRolesAsync();
        Task<Role?> GetByIdAsync(string role_id);
        Task AddAsync(Role role);
        Task UpdateAsync(Role role);
        Task DeleteAsync(string role_id);
    }

    public interface IDeptRepository
    {
        IQueryable<Dept> GetAllAsync();
        Task<List<Dept>> GetAllDept(string? searchTerm = null);
        Task<Dept?> GetByIdAsync(string deptId);
        Task AddAsync(Dept dept);
        Task UpdateAsync(Dept dept);
        Task DeleteAsync(string deptId);
    }
}
