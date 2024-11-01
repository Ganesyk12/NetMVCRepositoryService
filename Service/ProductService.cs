using System;

using System.Collections.Generic;
using new_pages.Models;
using new_pages.Repositories;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

public class ProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<(IEnumerable<User> Users, int filteredCount)> GetAllProductsAsync(int skip, int pageSize, string? searchColumn = null, string? searchValue = null)
    {
        var products = _productRepository.GetAllAsync();
        int totalCount = await products.CountAsync();

        // Terapkan filter dinamis jika searchValue diinputkan
        if (!string.IsNullOrEmpty(searchValue) && !string.IsNullOrEmpty(searchColumn))
        {
            // Terapkan filter pencarian
            products = products.Where(p => EF.Property<string>(p, searchColumn).Contains(searchValue));
        }

        // Menghitung total record setelah filter (jika ada)
        var filteredCount = await products.CountAsync();

        var result = await products
            .OrderBy(p => p.hdrid)
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync();
        return (result, filteredCount == 0 ? totalCount : filteredCount);
    }
    public async Task<int> GetTotalRecordsAsync()
    {
        return await _productRepository.GetAllAsync().CountAsync();
    }
}
