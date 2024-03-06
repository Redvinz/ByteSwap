using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ByteSwapFinalProject.Data;
using ByteSwapFinalProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;

namespace ByteSwapFinalProject.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public UserService(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<IEnumerable<Users>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<Users> GetUserByIdAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task AddUserAsync(Users user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<Users> AuthenticateUserAsync(string username, string password)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
        }

        public async Task<IEnumerable<Contacts>> GetUserContactsAsync(int userId)
        {
            // Assuming you have a DbSet<Contacts> in your AppDbContext named "Contacts"
            return await _context.Contacts.Where(c => c.UsersId == userId).ToListAsync();
        }

        public async Task UpdateContactNumberAsync(int userId, string contactNumber)
        {
            var user = await _context.Users.FindAsync(userId);
            user.ContactNumber = contactNumber.ToString();

            await _context.SaveChangesAsync();
        }

        public async Task UpdateEmailAsync(int userId, string email)
        {
            var user = await _context.Users.FindAsync(userId);
            user.Email = email;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateContactMethodAsync(int Id, string Title, string Description)
        {
            var contact = await _context.Contacts.FindAsync(Id);
            contact.Title = Title;
            contact.Description = Description;

            await _context.SaveChangesAsync();
        }
        public async Task AddContactMethodAsync(int userId, string Title, string Description)
        {
            Contacts contacts = new Contacts();
            contacts.UsersId = userId;
            contacts.Title = Title;
            contacts.Description = Description;

            await _context.Contacts.AddAsync(contacts);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteContactMethodAsync(int Id)
        {
            var contact = await _context.Contacts.FindAsync(Id);
            _context.Contacts.Remove(contact);

            await _context.SaveChangesAsync();
        }


        public async Task<IEnumerable<Products>> GetProductsForUserAsync(int userId)
        {
            // Assuming "UserId" is the field in Products that refers to the user's ID
            return await _context.Products.Where(p => p.UsersId == userId).Include(p => p.ProductImages).ToListAsync();
        }

        public async Task<IEnumerable<Products>> GetSavedProductsForUserAsync(int userId)
        {
            // Assuming you have a relationship between Users and Saves
            var savedProductIds = await _context.Saves
                .Include(s => s.Product.ProductImages)
                .Where(s => s.UsersId == userId)
                .Select(s => s.ProductsId)
                .ToListAsync();

            return await _context.Products

                .Include(p => p.ProductImages)
                .Where(p => savedProductIds.Contains(p.Id))
                .ToListAsync();
        }

        //used for deleting products in listing view
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


        //used for profile page
        public async Task RemoveSavedProductAsync(int productId, int userId)
        {
            // Retrieve the saved product for the user
            var savedProduct = await _context.Saves.FirstOrDefaultAsync(s => s.UsersId == userId && s.ProductsId == productId);

            if (savedProduct != null)
            {
                // Remove the saved product entry
                _context.Saves.Remove(savedProduct);
                await _context.SaveChangesAsync();
            }
        }

        public object GetUserById(int value)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateNamesAsync(int Id, string fname, string lname)
        {
            var user = await _context.Users.FindAsync(Id);
            user.Firstname = fname;
            user.Lastname = lname;
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePasswordAsync(Users user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

    }
}
