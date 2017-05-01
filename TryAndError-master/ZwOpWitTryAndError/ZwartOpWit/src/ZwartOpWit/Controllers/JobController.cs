﻿using Microsoft.AspNetCore.Mvc;
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
            jobListVm = CreateJobListViewModelByMachineId(date, 0);
            ViewBag.date = jobListVm.date;

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

            jobListVm = CreateJobListViewModelByMachineId(date, machineId);
            ViewBag.date = jobListVm.date; // viewBag aangemaakt om default.cshtml aan te spreken...(?)

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

            int myNewId = start.Id;

            JobListVM jobListVm = new JobListVM();
            jobListVm.jobId = start.Id;

            return View("StartStitch", jobListVm);
        }

        public IActionResult StopStitch(int Id)
        {
            TimeRegister stop = new TimeRegister();
            stop = _context.TimeRegisters.FirstOrDefault(e => e.Id == Id);
            stop.Stop = DateTime.Now;
            _context.Entry(stop).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();

            JobListVM jobListVm = new JobListVM();
            jobListVm.jobId = stop.JobLineId;

            return View("JobReady", jobListVm);

        }

        public IActionResult JobReady(int id, bool yes)
        {
            JobLine jobLine = new JobLine();
            jobLine = _context.JobLines.Include(j => j.Job).Where(j => j.Id == id).FirstOrDefault();
            jobLine.JobReady = yes;
            _context.Entry(jobLine).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();

            DateTime date = new DateTime();
            date = jobLine.Job.DeliveryDate;

            JobListVM jobListVm = new JobListVM();

            jobListVm = CreateJobListViewModelByMachineId(date, jobLine.MachineId);
            ViewBag.date = jobListVm.date;

            return View("Index", jobListVm);
        }

        public JobListVM CreateJobListViewModelByMachineId(DateTime date, int machineId)
        {
            JobListVM viewModelJobs = new JobListVM();

            if (machineId != 0)
            {
                viewModelJobs.jobLineList = _context.JobLines.Include(j => j.Job).
                    Where(j => j.Job.DeliveryDate == date && j.JobReady == false).
                    Where(j => j.MachineId == machineId).ToList();
                viewModelJobs.machineName = _context.Machines.Where(e => e.Id == machineId).FirstOrDefault().Name;
            }
            else
            {
                viewModelJobs.jobLineList = _context.JobLines.Include(j => j.Job).
                    Where(j => j.Job.DeliveryDate == date && j.JobReady == false).ToList();
                viewModelJobs.machineName = "All Jobs";
            }

            viewModelJobs.date = date.ToString("yyyy-MM-dd");
            viewModelJobs.machineId = machineId;
            viewModelJobs.departmentId = 1;
            
            viewModelJobs.plannedTime = CalculatePlannedTime();

            return viewModelJobs;
        }

        public TimeSpan CalculatePlannedTime()
        {
            TimeSpan planned = new TimeSpan(1, 30,0);
            return planned;
        }
    }
}
