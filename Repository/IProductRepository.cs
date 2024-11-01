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
}
