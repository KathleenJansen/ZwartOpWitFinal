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
    public class StitchController: Controller
    {
        private readonly AppDBContext _context;

        public StitchController(AppDBContext context)
        {
            _context = context;
        }

        public IActionResult Index(int id)
        {
            StitchJobsListVM stitchJobsListVM = new StitchJobsListVM();
            stitchJobsListVM.stitchJobsList = _context.Stitches.Where(e => e.MachineId == id).Include(m => m.Machine).ToList();
            return View(stitchJobsListVM);
        }
        public IActionResult Out(int id)
        {
            Stitch s = new Stitch();
            s = _context.Stitches.FirstOrDefault(e => e.Id == id);
            s.MachineId = 1;
            _context.Entry(s).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();

            StitchJobsListVM stitchJobsListVM = new StitchJobsListVM();
            stitchJobsListVM.stitchJobsList = _context.Stitches.Where(e => e.MachineId == 1).ToList();
            return View("Index", stitchJobsListVM);
        }
        public IActionResult PlanStitch1(int id)
        {
            Stitch s = new Stitch();
            s = _context.Stitches.FirstOrDefault(e => e.Id == id);
            s.MachineId = 2;
            _context.Entry(s).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();

            StitchJobsListVM stitchJobsListVM = new StitchJobsListVM();
            stitchJobsListVM.stitchJobsList = _context.Stitches.Where(e => e.MachineId == 2).ToList();
            return View("Index", stitchJobsListVM);
        }
        public IActionResult PlanStitch2(int id)
        {
            Stitch s = new Stitch();
            s = _context.Stitches.FirstOrDefault(e => e.Id == id);
            s.MachineId = 3;
            _context.Entry(s).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();

            StitchJobsListVM stitchJobsListVM = new StitchJobsListVM();
            stitchJobsListVM.stitchJobsList = _context.Stitches.Where(e => e.MachineId == 3).ToList();
            return View("Index", stitchJobsListVM);
        }
        public IActionResult PlanStitch3(int id)
        {
            Stitch s = new Stitch();
            s = _context.Stitches.FirstOrDefault(e => e.Id == id);
            s.MachineId = 4;
            _context.Entry(s).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();

            StitchJobsListVM stitchJobsListVM = new StitchJobsListVM();
            stitchJobsListVM.stitchJobsList = _context.Stitches.Where(e => e.MachineId == 4).ToList();
            return View("Index", stitchJobsListVM);
        }
        public IActionResult PlanStitch4(int id)
        {
            Stitch s = new Stitch();
            s = _context.Stitches.FirstOrDefault(e => e.Id == id);
            s.MachineId = 5;
            _context.Entry(s).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();

            StitchJobsListVM stitchJobsListVM = new StitchJobsListVM();
            stitchJobsListVM.stitchJobsList = _context.Stitches.Where(e => e.MachineId == 5).ToList();
            return View("Index", stitchJobsListVM);
        }
        public IActionResult Import()
        {
            using (StreamReader lezer = new StreamReader(new FileStream("myCsv.txt", FileMode.Open)))
            {
                string lijn = lezer.ReadLine();

                while (!lezer.EndOfStream)
                {
                    lijn = lezer.ReadLine();
                    string[] splits = lijn.Split(';');
                    Stitch stitch = new Stitch();
                    stitch.DeliveryDate = Convert.ToDateTime(splits[4]);
                    stitch.JobNumber = splits[1];

                    double aantal = double.Parse(splits[2]);
                    stitch.Quantity = Convert.ToInt16(aantal);

                    stitch.PaperBw = splits[8];
                    stitch.MachineId = 1;
                    stitch.UserId = "1";

                    _context.Stitches.Add(stitch);
                }
            }

            _context.SaveChanges();
            

            return RedirectToAction("Index", 1);
        }
    }
}
