using AthleticTrainingWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AthleticTrainingWebApp.Controllers
{
    public class ImageController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult ShowInstructorImage(string id)
        {
            var imageData = db.Instructors.Find(id).InstructorPhoto;

            return File(imageData, "imageData/jpg");
        }

        public ActionResult ShowStudentImage(string id)
        {
            var imageData = db.Students.Find(id).StudentPhoto;

            return File(imageData, "imageData/jpg");
        }

        public ActionResult ShowSchoolImage(int id)
        {
            var imageData = db.Schools.Find(id).SchoolPhoto;

            return File(imageData, "imageData/jpg");
        }
    }
}