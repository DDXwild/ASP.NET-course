using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SurveyPortal.Models.Identity.Entities;
using AutoMapper;
using SurveyPortal.Models.Identity.DTO;
using Microsoft.AspNetCore.Authorization;

namespace SurveyPortal.Controllers
{
    public class AccountsController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public AccountsController(SignInManager<User> signInManager, UserManager<User> userManager, IMapper mapper)
        {
            _signInManager = signInManager;
            _mapper = mapper;
            _userManager = userManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserForLoginDto userModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(userModel.Email, userModel.Password, userModel.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Email or password is incorrect.");
                    return View(userModel);
                }
                    
            }
            return View(userModel);
        }

        public IActionResult VerifyEmail()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> VerifyEmail(EmailVerificationDto userModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(userModel.Email);
                if (user == null)
                {
                    ModelState.AddModelError("Email", "No account found with this email address.");
                    return View(userModel);
                }

                else
                    return RedirectToAction("ChangePassword", "Accounts", new {username = userModel.Email});
            }

            return View(userModel);
        }



        public IActionResult ChangePassword(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return RedirectToAction("VerifyEmail", "Accounts");
            }
            return View(new ChangePasswordDto { Email = userName });
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto userModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(userModel.Email);
                if (user != null)
                {
                    var result = await _userManager.RemovePasswordAsync(user);
                    if (result.Succeeded)
                    {
                        result = await _userManager.AddPasswordAsync(user, userModel.NewPassword);
                        if (result.Succeeded)
                        {
                            return RedirectToAction("Login", "Accounts");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Failed to set new password.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to remove old password.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Email not found!");
                }
            }
            else
            {
                ModelState.AddModelError("", "Invalid input data.");
            }

            return View(userModel);
        }

        [HttpGet]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserForRegistrationDto userModel)
        {
            if (!ModelState.IsValid)
            {
                return View(userModel);
            }

            var existingUser = await _userManager.FindByEmailAsync(userModel.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "Email is already taken.");
                return View(userModel);
            }
            var user = _mapper.Map<User>(userModel);

            user.UserName = userModel.Email;
            user.Address = string.Empty;
            user.City = string.Empty;
            user.State = string.Empty;
            user.PostalCode = string.Empty;
            user.Country = string.Empty;
            user.ProfilePictureUrl = string.Empty;

            var result = await _userManager.CreateAsync(user, userModel.Password);

            if (!result.Succeeded)
            {
                return View(userModel);
            }

            await _userManager.AddToRoleAsync(user, "Visitor");

            return RedirectToAction("Login", "Accounts");
        }


        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            var userProfileDto = _mapper.Map<UserProfileDto>(user);
            return View(userProfileDto);
        }


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(UserProfileDto model, IFormFile profilePicture)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;
            user.DateOfBirth = model.DateOfBirth;
            user.Address = model.Address;
            user.City = model.City;
            user.State = model.State;
            user.PostalCode = model.PostalCode;
            user.Country = model.Country;

            if (user.Email != model.Email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, model.Email);
                if (!setEmailResult.Succeeded)
                {
                    foreach (var error in setEmailResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(model);
                }
                var setUsernameResult = await _userManager.SetUserNameAsync(user, model.Email);
                if (!setUsernameResult.Succeeded)
                {
                    foreach (var error in setUsernameResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(model);
                }
            }

            if (profilePicture != null && profilePicture.Length > 0)
            {
                try
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var fileExtension = Path.GetExtension(profilePicture.FileName).ToLowerInvariant();

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        ModelState.AddModelError("ProfilePicture", "Only image files (jpg, jpeg, png, gif) are allowed.");
                        return View(model);
                    }

                    if (profilePicture.Length > 5 * 1024 * 1024)
                    {
                        ModelState.AddModelError("ProfilePicture", "File size cannot exceed 5MB.");
                        return View(model);
                    }

                    var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "profiles");
                    if (!Directory.Exists(uploadsDir))
                    {
                        Directory.CreateDirectory(uploadsDir);
                    }

                    var fileName = $"{user.Id}_{Guid.NewGuid()}{fileExtension}";
                    var filePath = Path.Combine(uploadsDir, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await profilePicture.CopyToAsync(stream);
                    }

                    user.ProfilePictureUrl = $"/uploads/profiles/{fileName}";
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error uploading file: {ex.Message}");
                    return View(model);
                }
            }

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Your profile has been updated successfully!";
                return RedirectToAction("Profile");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
