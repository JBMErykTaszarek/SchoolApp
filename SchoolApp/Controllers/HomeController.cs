using Microsoft.AspNet.Identity;
using SchoolApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolApp.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            string currentParentId = User.Identity.GetUserId();
            ApplicationUser currentParent = db.Users.FirstOrDefault(x => x.Id == currentParentId);
                int count = db.Messages.Where(x => (x.to == currentParent.Email) && (x.isReaded == false)).Count();
            ViewData["count"] = count;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}