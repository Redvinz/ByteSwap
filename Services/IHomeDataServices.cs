using ByteSwapFinalProject.Models;

namespace ByteSwapFinalProject.Services
{
    public interface IHomeDataService
    {
        Task<Users> GetUserByIdAsync(int id);
        Task AddProductAsync(Products product);
        Task UpdateProductAsync(Products product);
        Task<List<Products>> GetProductsAsync();
        Task<List<Products>> GetProductsByCategoryAsync(string category);
        Task<List<Products>> GetProductsBySearchStringAsync(string searchString);
        List<string> GetCategories();
        Task<Products> GetProductByIdAsync(int productId);
        Task<bool> SaveProductForUserAsync(int productId, int userId);
        Task RemoveProductForUserAsync(int productId, int userId);
        bool IsProductSavedForCurrentUser(int productId, int userId);
    }
}

