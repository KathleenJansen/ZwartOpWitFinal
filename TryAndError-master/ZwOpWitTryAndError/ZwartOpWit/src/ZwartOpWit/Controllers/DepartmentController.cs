using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZwartOpWit.Models;
using ZwartOpWit.Models.Viewmodels;
using ZwartOpWit.Helpers;
using Microsoft.EntityFrameworkCore;

namespace ZwartOpWit.Controllers
{
    public class DepartmentController: Controller
    {
        private readonly AppDBContext _context;
        const int PageSize = 3;

        public DepartmentController(AppDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index( string sortOrder,
                                                string currentFilter,
                                                string searchString,
                                                int? page)
        {
            DepartmentListVM departmentListVM = new DepartmentListVM();
            departmentListVM.currentSort = sortOrder;
            departmentListVM.currentFilter = searchString;
            departmentListVM.nameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            if (searchString != currentFilter)
            {
                page = 1;
            }

            var departments = _context.Departments.AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                departments = departments.Where(s => s.Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    departments = departments.OrderByDescending(u => u.Name);
                    break;
                default:
                    departments = departments.OrderBy(u => u.Name);
                    break;
            }

            departmentListVM.departmentList = await PaginatedList<Department>.CreateAsync(departments.AsNoTracking(), page ?? 1, PageSize);

            return View(departmentListVM);
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Department department)
        {
            _context.Departments.Add(department);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ViewResult Update(int id)
        {
            DepartmentVM departmentVM = new DepartmentVM();
            departmentVM.department = _context.Departments.FirstOrDefault(x => x.Id == id);
            return View(departmentVM);
        }

        [HttpPost]
        public IActionResult Update(Department department)
        {
            _context.Entry(department).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var original = _context.Departments.FirstOrDefault(e => e.Id == id);
            if (original != null)
            {
                _context.Departments.Remove(original);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ViewResult Read(int id)
        {
            DepartmentVM departmentVM = new DepartmentVM();
            departmentVM.department = _context.Departments.FirstOrDefault(x => x.Id == id);

            return View(departmentVM);
        }
    }
}
