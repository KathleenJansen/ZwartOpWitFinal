using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZwartOpWit.Models;
using ZwartOpWit.Models.Viewmodels;
using System.IO;
using Microsoft.AspNetCore.Http;
using ZwartOpWit.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;

namespace ZwartOpWit.Controllers
{
    public class JobController : Controller
    {
        //Constant to determine session filter date time
        const string SessionKeyJobFilterDatetime = "JobFilterDatetime";
        const int PageSize = 10;

        private readonly AppDBContext _context;
        private readonly UserManager<User> _userManager;
        private IHostingEnvironment _environment;

        public JobController(AppDBContext context,
                                UserManager<User> userManager, IHostingEnvironment envivornment)
        {
            _context = context;
            _userManager = userManager;
            _environment = envivornment;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string sortOrder,
                                                string currentFilter,
                                                string searchString,
                                                int? page,
                                                int filterMachineId,
                                                MachineTypes filterMachineType,
                                                DateTime filterDateTime,
                                                bool completed)
        {
            JobListVM jobListVm = new JobListVM();
            filterDateTime = handleJobFilterDateTime(filterDateTime);

            jobListVm.currentSort = sortOrder;
            jobListVm.currentFilter = searchString;
            jobListVm.jobNumberSortParm = String.IsNullOrEmpty(sortOrder) ? "jobNumber_desc" : "";
            jobListVm.quantitySortParm = sortOrder == "quantity" ? "quantity_desc" : "quantity";
            jobListVm.pageQuantitySortParm = sortOrder == "pageQuantity" ? "pageQuantity_desc" : "pageQuantity";

            if (searchString != currentFilter)
            {
                page = 1;
            }

            var joblines = _context.JobLines.Include(j => j.Job).Include(j => j.Machine).AsQueryable();
            var machineList = _context.Machines.AsQueryable();

            //Add filters
            if (!String.IsNullOrEmpty(searchString))
            {
                joblines = joblines.Where(jl => jl.Job.JobNumber.Contains(searchString));
            }

            //Machine type filter is set
            if (System.Enum.IsDefined(typeof(MachineTypes), filterMachineType))
            {
                joblines = joblines.Where(jl => jl.MachineType == filterMachineType);
                machineList = machineList.Where(m => m.Type == filterMachineType);
            }

            //Machine id is set
            if (filterMachineId != 0)
            {
                joblines = joblines.Where(jl => jl.MachineId == filterMachineId);
                machineList = machineList.Where(m => m.Id == filterMachineId);
            }

            //Filter on date & completed
            if (filterDateTime != DateTime.MinValue)
            {
                joblines = joblines.Where(jl => jl.Job.DeliveryDate == filterDateTime && jl.Completed == completed);
            }

            switch (sortOrder)
            {
                case "jobNumber_desc":
                    joblines = joblines.OrderByDescending(u => u.Job.JobNumber);
                    break;
                case "quantity":
                    joblines = joblines.OrderBy(jl => jl.Job.Quantity);
                    break;
                case "quantity_desc":
                    joblines = joblines.OrderByDescending(jl => jl.Job.Quantity);
                    break;

                case "pageQuantity":
                    joblines = joblines.OrderBy(jl => jl.Job.PageQuantity);
                    break;
                case "pageQuantity_desc":
                    joblines = joblines.OrderByDescending(jl => jl.Job.PageQuantity);
                    break;
                default:
                    joblines = joblines.OrderBy(u => u.Job.JobNumber);
                    break;
            }

            jobListVm.machineList = machineList.ToList();
            jobListVm.filterDateTime = filterDateTime.ToString("yyyy-MM-dd");
            jobListVm.jobLineList = await PaginatedList<JobLine>.CreateAsync(joblines.AsNoTracking(), page ?? 1, PageSize);
            jobListVm.totalTime = calculateTotalTime(filterMachineId, filterDateTime);

            return View(jobListVm);
        }


        [HttpGet]
        public async Task<IActionResult> Import(MachineTypes machineType)
        {
            JobImportVM jobImportVM = new JobImportVM();
            jobImportVM.machineType = machineType;

            return View(jobImportVM);
        }

        [HttpPost]
        public async Task<IActionResult> Import(MachineTypes machineType,
                                                    ICollection<IFormFile> files)
        {
            return RedirectToAction("Index");
        }


