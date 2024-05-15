using Microsoft.AspNetCore.Mvc;
using ConferenceSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using ConferenceSystem.data;
using System.Threading.Tasks;

namespace ConferenceSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ConfeSystemData _dbContext;

        public HomeController(ConfeSystemData dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult AccessDenied(string returnUrl)
        {
            
            TempData["ReturnUrl"] = returnUrl;

            

            return RedirectToAction("Login"); 
        }

        [HttpGet]
        public IActionResult Login()
        {
            string returnUrl = TempData["ReturnUrl"]?.ToString();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User user)
        {
            if (!ModelState.IsValid)
            {
                // Find the user by email
                var userFromDb = await _dbContext.User
                    .Include(u => u.UserRole)
                    .ThenInclude(ur => ur.Role)
                    .FirstOrDefaultAsync(u => u.Email == user.Email);

                if (userFromDb != null && BCrypt.Net.BCrypt.Verify(user.Password, userFromDb.Password))
                {
                    var userRoles = userFromDb.UserRole.Select(ur => ur.Role.RoleName).ToList();

                    // Check if the user has the desired role
                    if (userRoles.Contains("Admin"))
                    {
                        // Create claims for the authenticated user
                        var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userFromDb.Email),
                    new Claim(ClaimTypes.NameIdentifier, userFromDb.UserId.ToString()),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        // Sign in the user with the authentication scheme
                        var authenticationProperties = new AuthenticationProperties
                        {
                            IsPersistent = true
                        };

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                        return RedirectToAction("Index", "EventSeminar"); // Redirect to the admin dashboard or other authorized page
                    }
                    else if (userRoles.Contains("Attendee"))
                    {
                        // Create claims for the authenticated user
                        var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userFromDb.Email),
                    new Claim(ClaimTypes.NameIdentifier, userFromDb.UserId.ToString()),
                    new Claim(ClaimTypes.Role, "Attendee")
                };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        // Sign in the user with the authentication scheme
                        var authenticationProperties = new AuthenticationProperties
                        {
                            IsPersistent = true
                        };

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                        return RedirectToAction("Index", "Registration"); // Redirect to the user dashboard or other authorized page
                    }
                    else
                    {
                        // User doesn't have the necessary role
                        ModelState.AddModelError("", "You don't have the necessary role to log in.");
                        return View();
                    }
                }
            }

            ModelState.AddModelError("", "Invalid Username or Password");
            return View(user);
        }

        public async Task<IActionResult> Logout()
        {
            
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

           
            return RedirectToAction("Index", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
