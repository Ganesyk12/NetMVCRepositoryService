using System;

using System.Collections.Generic;
using new_pages.Models;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class ProductService
{
    private readonly ApplicationDbContext _context;

    public ProductService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<object> GetProductsAsync(int skip, int pageSize, string searchValue, string searchColumn, string draw)
    {
        int recordsTotal = await _context.Users.CountAsync();
        var productData = _context.Users.AsQueryable();

        if (!string.IsNullOrEmpty(searchValue)  && !string.IsNullOrEmpty(searchColumn))
        {
            productData =  WhereDynamic(productData, searchColumn, searchValue);
        }

        int recordsFiltered = await productData.CountAsync();

        var data = await productData
            .OrderBy(p => p.hdrid)
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync();
        return new
        {
            draw = draw,
            recordsTotal = recordsTotal,
            recordsFiltered = recordsFiltered,
            data = data
        };
    }

    private IQueryable<T> WhereDynamic<T>(IQueryable<T> source, string searchColumn, string searchValue)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var member = Expression.PropertyOrField(parameter, searchColumn);
        var constant = Expression.Constant(searchValue);
        var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
        var containsExpression = Expression.Call(member, containsMethod, constant);

        var lambda = Expression.Lambda<Func<T, bool>>(containsExpression, parameter);
        return source.Where(lambda);
    }
}
