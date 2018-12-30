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
    public class AdminController : Controller
    {        
        
        IConfiguration myConf;
        public AdminController(IConfiguration tConf)
        {
            myConf=tConf;
        }
        public IActionResult Index()  
        {     
    
            Account tAcc=new Account(myConf["ConnectionStrings:MvcData"]);
            List<User> tAllUsers=tAcc.GetAllUsers();
            return View(tAllUsers);  
        }  
        public IActionResult Access()  
        {  
            return View();  
        }  


         // GET: PersonalDetails/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User tUser)
        {
            ModelState.Remove("ConfirmPassword");

            if(ModelState.IsValid)
            {
                //db.PersonalDetails.Add(tUser);
                //db.SaveChanges();
                Account tAcc=new Account(myConf["ConnectionStrings:MvcData"]);
                if(tAcc.InsertUser(tUser))
                {
                    return RedirectToAction("Index");
                }
            }

            return View(tUser);
        }

        public ActionResult Delete(string Id)
        {
            Account tAcc=new Account(myConf["ConnectionStrings:MvcData"]);
            tAcc.DeleteUser(Id);     
            return RedirectToAction("Index");   
        }

        public ActionResult Edit(string Id)
        {
            Account tAcc=new Account(myConf["ConnectionStrings:MvcData"]);
            return View(tAcc.GetUser(Id));
        }



        [HttpPost]
        public ActionResult Edit(User tUser)
        {
            Account tAcc=new Account(myConf["ConnectionStrings:MvcData"]);
            int tc=0;
            if(tUser.Confirm) tc=1;
            string items="Email='"+tUser.Email+"',Confirm="+tc;
            tAcc.UpdateUser(tUser.UserId,items);
            return RedirectToAction("Index");
        }


    }
}

