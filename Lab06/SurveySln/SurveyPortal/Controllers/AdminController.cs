using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurveyPortal.Models.Identity;
using SurveyPortal.Models.Identity.Entities;
using System.Threading.Tasks;

namespace SurveyPortal.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppIdentityDbContext _dbContext;

        public AdminController(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            AppIdentityDbContext dbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _dbContext = dbContext;
        }

        public async Task<IActionResult> UserManagement()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }

        public async Task<IActionResult> AddUserToAdminRole(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                TempData["ErrorMessage"] = "Email is required";
                return RedirectToAction(nameof(UserManagement));
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                TempData["ErrorMessage"] = $"User with email {email} not found";
                return RedirectToAction(nameof(UserManagement));
            }

            var roleExists = await _roleManager.RoleExistsAsync("Admin");
            if (!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                TempData["InfoMessage"] = $"User {email} is already in Admin role";
                return RedirectToAction(nameof(UserManagement));
            }

            var result = await _userManager.AddToRoleAsync(user, "Admin");
            if (!result.Succeeded)
            {
                string errors = string.Join(", ", result.Errors.Select(e => e.Description));
                TempData["ErrorMessage"] = $"Failed to add user to Admin role: {errors}";
                return RedirectToAction(nameof(UserManagement));
            }

            TempData["SuccessMessage"] = $"User {email} has been added to Admin role successfully";
            return RedirectToAction(nameof(UserManagement));
        }

        public async Task<IActionResult> AddUserToAdminRoleById(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "User ID is required";
                return RedirectToAction(nameof(UserManagement));
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["ErrorMessage"] = $"User with ID {userId} not found";
                return RedirectToAction(nameof(UserManagement));
            }

            var roleExists = await _roleManager.RoleExistsAsync("Admin");
            if (!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                TempData["InfoMessage"] = $"User {user.Email} is already in Admin role";
                return RedirectToAction(nameof(UserManagement));
            }

            var result = await _userManager.AddToRoleAsync(user, "Admin");
            if (!result.Succeeded)
            {
                string errors = string.Join(", ", result.Errors.Select(e => e.Description));
                TempData["ErrorMessage"] = $"Failed to add user to Admin role: {errors}";
                return RedirectToAction(nameof(UserManagement));
            }

            TempData["SuccessMessage"] = $"User {user.Email} has been added to Admin role successfully";
            return RedirectToAction(nameof(UserManagement));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddToAdminRoleDirect(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                TempData["ErrorMessage"] = "Email is required";
                return RedirectToAction(nameof(UserManagement));
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                TempData["ErrorMessage"] = $"User with email {email} not found";
                return RedirectToAction(nameof(UserManagement));
            }

            var adminRole = await _roleManager.FindByNameAsync("Admin");
            if (adminRole == null)
            {
                adminRole = new IdentityRole("Admin");
                await _roleManager.CreateAsync(adminRole);
            }

            bool isInRole = await _userManager.IsInRoleAsync(user, "Admin");
            if (isInRole)
            {
                TempData["InfoMessage"] = $"User {email} is already in Admin role";
                return RedirectToAction(nameof(UserManagement));
            }

            var result = await _userManager.AddToRoleAsync(user, "Admin");
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = $"User {email} has been added to Admin role successfully";
            }
            else
            {
                string errors = string.Join(", ", result.Errors.Select(e => e.Description));
                TempData["ErrorMessage"] = $"Failed to add user to Admin role: {errors}";
            }

            return RedirectToAction(nameof(UserManagement));
        }
    }
}