using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZwartOpWit.Models.Viewmodels;

namespace ZwartOpWit.Models.Components
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly AppDBContext _context;

        public MenuViewComponent(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            MenuVM indexVM = new MenuVM();

            indexVM.machineList = _context.Machines.ToList();

            return View(indexVM);
        }
    }
}