        public IActionResult AssignJobLine(int jobLineId,
                                            int machineId,
                                            string sortOrder,
                                            string currentFilter,
                                            string searchString,
                                            int? page,
                                            int filterMachineId,
                                            MachineTypes filterMachineType,
                                            DateTime filterDateTime)
        {
            JobLine jobLine;
            Machine machine;

            if (jobLineId != 0 && machineId != 0)
            {
                machine = _context.Machines.FirstOrDefault(m => m.Id == machineId);
                jobLine = _context.JobLines.Include(j => j.Job).FirstOrDefault(e => e.Id == jobLineId);
                jobLine.MachineId = machineId;

                jobLine.calculateTime(machine);

                _context.Entry(jobLine).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
            }

            return RedirectToAction(
                        "Index",
                        new { sortOrder = sortOrder, currentFilter = currentFilter, searchString = searchString, page = page, filterMachineId = filterMachineId, filterMachineType = filterMachineType, filterDateTime = filterDateTime });
        }

        public IActionResult doImport(MachineTypes machineType, IFormFile files)
        {
            var uploads = Path.Combine(_environment.ContentRootPath, "uploads");

            using (var fileStream = new FileStream(Path.Combine(uploads, files.FileName), FileMode.Create))
            {
                files.CopyToAsync(fileStream);
            }

            Job job;
            string currentLine;
            string[] splitArray;
            JobLine jobLine;

            List<string> lines = new List<string>();

            using (StreamReader reader = new StreamReader(new FileStream(Path.Combine(uploads, files.FileName), FileMode.Open)))
            {
                currentLine = reader.ReadLine();

                while (!reader.EndOfStream)
                {
                    currentLine = reader.ReadLine();
                    lines.Add(currentLine);
                }
            }

            foreach (string line in lines)
            {
                splitArray = line.Split(';');

                //Check if job exists with current job number
                job = _context.Jobs.FirstOrDefault(z => z.JobNumber == splitArray[1]);

                //Create Job & JobLine
                if (job == null)
                {
                    //Create Job
                    job = new Job();

                    job.DeliveryDate = DateTime.ParseExact(splitArray[4], "dd/MM/yyyy", null);
                    job.JobNumber = splitArray[1];

                    double aantal = double.Parse(splitArray[2]);
                    job.Quantity = Convert.ToInt16(aantal);

                    job.PaperBw = splitArray[8];
                    job.Cover = 0;
                    job.PaperCover = "no cover";
                    job.Heigth = 297;
                    job.Width = 210;
                    job.PageQuantity = int.Parse(splitArray[6].Remove(2));

                    _context.Jobs.Add(job);

                    //Create JobLine
                    jobLine = new JobLine();

                    jobLine.JobId = job.Id;
                    jobLine.Job = job;
                    jobLine.MachineId = 1;
                    jobLine.Sequence = 1;
                    jobLine.UserId = _userManager.GetUserId(User);
                    //jobLine.UserId = "2aff4902-2ab4-4e25-88fc-4765d661e8f2";
                    jobLine.MachineType = MachineTypes.Stitch;
                    jobLine.DepartmentId = 1;
                    jobLine.Completed = false;
                    jobLine.CalculatedTime = CalculatePlannedTime(jobLine);

                    _context.JobLines.Add(jobLine);
                }
            }

            _context.SaveChanges();

            return RedirectToAction("Index");
            
        }

        public async Task<IActionResult> StartStitchAsync(int jobId)
        {
            TimeRegister start = new TimeRegister();
            start.Start = DateTime.Now;
            start.JobLineId = jobId;
            start.User = await _userManager.GetUserAsync(User);

            _context.TimeRegisters.Add(start);
            _context.SaveChanges();

            JobListVM jobListVm = new JobListVM();

           
            jobListVm.jobId = start.Id;

            return View("StartStitch", jobListVm);
        }

        public IActionResult StopStitch(int jobId)
        {
            TimeRegister stop = new TimeRegister();
            stop = _context.TimeRegisters.FirstOrDefault(e => e.Id == jobId);
            stop.Stop = DateTime.Now;
            _context.Entry(stop).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();

            JobListVM jobListVm = new JobListVM();
            jobListVm.jobId = stop.JobLineId;

            return View("JobReady", jobListVm);

        }

