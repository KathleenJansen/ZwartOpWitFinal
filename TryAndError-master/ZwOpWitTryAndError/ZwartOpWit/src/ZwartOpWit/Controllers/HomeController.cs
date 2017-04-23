using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZwartOpWit.Models.Viewmodels;

namespace ZwartOpWit.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            JobListVM jobListVM = new JobListVM();
            jobListVM.date = DateTime.Today.ToString("yyyy-MM-dd");

            return View(jobListVM);
        }

        //public IActionResult Stitch()
        //{
        //    ViewData["Message"] = "Your Stitch page.";

        //    return View();
        //}

        public IActionResult Fold()
        {
            ViewData["Message"] = "Your Fold page.";

            return View();
        }

        public IActionResult Typo()
        {
            ViewData["Message"] = "Your Typo page.";

            return View();
        }

        public IActionResult Score()
        {
            ViewData["Message"] = "Your Score page";

            return View();
        }

        public IActionResult Bush()
        {
            ViewData["Message"] = "Your Bush page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
