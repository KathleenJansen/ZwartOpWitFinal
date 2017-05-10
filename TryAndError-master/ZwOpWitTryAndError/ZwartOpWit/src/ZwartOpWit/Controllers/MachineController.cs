using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZwartOpWit.Models;
using ZwartOpWit.Models.Viewmodels;
using ZwartOpWit.Helpers;

namespace ZwartOpWit.Controllers
{
    public class MachineController: Controller
    {
        private readonly AppDBContext _context;
        const int PageSize = 3;

        public MachineController(AppDBContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string sortOrder,
                                                 string currentFilter,
                                                 string searchString,
                                                 int? page)
        {
            MachineListVM machineListVm = new MachineListVM();

            machineListVm.currentSort = sortOrder;
            machineListVm.currentFilter = searchString;
            machineListVm.nameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            machineListVm.calculationMethodSortParm = sortOrder == "calcMethod" ? "calcMethod_desc" : "calcMethod";
            machineListVm.typeSortParm = sortOrder == "type" ? "type_desc" : "type";
            machineListVm.departmentSortParm = sortOrder == "department" ? "department_desc" : "department";

            if (searchString != currentFilter)
            {
                page = 1;
            }

            var machines = _context.Machines.Include(m => m.Department).AsQueryable();

            switch (sortOrder)
            {
                case "name_desc":
                    machines = machines.OrderByDescending(m => m.Name);
                    break;
                case "calcMethod":
                    machines = machines.OrderBy(m => m.CalculationMethod);
                    break;
                case "calcMethod_desc":
                    machines = machines.OrderByDescending(m => m.CalculationMethod);
                    break;
                case "type":
                    machines = machines.OrderBy(m => m.Type);
                    break;
                case "type_desc":
                    machines = machines.OrderByDescending(m => m.Type);
                    break;
                case "department":
                    machines = machines.OrderBy(m => m.Department.Name);
                    break;
                case "department_desc":
                    machines = machines.OrderByDescending(m => m.Department.Name);
                    break;
                default:
                    machines = machines.OrderBy(m => m.Name);
                    break;
            }

            machineListVm.machineList = await PaginatedList<Machine>.CreateAsync(machines.AsNoTracking(), page ?? 1, PageSize);

            return View(machineListVm);
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
