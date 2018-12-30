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
    public class LoginController : Controller
    {
        
        IConfiguration myConf;
        public LoginController(IConfiguration tConf)
        {
            myConf=tConf;
        }
        public IActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Index(User sm)
        {
            ModelState.Remove("ConfirmPassword");
            if(!ModelState.IsValid)
            {
                return View();
            }

            Account tAcc=new Account(myConf["ConnectionStrings:MvcData"]);
            if(tAcc.IsNew(sm.Email))
            {
                ModelState.AddModelError(string.Empty, "Email not in the database.");
                TempData["Email"]=sm.Email;
                return RedirectToAction("Register");
            }

            else if(!tAcc.Confirm(sm.Email))
            {
                ModelState.AddModelError(string.Empty, "Email is not confirmed.");
                TempData["UserId"]=tAcc.GetUserId(sm.Email).ToString();
                return RedirectToAction("RequestEmail");
            }   


            else if(!tAcc.PasswordMatch(sm.Email,sm.Password))
            {
                ModelState.AddModelError(string.Empty, "Password does not match.");
                return View();
            }

            TempData["UserId"]=tAcc.GetUserId(sm.Email);
            return RedirectToAction("Welcome");
        }

        public ActionResult Welcome()
        {
            if(TempData["UserId"]==null)
            {
                return Logout();
            }
            else
            {
                Account tAcc=new Account(myConf["ConnectionStrings:MvcData"]);
                return View(tAcc.GetUser(TempData["UserId"].ToString()));
            }
        }

        public ActionResult Logout()
        {
            return RedirectToAction("Index");
        }


     

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User sm)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            Account tAcc=new Account(myConf["ConnectionStrings:MvcData"]);
            if(!tAcc.IsNew(sm.Email)&&tAcc.Confirm(sm.Email))
            {
                ModelState.AddModelError(string.Empty, "Email confirmed in the database.");
                TempData["Email"]=sm.Email;
                return RedirectToAction("Index");
            } 

            else if(!tAcc.IsNew(sm.Email)&&!tAcc.Confirm(sm.Email))
            {
                ModelState.AddModelError(string.Empty, "Email is not confirmed.");
                TempData["UserId"]=tAcc.GetUserId(sm.Email).ToString();
                return RedirectToAction("RequestEmail");
            }

            
            else 
            {
                tAcc.InsertUser(sm);
                

                TempData["UserId"]=tAcc.GetUserId(sm.Email).ToString();
                return RedirectToAction("RequestEmail");
            }
        }


        public ActionResult RequestEmail()
        {
            ViewBag.Code=Int32.Parse(TempData["UserId"].ToString()) %997;
            return View();
        } 

        [HttpPost]
        public ActionResult RequestEmail(string userId, string code)
        {
            int uid;
            int cde;
            if(Int32.TryParse(userId,out uid)&&Int32.TryParse(code,out cde))
            {
                if(uid % 997==cde)
                {
                    Account tAcc=new Account(myConf["ConnectionStrings:MvcData"]);
                    tAcc.UpdateConfirm(userId);
                    TempData["UserId"]=userId;
                    return RedirectToAction("Welcome");
                }
                else
                {
                    TempData["UserId"]=userId;
                    return View();
                }
            }
            return View("Index");
        }
    }
}
