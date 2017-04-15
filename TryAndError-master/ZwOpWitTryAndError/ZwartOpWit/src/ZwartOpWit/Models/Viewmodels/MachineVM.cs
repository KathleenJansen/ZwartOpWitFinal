using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZwartOpWit.Models.Viewmodels
{
    public class MachineVM
    {
        public Machine machine { get; set; }

        public List<SelectListItem> selectDepartments { get; set; }
    }
}
