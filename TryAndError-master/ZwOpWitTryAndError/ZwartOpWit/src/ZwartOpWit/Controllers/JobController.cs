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

namespace ZwartOpWit.Controllers
{
    public class JobController : Controller
    {
        //Constant to determine session filter date time
        const string SessionKeyJobFilterDatetime = "JobFilterDatetime";
        const int PageSize = 10;

        private readonly AppDBContext _context;

        public JobController(AppDBContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Index( string sortOrder,
                                                string currentFilter,
                                                string searchString,
                                                int? page,
                                                int machineId,
                                                MachineTypes machineType,
                                                DateTime jobFilterDateTime)
        {
            JobListVM jobListVm = new JobListVM();
            jobFilterDateTime   = this.handleJobFilterDateTime(jobFilterDateTime);

            jobListVm.CurrentSort           = sortOrder;
            jobListVm .CurrentFilter        = searchString;
            jobListVm.JobNumberSortParm     = String.IsNullOrEmpty(sortOrder) ? "jobNumber_desc" : "";
            jobListVm.QuantitySortParm      = sortOrder == "quantity" ? "quantity_desc" : "quantity";
            jobListVm.PageQuantitySortParm  = sortOrder == "pageQuantity" ? "pageQuantity_desc" : "pageQuantity";

            if (searchString != currentFilter)
            {
                page = 1;
            }

            var joblines = _context.JobLines.Include(j => j.Job).AsQueryable();

            //Add filters
            if (!String.IsNullOrEmpty(searchString))
            {
                joblines = joblines.Where(jl => jl.Job.JobNumber.Contains(searchString));
            }

            //Machine type filter is set
            if (System.Enum.IsDefined(typeof(MachineTypes), machineType))
            {
                joblines = joblines.Where(jl => jl.MachineType == machineType);
            }

            //Machine id is set
            if (machineId != 0)
            {
                joblines = joblines.Where(jl => jl.MachineId == machineId);
            }

            //Filter on date
            if (jobFilterDateTime != DateTime.MinValue)
            {
                joblines = joblines.Where(jl => jl.Job.DeliveryDate == jobFilterDateTime);
            }

            //< th > PaperInside </ th >
            //< th > PaperCover </ th >
            //< th > Machine </ th >
            //< th > Time </ th >

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

            jobListVm.jobFilterDateTime = jobFilterDateTime.ToString("yyyy-MM-dd");
            jobListVm.jobLineList = await PaginatedList<JobLine>.CreateAsync(joblines.AsNoTracking(), page ?? 1, PageSize);

            return View(jobListVm);
        }

   

        public IActionResult ReadStitch(int jobId)
        {
            JobLineVM jobVm = new JobLineVM();

            JobLine jobLine = new JobLine();

            jobLine = _context.JobLines.Include(j => j.Job).FirstOrDefault(j => j.Id == jobId);

            jobVm.jobLine = jobLine;

            TimeRegister time = new TimeRegister();

            time = _context.TimeRegisters.Include(j => j.JobLine).FirstOrDefault(j => j.JobLineId == jobId);

            jobVm.date = jobLine.Job.DeliveryDate.ToString("yyyy-MM-dd");


            return View("ReadStitch", jobVm);
        }

        public IActionResult EditStitch(int jobLineId, string jobNr, string paperBw, DateTime date, 
            string width, string heigth, string pageQuantity, string machineId, string sequence, string jobReady)
        {
            Job myJob = new Job();
            JobLine myJobLine = new JobLine();

            myJobLine = _context.JobLines.Include(e => e.Job).FirstOrDefault(e => e.Id == jobLineId);
            myJob = _context.Jobs.FirstOrDefault(j => j.Id == myJobLine.Job.Id);

            myJob.JobNumber = jobNr;
            myJob.PaperBw = paperBw;
            myJob.DeliveryDate = date;
            myJob.Width = int.Parse(width);
            myJob.Heigth = int.Parse(heigth);
            myJob.PageQuantity = int.Parse(pageQuantity);
            myJobLine.MachineId = int.Parse(machineId);
            myJobLine.Sequence = int.Parse(sequence);
            //myJobLine.JobReady = bool.Parse(JobReady);

            _context.Entry(myJob).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.Entry(myJobLine).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();

            return View("Index");
        }

        public IActionResult PlanStitch(int jobId, DateTime date, int machineId)
        {
            
            if (jobId != 0)
            {
                JobLine j = new JobLine();
                j = _context.JobLines.FirstOrDefault(e => e.Id == jobId);
                j.MachineId = machineId;
                _context.Entry(j).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
            }

            if (DateTime.Equals(DateTime.MinValue, date))
            {
                date = DateTime.Today;
            }

            JobListVM jobListVm = new JobListVM();

            jobListVm = CreateJobListViewModelByMachineId(date, machineId);
            ViewBag.date = jobListVm.jobFilterDateTime; // viewBag aangemaakt om default.cshtml aan te spreken...(?)

            return View("Index", jobListVm);
        }

        public IActionResult Import()
        {
            Job job;

            using (StreamReader lezer = new StreamReader(new FileStream("myCsv.txt", FileMode.Open)))
            {
                string lijn = lezer.ReadLine();

                while (!lezer.EndOfStream)
                {
                    lijn = lezer.ReadLine();
                    string[] splits = lijn.Split(';');


                    //Check if job exists with current job number
                    job = _context.Jobs.Where(x => x.JobNumber == splits[1]).FirstOrDefault();

                    if (job == null)
                    {
                        //Create Job
                        job = new Job();

                        job.DeliveryDate = Convert.ToDateTime(splits[4]);
                        job.JobNumber = splits[1];

                        double aantal = double.Parse(splits[2]);
                        job.Quantity = Convert.ToInt16(aantal);

                        job.PaperBw = splits[8];
                        job.Cover = 0;
                        job.PaperCover = "no cover";
                        job.Heigth = 297;
                        job.Width = 210;

                        _context.Jobs.Add(job);

                        //Create JobLine
                        JobLine line = new JobLine();

                        line.JobId = job.Id;
                        line.MachineId = 1;
                        line.Sequence = 1;
                        line.UserId = "2aff4902-2ab4-4e25-88fc-4765d661e8f2";
                        line.MachineType = MachineTypes.Stitch;
                        line.DepartmentId = 1;
                        line.Completed = false;

                        _context.JobLines.Add(line);
                    }

                    //Create joblines
                    //if (job != null)
                    //{
                    //    JobLine line = new JobLine();

                    //    line.JobId = job.Id;

                    //    line.MachineType = MachineTypes.Stitch;

                    //    _context.JobLines.Add(line);
                    //}
                }
            }

            _context.SaveChanges();


            return RedirectToAction("Index", 1);
        }

        public IActionResult StartStitch(int jobId)
        {
            TimeRegister start = new TimeRegister();
            start.Start = DateTime.Now;
            start.JobLineId = jobId;
            _context.TimeRegisters.Add(start);
            _context.SaveChanges();

            int myNewId = start.Id;

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
            jobLine.Completed = true;
            _context.Entry(jobLine).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();

            DateTime date = new DateTime();
            date = jobLine.Job.DeliveryDate;

            JobListVM jobListVm = new JobListVM();

            jobListVm = CreateJobListViewModelByMachineId(date, jobLine.MachineId);
            ViewBag.date = jobListVm.jobFilterDateTime;

            return View("Index", jobListVm);
        }

        public JobListVM CreateJobListViewModelByMachineId(DateTime date, int machineId)
        {
            JobListVM viewModelJobs = new JobListVM();
            PlannedTimeCalculator plannedTimeCalculator = new PlannedTimeCalculator(_context);

            //if (machineId != 0)
            //{
            //    viewModelJobs.jobLineList = _context.JobLines.Include(j => j.Job).
            //        Where(j => j.Job.DeliveryDate == date && j.Completed == false).
            //        Where(j => j.MachineId == machineId).ToList();
            //    viewModelJobs.machineName = _context.Machines.Where(e => e.Id == machineId).FirstOrDefault().Name;
            //}
            //else
            //{
            //    viewModelJobs.jobLineList = _context.JobLines.Include(j => j.Job).
            //        Where(j => j.Job.DeliveryDate == date && j.Completed == false).ToList();
            //    viewModelJobs.machineName = "All Jobs";
            //}

            viewModelJobs.jobFilterDateTime = date.ToString();
            viewModelJobs.machineId = machineId;
            viewModelJobs.departmentId = 1;
            viewModelJobs.totalTime = new TimeSpan(0,0,0);

            foreach (JobLine j in viewModelJobs.jobLineList)
            {
                j.CalculatedTime = plannedTimeCalculator.CalculatePlannedTimeStich(j);
                viewModelJobs.totalTime += j.CalculatedTime;
                viewModelJobs.jobLineList.Add(j);
            }

            return viewModelJobs;
        }

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
    }
}
