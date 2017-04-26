using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZwartOpWit.Models;
using ZwartOpWit.Models.Viewmodels;

namespace ZwartOpWit.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDBContext _context;

        public HomeController(AppDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            ViewBag.date = DateTime.Today.ToString("yyyy-MM-dd");
            return View();
        }
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
