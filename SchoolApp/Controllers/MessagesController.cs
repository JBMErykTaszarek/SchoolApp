using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SchoolApp.Models;

namespace SchoolApp.Controllers
{
    public class MessagesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private object currentTeacher;

        // GET: Messages
        public ActionResult Index()
        {
            string currentParentId = User.Identity.GetUserId();
            ApplicationUser currentParent = db.Users.FirstOrDefault(x => x.Id == currentParentId);

            return View(db.Messages.ToList().Where(x => x.to == currentParent.Email));
        }

        // GET: Messages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // GET: Messages/Create
        public ActionResult Create()
        {
            string currentTeacherId = User.Identity.GetUserId();
            List<string> emails = new List<string> {"all"};
            ApplicationUser currentTeahcer = db.Users.FirstOrDefault(x => x.Id == currentTeacherId);
            foreach (var item in db.Users.ToList().Where(x => (x.Role.ToLower() == "parent") && (x.Clas == currentTeahcer.Clas)))
            {
                emails.Add(item.Email);
            }
            
            ViewData["ParentList"] = emails;
            return View();
        }

        // POST: Messages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,from,to,messageContent,isReaded")] Message message)
        {
            if (ModelState.IsValid)
            {
                string currentTeacherId = User.Identity.GetUserId();
                ApplicationUser currentTeahcer = db.Users.FirstOrDefault(x => x.Id == currentTeacherId);
                if (message.to == "all")
                {
                    foreach (var item in db.Users.ToList().Where(x => (x.Role.ToLower() == "parent") && (x.Clas == currentTeahcer.Clas)))
                    {
                        message.from = currentTeahcer.Email;
                        message.isReaded = false;
                        message.to = item.Email;
                        db.Messages.Add(message);
                        db.SaveChanges();
                    }

                }
                else
                {
                    message.from = currentTeahcer.Email;
                    message.isReaded = false;
                    db.Messages.Add(message);
                    db.SaveChanges();
                }
             
                return RedirectToAction("Index");
            }

            return View(message);
        }
        public ActionResult GetMyMessages()
        {
            string currentParentId = User.Identity.GetUserId();
            ApplicationUser currentParent = db.Users.FirstOrDefault(x => x.Id == currentParentId);
            foreach (var item in db.Messages)
            {
                if (item.to == currentParent.Email)
                {
                    item.isReaded = true;
                    db.Entry(item).State = EntityState.Modified;
                }
            }

            db.SaveChanges();

            return View(db.Messages.ToList().Where(x => x.to == currentParent.Email));
        }

        // GET: Messages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // POST: Messages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,from,to,message,isReaded")] Message message)
        {
            if (ModelState.IsValid)
            {
                db.Entry(message).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(message);
        }

        // GET: Messages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // POST: Messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Message message = db.Messages.Find(id);
            db.Messages.Remove(message);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
