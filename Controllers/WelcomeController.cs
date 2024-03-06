using ByteSwapFinalProject.Models;
using ByteSwapFinalProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace ByteSwap.Controllers
{
    public class WelcomeController : Controller
    {

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Welcome");
        }
        private readonly IUserService _userService;
        public WelcomeController(IUserService userService)
        {
            _userService = userService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            HttpContext.Session.Clear();
            return View();
        }

        public IActionResult Signup()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Signup(Users model) // Replace UsersViewModel with your view model for user registration
        {
            if (ModelState.IsValid)
            {
                if (model.Password != model.ConfirmPassword)
                {
                    ModelState.AddModelError("Password", "The password and confirmation password do not match.");
                    return View(model);
                }

                var user = new Users
                {
                    Firstname = model.Firstname,
                    Lastname = model.Lastname,
                    Email = model.Email,
                    ContactNumber = model.ContactNumber,
                    Username = model.Username,
                    Password = model.Password,
                    Type = UserType.User // You may set this based on your requirements
                };

                await _userService.AddUserAsync(user);

                // Redirect to a success page or login page
                return RedirectToAction("Login"); // Change this as per your route
            }

            // If model state is not valid, return to the Signup page with errors
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            // Implement logic to authenticate the user based on username and password
            Users authenticatedUser = await _userService.AuthenticateUserAsync(username, password);

            if (authenticatedUser != null)
            {
                // Store user ID in session upon successful login
                HttpContext.Session.SetInt32("AuthenticatedUserId", authenticatedUser.Id);

                if (authenticatedUser.Type == UserType.User && authenticatedUser != null) return RedirectToAction("Index", "Home");
                else return RedirectToAction("Index", "Admin");

            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return View();
            }
        }



    }
}