        public IActionResult JobReady(int jobId, bool yes)
        {
            JobLine jobLine = new JobLine();
            jobLine = _context.JobLines.Include(j => j.Job).Where(j => j.Id == jobId).FirstOrDefault();
            jobLine.Completed = yes;
            _context.Entry(jobLine).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();

            //DateTime date = new DateTime();
            //date = jobLine.Job.DeliveryDate;

            //JobListVM jobListVm = new JobListVM();

            //OK, hard to handle from here

            //jobListVm = CreateJobListViewModelByMachineId(date, jobLine.MachineId);
            //ViewBag.date = jobListVm.date;

            return RedirectToAction("Index");
        }

        public TimeSpan calculateTotalTime(int machineId, DateTime deliveryDate)
        {
            List<JobLine> jobLines = new List<JobLine>();
            TimeSpan totalTime = new TimeSpan(0, 0, 3);

            if (machineId != 0)
            {
                jobLines = _context.JobLines.Include(j => j.Job).Where(j => j.MachineId == machineId && j.Job.DeliveryDate == deliveryDate).ToList();
            }
            else
            {
                jobLines = _context.JobLines.Include(j => j.Job).Where(j => j.Job.DeliveryDate == deliveryDate).ToList();
            }

            foreach (JobLine line in jobLines)
            {
                totalTime += line.CalculatedTime;
            }

            return totalTime;
        }

        //public TimeSpan calculateTotalTime(int machineId, MachineTypes machineType, string searchString)
        //{
        //    TimeSpan totalTime = new TimeSpan(1,1,1);

        //    return totalTime;
        //}

        private DateTime handleJobFilterDateTime()
        {
           return this.handleJobFilterDateTime(DateTime.MinValue);
        }

        private DateTime handleJobFilterDateTime(DateTime jobFilterDateTime)
        {
            String jobFilterDateTimeString = String.Empty;

            if (jobFilterDateTime == DateTime.MinValue)
            {
                if (HttpContext.Session != null)
                {
                    jobFilterDateTimeString = HttpContext.Session.GetString(SessionKeyJobFilterDatetime);
                    if (!String.IsNullOrEmpty(jobFilterDateTimeString))
                    {
                        jobFilterDateTime = DateTime.Parse(jobFilterDateTimeString);
                    }
                }
            }

            if(jobFilterDateTime == DateTime.MinValue)
            {
                jobFilterDateTime = DateTime.Today;
            }
          
            HttpContext.Session.SetString(SessionKeyJobFilterDatetime, jobFilterDateTime.ToString());

            return jobFilterDateTime;
        }

        public TimeSpan CalculatePlannedTime(JobLine line)
        {
            Machine mach = _context.Machines.FirstOrDefault(m => m.Id == line.MachineId);

            int stations = line.Job.PageQuantity / 4;
            int quantity = line.Job.Quantity;
            
            double instelMachine = mach.SetupTime + (stations * mach.SetupTimeStationFactor);
            double startDraai1000 = mach.RunTimeTo1000Speed + (stations * mach.RunTimeTo1000SpeedStationFactor);
            double doorDraai1000 = mach.RunTimeFrom1000Speed + (stations * mach.RunTimeFrom1000SpeedStationFactor);
            double quantityDoorDraai = quantity - 1000;
            double centiTime = instelMachine + startDraai1000 + (doorDraai1000 * quantityDoorDraai / 1000);
            
            int hour = 0;
            int minute = 0;
            int second = 0;
            if (centiTime >= 1)
            {
                hour = 1;
                centiTime--;
            }
            
            double centiMinute = centiTime % 1;
            minute = (int)Math.Round(centiMinute * 60);
            
            double centiSecond = (centiMinute * 60) - minute;
            second = (int)Math.Round(centiSecond * 60);
            
            TimeSpan planned = new TimeSpan(hour, minute, second);
            return planned;
        }

        public IActionResult ReadStitch(int jobId)
        {
            JobLineVM jobLineVm = new JobLineVM();
            jobLineVm.jobLine = _context.JobLines.Include(j => j.Job).FirstOrDefault(j => j.Id == jobId);
            jobLineVm.date = jobLineVm.jobLine.Job.DeliveryDate.ToString("yyyy-MM-dd");

            return View("ReadStitch", jobLineVm);
        }

        public IActionResult UpdateStitch(JobLine jobLine)
        {
            Job job = jobLine.Job;
            _context.Entry(job).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            //_context.Entry(jobLine).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
