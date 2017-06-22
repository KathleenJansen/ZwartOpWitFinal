using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZwartOpWit.Models
{
    public class UserUpdateVM
    {
        public UserUpdateVM()
        { }

        public UserUpdateVM(RoleManager<IdentityRole> _roleManager, UserManager<User> _userManager, User _user)
        {
            
            UserId = _user.Id;
            Email = _user.Email;
            ApplicationRoleId = _roleManager.Roles.SingleOrDefault(r => r.Name == _userManager.GetRolesAsync(_user).Result.Single()).Id;

            ApplicationRoles = _roleManager.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id
            }).ToList();
        }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]

        public List<SelectListItem> ApplicationRoles { get; set; }

        [Display(Name = "Role")]
        public string ApplicationRoleId { get; set; }
        public string UserId { get; set; }
    }
}
