using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZwartOpWit.Models;
using ZwartOpWit.Models.Viewmodels;

namespace ZwartOpWit.Controllers
{
    public class DepartmentController: Controller
    {
        private readonly AppDBContext _context;

        public DepartmentController(AppDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            DepartmentListVM departmentListVM = new DepartmentListVM();
            departmentListVM.departmentList = _context.Departments.ToList();

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
