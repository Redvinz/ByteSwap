using System.Collections.Generic;
using System.Threading.Tasks;
using ByteSwapFinalProject.Models;

namespace ByteSwapFinalProject.Services
{
    public interface IUserService
    {
        Task<IEnumerable<Users>> GetUsersAsync();
        Task<Users> GetUserByIdAsync(int id);
        Task AddUserAsync(Users user);
        Task<Users> AuthenticateUserAsync(string username, string password);
        Task<IEnumerable<Contacts>> GetUserContactsAsync(int userId);
        Task<IEnumerable<Products>> GetProductsForUserAsync(int userId);
        Task<IEnumerable<Products>> GetSavedProductsForUserAsync(int userId);
        Task UpdateContactNumberAsync(int userId, string contactNumber);
        Task UpdateEmailAsync(int userId, string email);
        Task UpdateContactMethodAsync(int Id, string Title, string Description);
        Task AddContactMethodAsync(int userId, string Title, string Description);
        Task DeleteContactMethodAsync(int Id);
        Task UpdateNamesAsync(int Id, string fname, string lname);
        Task<Products> GetProductByIdAsync(int productId);
        Task DeleteProductAsync(int productId);
        Task DeleteImagesAsync(int productId);
        Task DeleteSavesAsync(int productId);

        Task RemoveSavedProductAsync(int productId, int userId);
        Task UpdatePasswordAsync(Users user);
        object GetUserById(int value);
    }
}
