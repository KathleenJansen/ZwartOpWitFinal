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
            JobListVM jobListVm = new JobListVM();
            jobListVm.jobLineList = _context.JobLines.Include(j => j.Job).Where(j => j.Job.DeliveryDate == date).Where(j => j.JobId == 98).ToList();

            if (DateTime.Equals(DateTime.MinValue, date))
            {
                date = DateTime.Today;
            }

            String formatted = date.ToString("yyyy-MM-dd");
            
            jobListVm.date = formatted;

            jobListVm.jobLineList = _context.JobLines.Where(e => e.DepartmentId == 1).Include(j => j.Job).ToList();

            return View(jobListVm);
        }

        public IActionResult IndexStitch(DateTime date)
        {
            if (DateTime.Equals(DateTime.MinValue, date))
            {
                date = DateTime.Today;
            }

            JobListVM jobListVm = new JobListVM();
            jobListVm.jobLineList = _context.JobLines.Include(j => j.Job).Where(j => j.Job.DeliveryDate == date).Where(j => j.MachineId == 1).ToList();

            string formatted = date.ToString("yyyy-MM-dd");
            jobListVm.date = formatted;
            jobListVm.departmentId = 1;
            jobListVm.machineId = 1;


            ViewBag.date = formatted;

            return View("Index", jobListVm);
        }

        public IActionResult PlanStitch(int id, DateTime date, int machineId)
        {

            if (id != 0)
            {
                JobLine j = new JobLine();
                j = _context.JobLines.FirstOrDefault(e => e.Id == id);
                j.MachineId = machineId;
                _context.Entry(j).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
            }

            if (DateTime.Equals(DateTime.MinValue, date))
            {
                date = DateTime.Today;
            }

            JobListVM jobListVm = new JobListVM();
            jobListVm.jobLineList = _context.JobLines.Include(j => j.Job).Where(j => j.Job.DeliveryDate == date).Where(j => j.MachineId == machineId).ToList();
            
            String formatted = date.ToString("yyyy-MM-dd");

            jobListVm.date = formatted;
            jobListVm.machineId = machineId;
            jobListVm.departmentId = 1;

            Machine m = _context.Machines.Where(e => e.Id == machineId).FirstOrDefault();

            jobListVm.machineName = m.Name;

            ViewBag.date = formatted;

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

        public IActionResult StartStitch(int Id)
        {
            TimeRegister start = new TimeRegister();
            start.Start = DateTime.Now;
            start.JobLineId = Id;
            _context.TimeRegisters.Add(start);
            _context.SaveChanges();

            JobListVM jobListVm = new JobListVM();
            TimeRegister t = new TimeRegister();
            t = _context.TimeRegisters.Where(z => z.JobLineId == Id).FirstOrDefault();
            jobListVm.jobId = t.Id;

            return View("StartStitch", jobListVm);
        }

        public IActionResult StopStitch(int Id)
        {
            TimeRegister stop = new TimeRegister();
            stop.Stop = DateTime.Now;
            stop.JobLineId = Id;
            _context.TimeRegisters.Add(stop);
            _context.SaveChanges();

            return RedirectToAction("Index", 1);
        }
    }
}
