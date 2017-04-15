using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZwartOpWit.Models;
using ZwartOpWit.Models.Viewmodels;

namespace ZwartOpWit.Controllers
{
    public class MachineController: Controller
    {
        private readonly AppDBContext _context;

        public MachineController(AppDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            MachineListVM machinelistVM = new MachineListVM();
            machinelistVM.machineList = _context.Machines.Include(m => m.Department).ToList();
            return View(machinelistVM);
        }
        [HttpGet]
        public ViewResult Create()
        {
            MachineVM machineVM = new MachineVM();

            machineVM.selectDepartments = _context.Departments.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }).ToList();

            return View(machineVM);
        }

        [HttpPost]
        public IActionResult Create(Machine machine)
        {
            _context.Machines.Add(machine);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ViewResult Update(int id)
        {
            MachineVM machineVM = new MachineVM();
            machineVM.machine = _context.Machines.FirstOrDefault(x => x.Id == id);
            machineVM.selectDepartments = _context.Departments.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }).ToList();
            return View(machineVM);
        }

        [HttpPost]
        public IActionResult Update(Machine machine)
        {
            _context.Entry(machine).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var original = _context.Machines.FirstOrDefault(e => e.Id == id);
            if (original != null)
            {
                _context.Machines.Remove(original);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ViewResult Read(int id)
        {
            MachineVM machineVM = new MachineVM();
            machineVM.machine = _context.Machines.Include(m => m.Department).FirstOrDefault(x => x.Id == id);
            return View(machineVM);
        }
    }
}
