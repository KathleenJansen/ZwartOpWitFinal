using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZwartOpWit.Models
{
    public class User : IdentityUser
    {
        public User()
        {
            Stitches = new List<Stitch>();
        }

    //    public string Username { get; set; }

        public List<Stitch> Stitches { get; set; }
    }
}
