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
            TimeRegisterList = new List<TimeRegister>();
            JobLineList = new List<JobLine>();
        } 

        public List<TimeRegister> TimeRegisterList { get; set; }
        public List<JobLine> JobLineList { get; set; }
    }
}
