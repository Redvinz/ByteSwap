using ByteSwapFinalProject.Models;

namespace ByteSwapFinalProject.Services
{
    public interface IAdminDataServices
    {

        Task<List<Users>> GetUsersAsync();
        Task DeleteUsersAsync(Users users);
        Task<List<Products>> GetProductsAsync();
        Task<Products> GetProductByIdAsync(int productId);

        Task DeleteProductAsync(int productId);
        Task DeleteImagesAsync(int productId);
        Task DeleteSavesAsync(int productId);
        Task DeleteUserAsync(Users user);
        Task DeleteUserAndProductsAsync(Users user);
        Task<Users> GetUserByIdAsync(int userId);
        Task DeleteSavesAndImagesAsync(int productId);
    }
}
