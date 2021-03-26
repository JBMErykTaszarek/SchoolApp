using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Office.Interop.Excel;
using SchoolApp.Models;

namespace SchoolApp.Controllers
{
    public class SchoolTasksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private Worksheet xlWorkSheet;

        // GET: SchoolTasks
        public ActionResult Index()
        {
            string currentTeacherId = User.Identity.GetUserId();
            ApplicationUser currentTeahcer = db.Users.FirstOrDefault(x => x.Id == currentTeacherId);
            return View(db.SchoolTasks.ToList().Where(x => x.whichClas == currentTeahcer.Clas));
        }

        public int[] countAVG(string title)
        {
            int avg = 0;
            int ct = 0;
            foreach (var item in db.Notes.ToArray().Where(x => x.ForTask == title))
            {
                if (item.WchitNote == "A")
                    avg += 5;
                if (item.WchitNote == "B")
                    avg += 4;
                if (item.WchitNote == "C")
                    avg += 3;
                if (item.WchitNote == "D")
                    avg += 2;
                if (item.WchitNote == "E")
                    avg += 1;
                ct++;
            }
            int[] retList = { avg, ct };
            return retList;
        }

        public ActionResult Generate()
        {
            string currentTeacherId = User.Identity.GetUserId();
            ApplicationUser currentTeahcer = db.Users.FirstOrDefault(x => x.Id == currentTeacherId);
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook worKbooK;
            worKbooK = xlApp.Workbooks.Add(Type.Missing);
            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)worKbooK.Worksheets.get_Item(1);
            xlWorkSheet.Cells[1, 1] = "Nazwa Zadania";
            xlWorkSheet.Cells[1, 2] = "Ile wystawionych ocen";
            xlWorkSheet.Cells[1, 3] = "Średnia ocena za zadanie:";


            int count = 2;
            foreach (SchoolTask item in (db.SchoolTasks.ToList().Where(x => x.whichClas == currentTeahcer.Clas)))
            {
                xlWorkSheet.Cells[count, 1] = item.title;
                int[] avgctlist = countAVG(item.title);
                xlWorkSheet.Cells[count, 2] = avgctlist[1];
                xlWorkSheet.Cells[count, 3] = avgctlist[0] / avgctlist[1];
                count++;


            }
            worKbooK.SaveAs("raport-" + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx");

            return RedirectToAction("Index");
        }

        public ActionResult GetMyStudents()
        {
            string currentTeacherId = User.Identity.GetUserId();
            ApplicationUser currentTeahcer = db.Users.FirstOrDefault(x => x.Id == currentTeacherId);
            return View(db.Users.ToList().Where(x => (x.Clas == currentTeahcer.Clas) && (x.Role.ToLower() == "student")));
        }
        public ActionResult AllNotes()
        {

            return View(db.Notes.ToList());
        }
        public ActionResult GetStudentNotes(string email)
        {

            return View(db.Notes.ToList().Where(x => x.email == email));
        }
        //get
        [HttpPost]
        public ActionResult UpdateNotes([Bind(Include = "Id,ForTask,WchitNote,email")] Note thisnote, string email, string note, string title)

        {
            thisnote.WchitNote = note;
            thisnote.email = email;
            thisnote.ForTask = title;
            db.Notes.Add(thisnote);
            db.SaveChanges();
            return RedirectToAction("NoteTask");

        }


        public ActionResult NoteTask(int? id, string title)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SchoolTask schoolTask = db.SchoolTasks.Find(id);
            if (schoolTask == null)
            {
                return HttpNotFound();
            }
            string currentTeacherId = User.Identity.GetUserId();
            ApplicationUser currentTeahcer = db.Users.FirstOrDefault(x => x.Id == currentTeacherId);
            ViewData["tytul"] = title;
            return View(db.Users.ToList().Where(x => x.Clas == currentTeahcer.Clas));
        }

        public ActionResult NoteThisTask(string email)
        {
            return View(email);
        }

        // GET: SchoolTasks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SchoolTask schoolTask = db.SchoolTasks.Find(id);
            if (schoolTask == null)
            {
                return HttpNotFound();
            }
            return View(schoolTask);
        }

        // GET: SchoolTasks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SchoolTasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,title,description,whichClas")] SchoolTask schoolTask)
        {
            if (ModelState.IsValid)
            {
                string currentTeacherId = User.Identity.GetUserId();
                ApplicationUser currentTeahcer = db.Users.FirstOrDefault(x => x.Id == currentTeacherId);
                schoolTask.whichClas = currentTeahcer.Clas;
                db.SchoolTasks.Add(schoolTask);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(schoolTask);
        }

        public ActionResult Photoadd()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Photoadd(HttpPostedFileBase file)
        {
            string path = Server.MapPath("~/Images");
            string filename = Path.GetFileName(file.FileName);
            string currentTeacherId = User.Identity.GetUserId();
            ApplicationUser currentTeahcer = db.Users.FirstOrDefault(x => x.Id == currentTeacherId);
            string fn = currentTeahcer.kidMail.ToString();
            filename = fn.Substring(0,fn.IndexOf('@')) + ".jpg";
            string fullpath = Path.Combine(path, filename);
            file.SaveAs(fullpath);
            return RedirectToAction("Index","Home");
        }
        //[HttpPost]
        //[Route("uploader")]
        //public ActionResult Photoadd2(HttpPostedFileBase file)
        //{
           
            
        //        string filename = ContentDispositionHeaderValue.Parse(source.ContentDisposition).FileName.Trim('"');
        //        filename = this.EnsureCorrectFilename(filename); 
        //        var filenpath = this.GetPathAndFilename(filename);
        //        using (FileStream output = System.IO.File.Create(filenpath)) await source.CopyToAsync(output);
        //        var csv = GetCsvFromFile(filenpath);
        //        if (csv != null)
        //        { response = new JsonResult(csv); }
            
        //    return response;
        //}

        public ActionResult CheckKidNotes()
        {
            string currentParentId = User.Identity.GetUserId();
            ApplicationUser currentParent = db.Users.FirstOrDefault(x => x.Id == currentParentId);
            return View(db.Notes.ToList().Where(x => x.email == currentParent.kidMail));
        }

        // GET: SchoolTasks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SchoolTask schoolTask = db.SchoolTasks.Find(id);
            if (schoolTask == null)
            {
                return HttpNotFound();
            }
            return View(schoolTask);
        }

        // POST: SchoolTasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,title,description,whichClass")] SchoolTask schoolTask)
        {
            if (ModelState.IsValid)
            {
                db.Entry(schoolTask).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(schoolTask);
        }

        // GET: SchoolTasks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SchoolTask schoolTask = db.SchoolTasks.Find(id);
            if (schoolTask == null)
            {
                return HttpNotFound();
            }
            return View(schoolTask);
        }

        // POST: SchoolTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SchoolTask schoolTask = db.SchoolTasks.Find(id);
            db.SchoolTasks.Remove(schoolTask);
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
