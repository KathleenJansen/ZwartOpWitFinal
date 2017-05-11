using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ZwartOpWit.Models.Viewmodels
{
    public class TimeRegisterVM
    {
        public TimeRegister timeRegister { get; set; }

        public List<SelectListItem> selectJobs { get; set; }

        public List<SelectListItem> selectUsers{ get; set; }
    }
}
