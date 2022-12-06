using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace asp.net_mvc_test.Controllers
{
    public class HelloWorldController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Welcome(string name, int numTimes =1)
        {
            ViewData["Message"] = $"こんにちは!{name}さん";
            ViewData["numTimes"] = numTimes;

            return View();
        }



    }
}