using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZwartOpWit.Models;
using ZwartOpWit.Models.Viewmodels;
using System.IO;

namespace ZwartOpWit.Controllers
{
    public class JobController : Controller
    {
        private readonly AppDBContext _context;

        public JobController(AppDBContext context)
        {
            _context = context;
        }

        public IActionResult Index(DateTime date)
        {
            JobListVM jobListVM = new JobListVM();
            jobListVM.jobList = _context.Jobs.Where(e => e.DeliveryDate == date).Include(l => l.JobLineList).ToList();
            //jobListVM.jobList = _context.Jobs.ToList();

           // var query = _context.JobLines.Join(_context.Jobs, e => e.DepartmentId, f => f.DeliveryDate, (e, f) => new { e = 1, f = date });

            String formatted = date.ToString("yyyy-MM-dd");
            
            jobListVM.date = formatted;

            jobListVM.jobLineList = _context.JobLines.Where(e => e.DepartmentId == 1).Include(j => j.Job).ToList();

            return View(jobListVM);
        }

        public IActionResult PlanStitch(int id, DateTime date)
        {
            //if (id != 0)
            //{
            //    Job j = new Job();
            //    j = _context.Jobs.FirstOrDefault(e => e.Id == id);
            //    j.MachineId = StitchId;
            //    _context.Entry(s).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            //    _context.SaveChanges();
            //}

            

            JobListVM jobListVM = new JobListVM();
            jobListVM.jobList = _context.Jobs.Where(e => e.DeliveryDate == date).Include(l => l.JobLineList).ToList();
            //stitchJobsListVM.stitchJobsList = _context.Stitches.Where(e => e.MachineId == StitchId && e.DeliveryDate == date).ToList();

            

            String formatted = date.ToString("yyyy-MM-dd");

            jobListVM.date = formatted;
            //stitchJobsListVM.StitchId = StitchId;

            return View("Index", jobListVM);
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
                        job = new Job();

                        job.DeliveryDate = Convert.ToDateTime(splits[4]);
                        job.JobNumber = splits[1];

                        double aantal = double.Parse(splits[2]);
                        job.Quantity = Convert.ToInt16(aantal);

                        job.PaperBw = splits[8];

                        _context.Jobs.Add(job);
                    }

                    //Create joblines
                    if (job != null)
                    {
                        JobLine line = new JobLine();

                        line.MachineType = MachineTypes.Stitch;

                        _context.JobLines.Add(line);
                    }
                }
            }

            _context.SaveChanges();


            return RedirectToAction("Index", 1);
        }
        //    public IActionResult StartStitch()
        //    {
        //        StartStop Start = new StartStop();
        //        Start.Start = DateTime.Today;
        //        //_context.StartStops.Add(Start);
        //        _context.SaveChanges();

        //        StitchJobsListVM stitchJobsListVM = new StitchJobsListVM();
        //        return View("StartStitch", stitchJobsListVM);
        //    }
    }
}
