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
                        line.JobReady = false;

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
            viewModelJobs.totalTime = new TimeSpan(0,0,0);

            foreach (JobLine j in viewModelJobs.jobLineList)
            {
                JobLineCalculatedTime calc = new JobLineCalculatedTime();

                calc.Id = j.Id;
                calc.Machine = j.Machine;
                calc.MachineId = j.MachineId;
                calc.Job = j.Job;
                calc.JobId = j.JobId;
                calc.User = j.User;
                calc.UserId = j.UserId;
                calc.MachineType = j.MachineType;
                calc.Sequence = j.Sequence;
                calc.Department = j.Department;
                calc.DepartmentId = j.DepartmentId;
                calc.JobReady = j.JobReady;

                calc.CalculatedTime = CalculatePlannedTime(j);
                viewModelJobs.totalTime += calc.CalculatedTime;
                viewModelJobs.jobLineListCalculatedTime.Add(calc);
            }

            return viewModelJobs;
        }

        public TimeSpan CalculatePlannedTime(JobLine line)
        {
            int stations = line.Job.PageQuantity/4;
            int quantity = line.Job.Quantity;

            double instelMachine = 0.18 + (stations * 0.02);
            double startDraai1000 = 0.268 + (stations * 0.0224);
            double doorDraai1000 = 0.241 + (stations * 0.0163);
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
    }
}