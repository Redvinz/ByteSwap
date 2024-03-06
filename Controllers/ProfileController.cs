using ByteSwapFinalProject.Models;
using ByteSwapFinalProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace ByteSwap.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IUserService _userService;

        public ProfileController(IUserService userService)
        {
            _userService = userService;
        }

        

        public async Task<IActionResult> Index() // Profile
        {
            ViewData["ProfileNav.Page"] = 0;
            int? authenticatedUserId = HttpContext.Session.GetInt32("AuthenticatedUserId");

            if (authenticatedUserId.HasValue)
            {
                Users authenticatedUser = await _userService.GetUserByIdAsync(authenticatedUserId.Value);
                ViewData["Username"] = authenticatedUser.Username;

                // Retrieve contacts for the authenticated user
                var userContacts = await _userService.GetUserContactsAsync(authenticatedUserId.Value);

                // Create a view model or add contacts to the existing user model
                // This assumes you have a property named UserContacts in the Users model to hold contacts
                authenticatedUser.UserContacts = userContacts;

                return View(authenticatedUser); // Pass the authenticated user with contacts to the view
            }

            return RedirectToAction("Login", "Welcome"); // Redirect to login if user is not authenticated
        }

        public async Task<IActionResult> Listings()
        {
            ViewData["Title"] = "Listings";
            ViewData["LayoutForm"] = 1;
            ViewData["ProfileNav.Page"] = 1;

            int? authenticatedUserId = HttpContext.Session.GetInt32("AuthenticatedUserId");




            if (authenticatedUserId.HasValue)
            {
                // Assuming GetProductsForUserAsync returns a collection of Products
                var userProducts = await _userService.GetProductsForUserAsync(authenticatedUserId.Value);


                Users authenticatedUser = await _userService.GetUserByIdAsync(authenticatedUserId.Value);

                ViewData["Username"] = authenticatedUser.Username;

                return View(userProducts); // Pass the user's products to the view
            }

            return RedirectToAction("Login", "Welcome"); // Redirect to login if user is not authenticated
        }

        [HttpPost]
        public async Task<IActionResult> AddContact(string ncmTitle, string ncmDescription)
        {
            int? authenticatedUserId = HttpContext.Session.GetInt32("AuthenticatedUserId");

            await _userService.AddContactMethodAsync(authenticatedUserId.Value, ncmTitle, ncmDescription);
            return RedirectToAction("Index");
        }


        [HttpPost] // This action performs the actual deletion
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            int? authenticatedUserId = HttpContext.Session.GetInt32("AuthenticatedUserId");

            if (authenticatedUserId.HasValue)
            {
                var productToDelete = await _userService.GetProductByIdAsync(productId);

                // Check if the product belongs to the authenticated user
                if (productToDelete != null && productToDelete.UsersId == authenticatedUserId)
                {
                    // Remove associated Saves entries for the product
                    await _userService.DeleteSavesAsync(productId);

                    // Perform deletion of images and product
                    await _userService.DeleteImagesAsync(productId);
                    await _userService.DeleteProductAsync(productId);

                    return RedirectToAction("Listings"); // Redirect to the Listings view after deletion
                }
            }

            return RedirectToAction("Login", "Welcome"); // Redirect if not authenticated or product doesn't exist
        }


        [HttpPost]
        public async Task<IActionResult> UpdateContactNumber(Users user)
        {
            int? authenticatedUserId = HttpContext.Session.GetInt32("AuthenticatedUserId");

            await _userService.UpdateContactNumberAsync((int)authenticatedUserId, user.ContactNumber.ToString());

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateEmail(Users user)
        {
            int? authenticatedUserId = HttpContext.Session.GetInt32("AuthenticatedUserId");

            await _userService.UpdateEmailAsync((int)authenticatedUserId, user.Email);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateContactMethod(int cmId, string cmTitle, string cmDescription)
        {
            await _userService.UpdateContactMethodAsync(cmId, cmTitle, cmDescription);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteContactMethod(int cmId)
        {
            await _userService.DeleteContactMethodAsync(cmId);
            return RedirectToAction("Index");
        }

        public IActionResult ChangePassword()
        {
            ViewData["Title"] = "Change Password";
            ViewData["LayoutForm"] = 1;
            ViewData["ProfileNav.Page"] = 3; // You can set a new page index for the Change Password page

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(string password, string confirmPassword)
        {
            int? authenticatedUserId = HttpContext.Session.GetInt32("AuthenticatedUserId");

            if (authenticatedUserId.HasValue)
            {
                var authenticatedUser = await _userService.GetUserByIdAsync(authenticatedUserId.Value);

                // Check if the current password matches the user's actual password
                if (authenticatedUser.Password != password)
                {
                    ModelState.AddModelError("Password", "Current password is incorrect.");
                    return View();
                }

                // Update the user's password to the new password
                authenticatedUser.Password = confirmPassword; // Using confirmPassword here as the new password

                await _userService.UpdatePasswordAsync(authenticatedUser);

                // Redirect to profile or any desired page after successful password change
                return RedirectToAction("Index", "Profile");
            }

            return RedirectToAction("Login", "Welcome"); // Redirect if not authenticated
        }





        public async Task<IActionResult> Saves()
        {
            ViewData["ProfileNav.Page"] = 2;

            int? authenticatedUserId = HttpContext.Session.GetInt32("AuthenticatedUserId");

            if (authenticatedUserId.HasValue)
            {
                var savedProducts = await _userService.GetSavedProductsForUserAsync(authenticatedUserId.Value);

                Users authenticatedUser = await _userService.GetUserByIdAsync(authenticatedUserId.Value);
                ViewData["Username"] = authenticatedUser.Username;
                // Assuming GetSavedProductsForUserAsync retrieves the saved products for the user

                return View(savedProducts); // Pass the saved products/items to the view
            }

            return RedirectToAction("Login", "Welcome");
        }

        [HttpPost]
        public async Task<IActionResult> ToggleUnsaveProduct(int productId)
        {
            int? authenticatedUserId = HttpContext.Session.GetInt32("AuthenticatedUserId");

            if (authenticatedUserId.HasValue)
            {
                // Call a method in your UserService to remove the product from the user's saved products
                await _userService.RemoveSavedProductAsync(productId, authenticatedUserId.Value);

                return RedirectToAction("Saves"); // Redirect back to the Saves view after unsaving the product
            }

            return RedirectToAction("Login", "Welcome"); // Redirect if not authenticated
        }

        [HttpPost]
        public async Task<IActionResult> UpdateNames(int Id, string fname, string lname)
        {
            await _userService.UpdateNamesAsync(Id, fname, lname);

            return RedirectToAction("Index", "Profile");
        }


    }
}
