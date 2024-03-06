using ByteSwapFinalProject.Data;
using ByteSwapFinalProject.Models;
using ByteSwapFinalProject.Services;
using Microsoft.EntityFrameworkCore;
using System;

public class HomeDataService : IHomeDataService
{
    private readonly AppDbContext _appDbContext;
    private readonly IWebHostEnvironment _environment;

    public HomeDataService(AppDbContext appDbContext, IWebHostEnvironment environment)
    {
        _appDbContext = appDbContext;
        _environment = environment;
    }

    public async Task<Users> GetUserByIdAsync(int id)
    {
        return await _appDbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<List<Products>> GetProductsAsync()
    {
        var products = await _appDbContext.Products
            .Include(p => p.Conditions)
            .Include(p => p.ProductImages) // Include ProductImages related to Products
            .ToListAsync();

        return products;
    }

    public async Task<List<Products>> GetProductsByCategoryAsync(string category)
    {
        var products = await _appDbContext.Products
            .Include(p => p.Conditions)
            .Include(p => p.Category) // Include Category navigation property

            .Include(p => p.ProductImages) // Include ProductImages related to Products
            .Where(p => p.Category.Name == category)

            .ToListAsync();

        return products;
    }

    public async Task<List<Products>> GetProductsBySearchStringAsync(string searchString)
    {
        var products = await _appDbContext.Products
            .Include(p => p.Conditions)
            .Include(p => p.ProductImages) // Include ProductImages related to Products
            .Where(p =>
                p.Title.Contains(searchString) ||
                p.Description.Contains(searchString) ||
                p.Brand.Contains(searchString) ||
                p.Method.Contains(searchString) ||
                p.Description.Contains(searchString)
            // Include other relevant properties for search
            // Example: p.Category.Name.Contains(searchString)
            )
            .ToListAsync();

        return products;
    }
    public List<string> GetCategories()
    {
        var categories = _appDbContext.Categories.OrderBy(c => c.Name).Select(c => c.Name).ToList();
        return categories;
    }

    public async Task AddProductAsync(Products product)
    {
        await _appDbContext.Products.AddAsync(product);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task UpdateProductAsync(Products product)
    {
        _appDbContext.Products.Update(product);
        await _appDbContext.SaveChangesAsync();
    }


    public async Task<Products> GetProductByIdAsync(int productId)
    {
        var product = await _appDbContext.Products
            .Include(p => p.Conditions)
            .Include(p => p.Users) // Include Users related to Products
                .ThenInclude(u => u.UserContacts)
            .Include(p => p.Category)
            .Include(p => p.ProductImages) // Include ProductImages related to Products
            .FirstOrDefaultAsync(p => p.Id == productId);

        return product;
    }


    public async Task<bool> SaveProductForUserAsync(int productId, int userId)
{
    var product = await _appDbContext.Products.FindAsync(productId);

    if (product.UsersId == userId)
    {
        return false; // Indicate that the user cannot save their own product
    }

    var save = new Saves
    {
        ProductsId = productId,
        UsersId = userId
    };

    _appDbContext.Saves.Add(save);
    await _appDbContext.SaveChangesAsync();

    return true; // Indicate that the product was saved successfully
}

    public async Task RemoveProductForUserAsync(int productId, int userId)
    {
        var save = await _appDbContext.Saves.FirstOrDefaultAsync(s => s.ProductsId == productId && s.UsersId == userId);

        if (save != null)
        {
            _appDbContext.Saves.Remove(save);
            await _appDbContext.SaveChangesAsync();
        }
    }

    public bool IsProductSavedForCurrentUser(int productId, int userId)
    {
        // Check if the product is saved for the current user
        return _appDbContext.Saves.Any(s => s.ProductsId == productId && s.UsersId == userId);
    }



}