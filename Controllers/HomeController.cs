using ByteSwapFinalProject.Data;
using ByteSwapFinalProject.Models;
using ByteSwapFinalProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using System;

namespace ByteSwapFinalProject.Controllers
{

    public class HomeController : Controller
    {
        private IHomeDataService _service;
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _environment;

        public HomeController(IHomeDataService service, AppDbContext dbContext, IWebHostEnvironment environment)
        {
            _service = service;
            _dbContext = dbContext;
            _environment = environment;
            ViewBag.display = "test";
        }


        //Index.cshtml
        public async Task<IActionResult> Index(string category = null, string searchString = null)
        {
            int? authenticatedUserId = HttpContext.Session.GetInt32("AuthenticatedUserId");

            if (!authenticatedUserId.HasValue)
            {
                return RedirectToAction("Login", "Welcome"); // Redirect to login if user is not authenticated
            }

            Users authenticatedUser = await _service.GetUserByIdAsync(authenticatedUserId.Value);
            ViewData["Username"] = authenticatedUser.Username;

            ViewBag.AuthenticatedUserId = authenticatedUserId.Value;

            List<Products> products;

            // If category is provided, filter products by category
            if (!string.IsNullOrEmpty(category))
            {
                products = await _service.GetProductsByCategoryAsync(category);
            }
            // If searchString is provided, filter products by search string
            else if (!string.IsNullOrEmpty(searchString))
            {
                products = await _service.GetProductsBySearchStringAsync(searchString);
            }
            // If no category or search string is provided, get all products
            else
            {
                products = await _service.GetProductsAsync();
            }

            var categories = _service.GetCategories();

            // Pass categories data to ViewData
            ViewData["Categories"] = categories;

            ViewData["HomeDataService"] = _service;


            return View(products);
        }


        //ProductDetails.cshtml
        public async Task<IActionResult> ProductDetails(int id)
        {

            int? authenticatedUserId = HttpContext.Session.GetInt32("AuthenticatedUserId");

            if (!authenticatedUserId.HasValue)
            {
                return RedirectToAction("Login", "Welcome"); // Redirect to login if user is not authenticated
            }

            Users authenticatedUser = await _service.GetUserByIdAsync(authenticatedUserId.Value);
            ViewData["Username"] = authenticatedUser.Username;


            ViewBag.AuthenticatedUserId = authenticatedUserId.Value;

            // Retrieve the product information by ID from the database or service
            var product = await _service.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound(); // Or handle if the product is not found
            }


            var categories = _service.GetCategories();

            // Pass categories data to ViewData
            ViewData["Categories"] = categories;


            ViewData["HomeDataService"] = _service;

            return View(product); // Pass the product details to the view
        }

        public async Task<IActionResult> AddProduct()
        {
            int? authenticatedUserId = HttpContext.Session.GetInt32("AuthenticatedUserId");

            if (!authenticatedUserId.HasValue)
            {
                return RedirectToAction("Login", "Welcome"); // Redirect to login if user is not authenticated
            }

            Users authenticatedUser = await _service.GetUserByIdAsync(authenticatedUserId.Value);
            ViewData["Username"] = authenticatedUser.Username;


            var categories = _service.GetCategories();

            // Pass categories data to ViewData
            ViewData["Categories"] = categories;

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> AddProduct(Products product, List<IFormFile> ProductImages, [FromServices] IWebHostEnvironment environment)
        {
            ModelState.Remove("Users");
            ModelState.Remove("Category");
            ModelState.Remove("Conditions");

            if (ModelState.IsValid)
            {
                int? authenticatedUserId = HttpContext.Session.GetInt32("AuthenticatedUserId");

                if (authenticatedUserId.HasValue)
                {
                    // Associate the authenticated user ID with the product
                    product.UsersId = authenticatedUserId.Value;

                    if (string.IsNullOrWhiteSpace(product.Brand))
                    {
                        product.Brand = "No Brand";
                    }
                    // Add the product using the HomeDataService
                    await _service.AddProductAsync(product);

                    // Handle image uploads
                    foreach (var image in ProductImages)
                    {
                        if (image != null && image.Length > 0)
                        {
                            // Determine the file path and name where you want to save the uploaded image
                            var uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                            var filePath = Path.Combine(_environment.WebRootPath, "Products", uniqueFileName);

                            // Copy the uploaded image to the desired location
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await image.CopyToAsync(stream);
                            }

                            var newImage = new Images
                            {
                                ProductsId = product.Id, // Assign the product ID to associate the image with the product
                                Filename = uniqueFileName // Store the unique filename to avoid conflicts
                            };

                            // Save the image information to the database
                            _dbContext.Images.Add(newImage);
                            await _dbContext.SaveChangesAsync(); // Save changes to the database
                        }
                    }


                    return RedirectToAction("Index");
                }
            }

            // If ModelState is not valid or user is not authenticated, return to the Add view with validation errors
            return View(product);
        }



