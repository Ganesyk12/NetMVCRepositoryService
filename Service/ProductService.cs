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

    public async Task<(IEnumerable<User> Users, int filteredCount)> GetAllProductsAsync(int skip, int pageSize, string? searchColumn, string? searchValue, string sortColumn, string sortDirection)
    {
        var products = _productRepository.GetAllAsync();
        int totalCount = await products.CountAsync();

        // Filter (jika ada)
        if (!string.IsNullOrEmpty(searchValue) && !string.IsNullOrEmpty(searchColumn))
        {
            products = products.Where(p => EF.Property<string>(p, searchColumn).Contains(searchValue));
        }
        if (!string.IsNullOrEmpty(sortColumn))
        {
            products = sortDirection == "asc"
                ? products.OrderBy(p => EF.Property<object>(p, sortColumn))
                : products.OrderByDescending(p => EF.Property<object>(p, sortColumn));
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

    public async Task<User?> GetByIdAsync(string hdrid)
    {
        return await _productRepository.GetByIdAsync(hdrid);
    }

     public async Task<string> GenerateHdridAsync()
    {
        string prefix = "USR" + DateTime.Now.ToString("YYYYMM");
        // Dapatkan ID maksimum saat ini dari database, Jika belum ada ID, mulai dari 001
        string maxHdrid = await _productRepository.GetMaxHdridStartingWithAsync(prefix);
        if (string.IsNullOrEmpty(maxHdrid))
        {
            return $"{prefix}001";
        }
        // Ambil angka setelah prefix untuk increment
        int currentNumber = int.Parse(maxHdrid.Substring(prefix.Length));
        currentNumber++;
        return $"{prefix}{currentNumber:D3}";
    }

    public async Task<string> GenerateNikAsync()
    {
        string prefix = "DM" + DateTime.Now.ToString("MMdd");
        string maxNik = await _productRepository.GetMaxNikStartingWithAsync(prefix);
        if (string.IsNullOrEmpty(maxNik))
        {
            return $"{prefix}001";
        }
        int currentNumber = int.Parse(maxNik.Substring(prefix.Length));
        currentNumber++;
        return $"{prefix}{currentNumber:D3}";
    }

    public async Task SaveUserDataAsync(User user)
    {
        user.hdrid = await GenerateHdridAsync();
        user.nik = await GenerateNikAsync();
        await _productRepository.AddAsync(user);
    }

    public async Task DeleteAsync(string hdrid)
    {
        await _productRepository.DeleteAsync(hdrid);
    }
}
