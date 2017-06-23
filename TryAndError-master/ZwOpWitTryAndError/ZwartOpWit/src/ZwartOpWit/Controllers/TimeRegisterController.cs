using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZwartOpWit.Models;
using ZwartOpWit.Models.Viewmodels;
using ZwartOpWit.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace ZwartOpWit.Controllers
{
	public class TimeRegisterController: Controller
    {
        private readonly AppDBContext _context;
        const int PageSize = 3;

        public TimeRegisterController(AppDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index( string sortOrder,
                                                string currentFilter,
                                                string searchString,
                                                int? page)
        {
            TimeRegistertListVM timeRegisterListVM = new TimeRegistertListVM();
            timeRegisterListVM.currentSort = sortOrder;
            timeRegisterListVM.currentFilter = searchString;
            timeRegisterListVM.jobIdSortParm = String.IsNullOrEmpty(sortOrder) ? "jobId_desc" : "";
            timeRegisterListVM.userNameSortParm = sortOrder == "userName" ? "userName_desc" : "userName";
            timeRegisterListVM.startTimeSortParm = sortOrder == "startTime" ? "startTime_desc" : "startTime";
            timeRegisterListVM.stopTimeSortParm = sortOrder == "stopTime" ? "stopTime_desc" : "stopTime";

            if (searchString != currentFilter)
            {
                page = 1;
            }

            var timeRegisters = _context.TimeRegisters.Include(u => u.User).AsQueryable();
            timeRegisters = timeRegisters.Include(jl => jl.JobLine).AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                timeRegisters = timeRegisters.Where(s => s.User.Email.Contains(searchString));
            }

            switch (sortOrder)
            {

                case "jobId_desc":
                    timeRegisters = timeRegisters.OrderByDescending(u => u.JobLine.JobId);
                    break;
                case "userName":
                    timeRegisters = timeRegisters.OrderBy(m => m.User.Email);
                    break;
                case "userName_desc":
                    timeRegisters = timeRegisters.OrderByDescending(m => m.User.Email);
                    break;
                case "startTime":
                    timeRegisters = timeRegisters.OrderBy(m => m.Start);
                    break;
                case "startTime_desc":
                    timeRegisters = timeRegisters.OrderByDescending(m => m.Start);
                    break;
                case "stopTime":
                    timeRegisters = timeRegisters.OrderBy(m => m.Stop);
                    break;
                case "stopTime_desc":
                    timeRegisters = timeRegisters.OrderByDescending(m => m.Stop);
                    break;
                default:
                    timeRegisters = timeRegisters.OrderBy(u => u.JobLine.JobId);
                    break;
            }

            timeRegisterListVM.timeRegisterList = await PaginatedList<TimeRegister>.CreateAsync(timeRegisters.AsNoTracking(), page ?? 1, PageSize);

            return View(timeRegisterListVM);
        }

        [HttpGet]
        public ViewResult Create()
        {
            TimeRegisterVM timeRegisterVM = new TimeRegisterVM();
            timeRegisterVM.selectJobs = _context.Jobs.Select(x => new SelectListItem() { Text = x.JobNumber, Value = x.Id.ToString() }).ToList();
            timeRegisterVM.selectUsers = _context.Users.Select(x => new SelectListItem() { Text = x.UserName, Value = x.Id.ToString() }).ToList();
            return View(timeRegisterVM);
        }

        [HttpPost]
        public IActionResult Create(TimeRegister timeRegister)
        {
            _context.TimeRegisters.Add(timeRegister);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ViewResult Update(int id)
        {
            TimeRegisterVM timeRegisterVM = new TimeRegisterVM();
            timeRegisterVM.timeRegister = _context.TimeRegisters.Include(jl => jl.JobLine).Include(u => u.User).FirstOrDefault(x => x.Id == id);
            timeRegisterVM.selectJobs = _context.Jobs.Select(x => new SelectListItem() { Text = x.JobNumber, Value = x.Id.ToString() }).ToList();
            timeRegisterVM.selectUsers = _context.Users.Select(x => new SelectListItem() { Text = x.UserName, Value = x.Id.ToString() }).ToList();
            return View(timeRegisterVM);
        }

        [HttpPost]
        public IActionResult Update(TimeRegister timeRegister)
        {
			timeRegister.UserId = timeRegister.User.Id;
            _context.Entry(timeRegister).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var original = _context.TimeRegisters.FirstOrDefault(e => e.Id == id);
            if (original != null)
            {
                _context.TimeRegisters.Remove(original);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ViewResult Read(int id)
        {
            TimeRegisterVM timeRegisterVM = new TimeRegisterVM();
            timeRegisterVM.timeRegister = _context.TimeRegisters.Include(jl => jl.JobLine).Include(u => u.User).FirstOrDefault(x => x.Id == id);
            return View(timeRegisterVM);
        }
    }
}
