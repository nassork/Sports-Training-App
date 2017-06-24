using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AthleticTrainingWebApp.Models;
using Microsoft.AspNet.Identity;

namespace AthleticTrainingWebApp.Controllers
{
    public class LessonsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpGet]
        public ViewResult SearchLesson()
        {
            //ViewBag.Lessons = new SelectList(db.Lessons, "LId", "LessonSport");
            //ViewBag.Schools = new SelectList(db.Schools, "SchoolId", "SchoolName");
            //ViewBag.Instructors = new SelectList(db.Instructors, "InstructorId", "UserFullName");

            return View();
        }

        [HttpPost]
        public ViewResult SearchLesson(string sport)
        {

            List<Lesson> lessons = Lesson.SearchLessonBySport(sport);
             

            return View("SearchLessonResult", lessons);

        }


        // GET: Lessons
        public ActionResult Index()
        {
            var lessons = db.Lessons.Include(l => l.Instructor).Include(l => l.School).Include(l => l.Student);
            return View(lessons.ToList());
        }


        [Authorize(Roles = "Student")]
        public ActionResult StudentRegistrations()
        {
            string userId = User.Identity.GetUserId();
            var lessons = db.Lessons.Include(L => L.Instructor).Include(L => L.Student).Include(L => L.School)
                .Where(L => L.StudentId == userId);


            return View("Index", lessons.ToList());
        }


        // GET: Lessons/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lesson lesson = db.Lessons.Find(id);
            if (lesson == null)
            {
                return HttpNotFound();
            }
            return View(lesson);
        }




        [Authorize(Roles = "Student,Administrator")]
        public ActionResult RegisterStudent(int? id)
        {
            // Session["LId"] = id;

            Lesson lesson = db.Lessons.Find(id);

            ActionResult actionResult = Edit(lesson);


            return actionResult;
        }



        // GET: Lessons/Create

        [Authorize(Roles = "Instructor,Administrator")]
        public ActionResult Create()
        {
            ViewBag.InstructorId = new SelectList(db.Instructors, "Id", "UserFullName");
            ViewBag.SchoolId = new SelectList(db.Schools, "SchoolId", "SchoolName");
            ViewBag.StudentId = new SelectList(db.Students, "Id", "UserFullName");
            return View();
        }

        // POST: Lessons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LId,StudentId,InstructorId,SchoolId,LessonFee,LessonLength,LessonSport,LessonDate")] Lesson lesson)
        {
            


            if (ModelState.IsValid)
            {                           

                db.Lessons.Add(lesson);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.InstructorId = new SelectList(db.Users, "Id", "UserFullName", lesson.InstructorId);
            ViewBag.SchoolId = new SelectList(db.Schools, "SchoolId", "SchoolName", lesson.SchoolId);
            ViewBag.StudentId = new SelectList(db.Users, "Id", "UserFullName", lesson.StudentId);
            return View(lesson);
        }

        // GET: Lessons/Edit/5

        [Authorize(Roles = "Instructor,Administrator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lesson lesson = db.Lessons.Find(id);
            if (lesson == null)
            {
                return HttpNotFound();
            }
            ViewBag.InstructorId = new SelectList(db.Users, "Id", "UserFullName", lesson.InstructorId);
            ViewBag.SchoolId = new SelectList(db.Schools, "SchoolId", "SchoolName", lesson.SchoolId);
            ViewBag.StudentId = new SelectList(db.Users, "Id", "UserFullName", lesson.StudentId);
            return View(lesson);
        }

        // POST: Lessons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LId,StudentId,InstructorId,SchoolId,LessonFee,LessonLength,LessonSport,LessonDate")] Lesson lesson)
        {
           // lesson.LId = (int)Session["LId"];
           // lesson.StudentId = User.Identity.GetUserId();

            if (ModelState.IsValid)
            {
                lesson.StudentId = User.Identity.GetUserId();
                db.Entry(lesson).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.InstructorId = new SelectList(db.Users, "Id", "UserFullName", lesson.InstructorId);
            ViewBag.SchoolId = new SelectList(db.Schools, "SchoolId", "SchoolName", lesson.SchoolId);
            ViewBag.StudentId = new SelectList(db.Users, "Id", "UserFullName", lesson.StudentId);
            return View(lesson);
        }

        // GET: Lessons/Delete/5

        [Authorize(Roles = "Instructor,Administrator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lesson lesson = db.Lessons.Find(id);
            if (lesson == null)
            {
                return HttpNotFound();
            }
            return View(lesson);
        }

        // POST: Lessons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Lesson lesson = db.Lessons.Find(id);
            db.Lessons.Remove(lesson);
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
