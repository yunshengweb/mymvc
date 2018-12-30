using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using mymvc.Models;

namespace mymvc.Controllers
{


    public class HomeController : Controller
    {
        
        IConfiguration myConf;
        public HomeController(IConfiguration tConf)
        {
            myConf=tConf;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            ViewData["m1"]=myConf["ConnectionStrings:MvcData"];
            ViewData["Connect"]="abc";
            return View();
        }

        public IActionResult SignIn()
        {
            return RedirectToAction("Index","Login");
        }

        public IActionResult Administer()
        {
            return RedirectToAction("Index","Admin");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
