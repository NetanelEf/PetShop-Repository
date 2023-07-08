using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NuGet.Protocol.Plugins;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;    
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<IdentityUser> usereManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = usereManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpPost]
        public async Task<IActionResult> Login(SignInModel model)
        {
            if(ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("You have a bunch of errors:");

            foreach (var modelState in ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    sb.AppendLine(error.ErrorMessage);
                }
            }
            TempData["Errors"] = sb.ToString();
            return View("~/Views/Home/LoginSignup.cshtml");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Register(string username, string nickname, string password, string confirmPassword, string email ,string phoneNumber)
        {
            StringBuilder errorSb = new StringBuilder();
            if (ModelState.IsValid)
            {
                if (confirmPassword.Equals(password))
                {
                    var user = await _userManager.FindByNameAsync(username);
                    if (user == null)
                    {
                        IdentityUser newUser = new IdentityUser
                        {
                            UserName = username,
                            NormalizedUserName = nickname,
                            Email = email,
                            PhoneNumber = phoneNumber
                        };

                        var result = await _userManager.CreateAsync(newUser, password);

                        if (_userManager.Users.Count() == 0)
                        {
                            IdentityRole newRole = new IdentityRole();
                            newRole.Name = "Administrator";
                            await _roleManager.CreateAsync(newRole);
                            await _userManager.AddToRoleAsync(newUser,"Administrator");
                        }

                        if (result.Succeeded)
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        return Content("Oh no there was a failure in the process please try again!");
                    }
                    else
                    {
                        errorSb.AppendLine("Username already exists");
                    }
                }
                else
                {
                    errorSb.AppendLine("Passwords do not match");
                }
                
            }
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("You have a bunch of errors:");

            foreach (var modelState in ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    sb.AppendLine(error.ErrorMessage);
                }
            }
            if(errorSb.Length > 0)
                sb.Append(errorSb.ToString());
            TempData["Errors"] = sb.ToString();
            return View("~/Views/Home/LoginSignup.cshtml");
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AddRoleUser(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            await _userManager.AddToRoleAsync(user!, "Administrator");
            return Content("Success! Added Role!");
        }

        [Authorize(Roles = "Administrator")]
        public AdminPageModels GetUsers()
        {
            AdminPageModels model = new AdminPageModels
            {
                IdentityUsers = _userManager.Users,
                Roles = _roleManager.Roles
            };

            foreach( var user in _userManager.Users)
            {
                if (_userManager.IsInRoleAsync(user, "Administrator").Result)
                    ViewBag.Admins += user.UserName;
            } 
            return model;
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public async Task<IActionResult> UpdateUserAuth(string userID)
        {
            var user = await _userManager.FindByIdAsync(userID);
            var isAdmin = _userManager.IsInRoleAsync(user!, "Administrator");
            if (isAdmin.Result)
            {
                await _userManager.RemoveFromRoleAsync(user!, "Administrator");
                return RedirectToAction("UserChangeAuth");
            }
            else
            {
                await _userManager.AddToRoleAsync(user!, "Administrator");
                return RedirectToAction("UserChangeAuth");
            }
        }

        [Authorize(Roles = "Administrator")]

        public IActionResult UserChangeAuth()
        {
            return View(GetUsers());
        }

        [HttpGet]
        public async void ConfirmEmail(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            user!.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);
        }
    }
}
