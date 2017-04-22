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

        public IActionResult Index(int id, DateTime date, int StitchId)
        {
            StitchJobsListVM stitchJobsListVM = new StitchJobsListVM();
            stitchJobsListVM.stitchJobsList = _context.Stitches.Where(e => e.MachineId == StitchId && e.DeliveryDate == date).Include(m => m.Machine).ToList();

            String formatted = date.ToString("yyyy-MM-dd");

            stitchJobsListVM.date = formatted;
            stitchJobsListVM.StitchId = 1;
            return View(stitchJobsListVM);
        }
        
        public IActionResult PlanStitch(int id, DateTime date, int StitchId)
        {
            if (id != 0)
            {
                Stitch s = new Stitch();
                s = _context.Stitches.FirstOrDefault(e => e.Id == id);
                s.MachineId = StitchId;
                _context.Entry(s).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
            }

            StitchJobsListVM stitchJobsListVM = new StitchJobsListVM();
            stitchJobsListVM.stitchJobsList = _context.Stitches.Where(e => e.MachineId == StitchId && e.DeliveryDate == date).ToList();

            String formatted = date.ToString("yyyy-MM-dd");

            stitchJobsListVM.date = formatted;
            stitchJobsListVM.StitchId = StitchId;

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
                    stitch.UserId = "8ffe39f3-1f56-4155-bf48-af5544e318fd";

                    _context.Stitches.Add(stitch);
                }
            }

            _context.SaveChanges();
            

            return RedirectToAction("Index", 1);
        }
        public IActionResult StartStitch()
        {
            StartStop Start = new StartStop();
            Start.Start = DateTime.Today;
            //_context.StartStops.Add(Start);
            _context.SaveChanges();

            StitchJobsListVM stitchJobsListVM = new StitchJobsListVM();
            return View("StartStitch", stitchJobsListVM);
        }
    }
}