        public async Task<IActionResult> EditProduct(int id)
        {
            int? authenticatedUserId = HttpContext.Session.GetInt32("AuthenticatedUserId");

            if (!authenticatedUserId.HasValue)
            {
                return RedirectToAction("Login", "Welcome"); // Redirect to login if user is not authenticated
            }

            Users authenticatedUser = await _service.GetUserByIdAsync(authenticatedUserId.Value);
            ViewData["Username"] = authenticatedUser.Username;


            var product = await _service.GetProductByIdAsync(id);


            var categories = _service.GetCategories();

            // Pass categories data to ViewData
            ViewData["Categories"] = categories;

            return View(product);
        }



        [HttpPost]
        public async Task<IActionResult> EditProduct(Products product)
        {
            ModelState.Remove("Users");
            ModelState.Remove("Category");
            ModelState.Remove("Conditions");
            ModelState.Remove("ProductImages");

            if (ModelState.IsValid)
            {
                int? authenticatedUserId = HttpContext.Session.GetInt32("AuthenticatedUserId");

                if (authenticatedUserId.HasValue)
                {
                    var existingProduct = await _service.GetProductByIdAsync(product.Id);

                    if (existingProduct == null)
                    {
                        return NotFound(); // Handle case where the product doesn't exist
                    }

                    existingProduct.Title = product.Title;
                    existingProduct.Price = product.Price;
                    existingProduct.Status = product.Status;
                    existingProduct.Brand = string.IsNullOrWhiteSpace(product.Brand) ? "No Brand" : product.Brand;
                    existingProduct.CategoriesId = product.CategoriesId;
                    existingProduct.ConditionsId = product.ConditionsId;
                    existingProduct.Method = product.Method;
                    existingProduct.Description = product.Description;

                    await _service.UpdateProductAsync(existingProduct);

                    return RedirectToAction("Listings", "Profile");
                }
            }


            // If ModelState is not valid or user is not authenticated, return to the Edit view with validation errors
            return View(product);
        }


        [HttpPost]
        public async Task<IActionResult> ToggleSaveProduct(int productId)
        {
            int? authenticatedUserId = HttpContext.Session.GetInt32("AuthenticatedUserId");

            if (authenticatedUserId.HasValue)
            {
                bool isProductSaved = _service.IsProductSavedForCurrentUser(productId, authenticatedUserId.Value);

                if (isProductSaved)
                {
                    await _service.RemoveProductForUserAsync(productId, authenticatedUserId.Value);
                    TempData["NotificationMessage"] = "Product removed successfully!";
                }
                else
                {
                    var product = await _service.GetProductByIdAsync(productId);

                    if (product != null)
                    {
                        bool isUserOwnProduct = product.UsersId == authenticatedUserId.Value;

                        if (isUserOwnProduct)
                        {
                            TempData["NotificationMessage"] = "You can't save your own product!";
                        }
                        else
                        {
                            await _service.SaveProductForUserAsync(productId, authenticatedUserId.Value);
                            TempData["NotificationMessage"] = "Product saved successfully!";
                        }
                    }
                    else
                    {
                        TempData["NotificationMessage"] = "Product not found!";
                    }
                }

                return RedirectToAction("Index");
            }

            return RedirectToAction("Login", "Welcome");
        }






        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
