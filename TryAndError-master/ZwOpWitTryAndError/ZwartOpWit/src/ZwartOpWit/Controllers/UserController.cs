using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZwartOpWit.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ZwartOpWit.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDBContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger _logger;

        public UserController   (AppDBContext context,
                                UserManager<User> userManager,
                                ILoggerFactory loggerFactory)
        {
            _context = context;
            _userManager = userManager;
            _logger = loggerFactory.CreateLogger<AccountController>();

        }

        [HttpGet]
        public ViewResult Index()
        {
            UserListVM userListVM = new UserListVM();
            
            userListVM.userList = _context.Users.ToList(); 

            return View(userListVM);
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpGet]
        public ViewResult Read(string id)
        {
            UserVM userVM = new UserVM();
            //userVM.user = _context.Users.FirstOrDefault(x => x.Id == id);

            return View(userVM);
        }

        [HttpGet]
        public ViewResult Update(string id)
        {
            UserVM userVM = new UserVM();
            //User user;
            //userVM.user = _context.Users.FirstOrDefault(x => x.Id == id);
            return View(userVM);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserVM model, string returnUrl = null)
        {
            //if (_context.Users.Any(e => e.UserName == model.UserName))
            //{
            //    ModelState.AddModelError("Username", "Username is already in use.");
                
            //}

            if (ModelState.IsValid)
            {
                var user = new User { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation(3, "User created a new account with password.");
                }
                AddErrors(result);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Update(User user)
        {
            User originalUser;
            UserVM userVM;

            userVM = new UserVM();
            //originalUser = _context.Users.FirstOrDefault(e => e.Id == user.Id);

            ////Check if user that needs to be updated exists
            //if (originalUser == null)
            //{
            //    throw new Exception("No user found with id " + user.Id);
            //}

            ////Validate if another user already uses this username
            //if (_context.Users.Any(e => e.Username == user.Username && e.Username != originalUser.Username))
            //{
            //    ModelState.AddModelError("user.Username", "Username is already in use.");
            //}

            if (!ModelState.IsValid)
            {
             //   userVM.user = user;
                return View(userVM);
            }
            _context.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(string id)
        {
            var original = _context.Users.FirstOrDefault(e => e.Id == id);
            if (original != null)
            {
                _context.Users.Remove(original);
                _context.SaveChanges();
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
