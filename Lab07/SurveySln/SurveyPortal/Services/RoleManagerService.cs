using Microsoft.AspNetCore.Identity;
using SurveyPortal.Models.Identity.Entities;
using System.Threading.Tasks;

namespace SurveyPortal.Services
{
    public class RoleManagerService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleManagerService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<bool> AddUserToAdminRoleAsync(string userIdOrEmail)
        {
            User user = await _userManager.FindByIdAsync(userIdOrEmail);

            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(userIdOrEmail);
            }

            if (user == null)
            {
            }

            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                return true;
            }

            var result = await _userManager.AddToRoleAsync(user, "Admin");
            return result.Succeeded;
        }

        public async Task<bool> RemoveUserFromRoleAsync(string userIdOrEmail, string roleName)
        {
            User user = await _userManager.FindByIdAsync(userIdOrEmail);

            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(userIdOrEmail);
            }

            if (user == null)
            {
                return false;
            }

            if (!await _userManager.IsInRoleAsync(user, roleName))
            {
                return true;
            }

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            return result.Succeeded;
        }
    }
}