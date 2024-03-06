using ByteSwapFinalProject.Data;
using ByteSwapFinalProject.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ByteSwapFinalProject.Services
{
    public class AdminDataServices : IAdminDataServices
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public AdminDataServices(AppDbContext appDbContext, IWebHostEnvironment environment)
        {
            _context = appDbContext;
            _environment = environment;
        }



        public async Task<List<Users>> GetUsersAsync()
        {
            var users = await _context.Users.ToListAsync();
            return users;
        }

        public async Task DeleteUsersAsync(Users users)
        {
            var selectedUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == users.Id);

            if (selectedUser != null)
            {
                _context.Users.Remove(selectedUser);
                await _context.SaveChangesAsync();
            }
        }



        public async Task<List<Products>> GetProductsAsync()
        {
            var products = await _context.Products.ToListAsync();
            return products;
        }

        public async Task<Products> GetProductByIdAsync(int productId)
        {
            var product = await _context.Products
                .Include(p => p.Conditions)
                .Include(p => p.Users) // Include Users related to Products
                    .ThenInclude(u => u.UserContacts)
                .Include(p => p.Category)
                .Include(p => p.ProductImages) // Include ProductImages related to Products
                .FirstOrDefaultAsync(p => p.Id == productId);

            return product;
        }

        public async Task DeleteSavesAsync(int productId)
        {
            var savesToDelete = _context.Saves.Where(s => s.ProductsId == productId);
            _context.Saves.RemoveRange(savesToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteImagesAsync(int productId)
        {
            var imagesToDelete = await _context.Images.Where(i => i.ProductsId == productId).ToListAsync();

            // Remove associated images and delete their corresponding files
            foreach (var image in imagesToDelete)
            {
                var filePath = Path.Combine(_environment.WebRootPath, "Products", image.Filename);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }

            // Remove images from the database
            _context.Images.RemoveRange(imagesToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int productId)
        {
            var productToDelete = await _context.Products.FindAsync(productId);

            // Remove the product itself
            if (productToDelete != null)
            {
                _context.Products.Remove(productToDelete);
                await _context.SaveChangesAsync();
            }
        }


        public async Task DeleteUserAsync(Users user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAndProductsAsync(Users user)
        {
            var userProductIds = await _context.Products
                .Where(p => p.UsersId == user.Id)
                .Select(p => p.Id)
                .ToListAsync();

            foreach (var productId in userProductIds)
            {
                await DeleteSavesAndImagesAsync(productId);
            }

            var userProducts = await _context.Products.Where(p => p.UsersId == user.Id).ToListAsync();
            _context.Products.RemoveRange(userProducts);
            _context.Users.Remove(user);

            await _context.SaveChangesAsync();
        }


        public async Task DeleteSavesAndImagesAsync(int productId)
        {
            await DeleteSavesAsync(productId);
            await DeleteImagesAsync(productId);
        }


        public async Task<Users> GetUserByIdAsync(int userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }


    }
}
