using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using new_pages.Models;

namespace new_pages.Repositories
{
    public class DeptRepository : IDeptRepository
    {
        private readonly ApplicationDbContext _context;

        public DeptRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Dept> GetAllAsync()
        {
            return _context.Depts.AsQueryable();
        }

        public async Task<List<Dept>> GetAllDept(string? searchTerm = null)
        {
            var query = _context.Depts.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(d => d.dept_name.Contains(searchTerm));
            }
            
            return await query.ToListAsync();
        }

        public async Task<Dept?> GetByIdAsync(string deptId)
        {
            return await _context.Depts.FindAsync(deptId);
        }

        // Menambahkan data Role baru
        public async Task AddAsync(Dept dept)
        {
            await _context.Depts.AddAsync(dept);
            await _context.SaveChangesAsync();
        }

         public async Task UpdateAsync(Dept dept)
        {
            _context.Depts.Update(dept);
            await _context.SaveChangesAsync();
        }

        // Menghapus data Role berdasarkan role_id
        public async Task DeleteAsync(string deptId)
        {
            var dept = await _context.Depts.FindAsync(deptId);
            if (dept != null)
            {
                _context.Depts.Remove(dept);
                await _context.SaveChangesAsync();
            }
        }
    }
}