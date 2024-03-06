using ByteSwapFinalProject.Data;
using ByteSwapFinalProject.Models;
using ByteSwapFinalProject.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks; // Import necessary namespaces

namespace ByteSwapFinalProject.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminDataServices _service; // Ensure _service is readonly

        public AdminController(IAdminDataServices service)
        {
            _service = service; // Initialize _service through constructor injection
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            int? authenticatedUserId = HttpContext.Session.GetInt32("AuthenticatedUserId");

            if (!authenticatedUserId.HasValue)
            {
                return RedirectToAction("Login", "Welcome"); // Redirect to login if user is not authenticated
            }

            Users authenticatedUser = await _service.GetUserByIdAsync(authenticatedUserId.Value);
            ViewData["Username"] = authenticatedUser.Username;

            ViewData["LayoutForm"] = 0;
            ViewData["AdminNav.Page"] = 0;

            var users = await _service.GetUsersAsync(); // Use camelCase for variable names
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteUsers(int id)
        {
            int? authenticatedUserId = HttpContext.Session.GetInt32("AuthenticatedUserId");

            if (!authenticatedUserId.HasValue)
            {
                return RedirectToAction("Login", "Welcome"); // Redirect to login if user is not authenticated
            }

            Users authenticatedUser = await _service.GetUserByIdAsync(authenticatedUserId.Value);
            ViewData["Username"] = authenticatedUser.Username;

            ViewData["LayoutForm"] = 0;
            ViewData["AdminNav.Page"] = 0;
            var Users = await _service.GetUsersAsync();
            var User = Users.Where(x => x.Id == id).FirstOrDefault();
            return View(User);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUserById(int id)
        {
            var userToDelete = await _service.GetUserByIdAsync(id);

            if (userToDelete != null)
            {
                // Delete user and associated products
                await _service.DeleteUserAndProductsAsync(userToDelete);
                return RedirectToAction("Index");
            }

            return RedirectToAction("Login", "Welcome");
        }








        public async Task<IActionResult> Products()
        {
            int? authenticatedUserId = HttpContext.Session.GetInt32("AuthenticatedUserId");

            if (!authenticatedUserId.HasValue)
            {
                return RedirectToAction("Login", "Welcome"); // Redirect to login if user is not authenticated
            }

            Users authenticatedUser = await _service.GetUserByIdAsync(authenticatedUserId.Value);
            ViewData["Username"] = authenticatedUser.Username;

            ViewData["LayoutForm"] = 0;
            ViewData["AdminNav.Page"] = 1;
            var products = await _service.GetProductsAsync();
            return View(products); // Pass the list of products to the view
        }

        [HttpGet]
        public async Task<IActionResult> DeleteProducts(int id)
        {
            int? authenticatedUserId = HttpContext.Session.GetInt32("AuthenticatedUserId");

            if (!authenticatedUserId.HasValue)
            {
                return RedirectToAction("Login", "Welcome"); // Redirect to login if user is not authenticated
            }

            Users authenticatedUser = await _service.GetUserByIdAsync(authenticatedUserId.Value);
            ViewData["Username"] = authenticatedUser.Username;
            ViewData["LayoutForm"] = 0;
            ViewData["AdminNav.Page"] = 1;
            var products = await _service.GetProductsAsync();
            var product = products.FirstOrDefault(x => x.Id == id);
            return View(product);
        }

        [HttpPost] // This action performs the actual deletion
        public async Task<IActionResult> DeleteProductConfirmed(Products product)
        {
                var productToDelete = await _service.GetProductByIdAsync(product.Id);

                // Check if the product belongs to the authenticated user
                if (productToDelete != null)
                {
                    // Remove associated Saves entries for the product
                    await _service.DeleteSavesAsync(product.Id);

                    // Perform deletion of images and product
                    await _service.DeleteImagesAsync(product.Id);
                    await _service.DeleteProductAsync(product.Id);

                    return RedirectToAction("Products"); // Redirect to the Listings view after deletion
                }
            

            return RedirectToAction("Login", "Welcome"); // Redirect if not authenticated or product doesn't exist
        }
    }
}
