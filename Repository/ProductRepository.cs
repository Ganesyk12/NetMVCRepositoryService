using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using new_pages.Models;

namespace new_pages.Repositories
{
    public class ProductRepository : IProductRepository //implement IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<User> GetAllAsync()
        {
            return _context.Users.AsQueryable();
        }

         public async Task<User?> GetByIdAsync(string hdrid)
        {
            return await _context.Users.FirstOrDefaultAsync(p => p.hdrid == hdrid);
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string hdrid)
        {
            var user = await GetByIdAsync(hdrid);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}