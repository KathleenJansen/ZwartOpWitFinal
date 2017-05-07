using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZwartOpWit.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ZwartOpWit.Helpers;
using ZwartOpWit.Models.Viewmodels;

namespace ZwartOpWit.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDBContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger _logger;
        const int PageSize = 3;

        public UserController(AppDBContext context,
                                UserManager<User> userManager,
                                RoleManager<IdentityRole> roleManager,
                                ILoggerFactory loggerFactory)
        {
            _context = context;
            _userManager = userManager;
            _logger = loggerFactory.CreateLogger<UserController>();
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index( string sortOrder,
                                                string currentFilter,
                                                string searchString,
                                                int? page)
        {
            UserListVM userListVM = new UserListVM();

            userListVM.CurrentSort      = sortOrder;
            userListVM.CurrentFilter    = searchString;
            userListVM.EmailSortParm    = String.IsNullOrEmpty(sortOrder) ? "email_desc" : "";

            if (searchString != currentFilter)
            {
                page = 1;
            }

            var users = from u in _context.Users
                           select u;

            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(s => s.Email.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "email_desc":
                    users = users.OrderByDescending(u => u.Email);
                    break;
                default:
                    users = users.OrderBy(u => u.Email);
                    break;
            }

            userListVM.userList = await PaginatedList<User>.CreateAsync(users.AsNoTracking(), page ?? 1, PageSize);

            return View(userListVM);
        }

        [HttpGet]
        public ViewResult Create()
        {
            UserVM userVM = new UserVM(_roleManager);

            return View(userVM);
        }

        [HttpGet]
        public async Task<IActionResult> Read(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            UserVM userVM = new UserVM(_roleManager, _userManager, user);

            return View(userVM);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserVM model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                IdentityRole applicationRole;

                var user = new User { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation(3, "User created a new account with password.");

                    applicationRole = await _roleManager.FindByIdAsync(model.ApplicationRoleId);
                    if (applicationRole != null)
                    {
                        IdentityResult roleResult = await _userManager.AddToRoleAsync(user, applicationRole.Name);
                        if (roleResult.Succeeded)
                        {
                            return RedirectToAction("Index");
                        }
                    }

                }
                AddErrors(result);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            UserVM userVM = new UserVM(_roleManager, _userManager, user);

            return View(userVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(string id, UserVM model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(id);
                if (user != null)
                {
                    user.Email = model.Email;
                    string existingRole = _userManager.GetRolesAsync(user).Result.Single();
                    string existingRoleId = _roleManager.Roles.Single(r => r.Name == existingRole).Id;
                    IdentityResult result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        if (existingRoleId != model.ApplicationRoleId)
                        {
                            IdentityResult roleResult = await _userManager.RemoveFromRoleAsync(user, existingRole);
                            if (roleResult.Succeeded)
                            {
                                IdentityRole applicationRole = await _roleManager.FindByIdAsync(model.ApplicationRoleId);
                                if (applicationRole != null)
                                {
                                    IdentityResult newRoleResult = await _userManager.AddToRoleAsync(user, applicationRole.Name);
                                    if (newRoleResult.Succeeded)
                                    {
                                        return RedirectToAction("Index");
                                    }
                                }
                            }
                        }
                    }
                }
            }


            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                User applicationUser = await _userManager.FindByIdAsync(id);
                if (applicationUser != null)
                {
                    IdentityResult result = await _userManager.DeleteAsync(applicationUser);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }

            return RedirectToAction("Index");
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}
