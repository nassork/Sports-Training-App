using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace AthleticTrainingWebApp.Models
{
    public class Lesson
    {
        [Key]
        public int LId { get; set; }

        public string StudentId { get; set; }

        public string InstructorId { get; set; }

        public int SchoolId { get; set; }

        public double LessonFee { get; set; }

        public string LessonLength { get; set; }

        public string LessonSport { get; set; }

        public DateTime LessonDate { get; set; }

        [ForeignKey("StudentId")]
        public Student Student { get; set; }

        [ForeignKey("InstructorId")]
        public Instructor Instructor { get; set; }

        [ForeignKey("SchoolId")]
        public School School { get; set; }


        public Lesson() { }

        public Lesson(string instructorId, string studentId, int schoolId, DateTime lessonDate, string lessonLength, double lessonFee, string lessonSport)
        {
            this.InstructorId = instructorId;
            this.StudentId = studentId;
            this.SchoolId = schoolId;
            this.LessonDate = lessonDate;
            this.LessonLength = lessonLength;
            this.LessonFee = lessonFee;
            this.LessonSport = lessonSport;
        }

        public static List<Lesson> PopulateLessons()
        {
            List<Lesson> lessonList = new List<Lesson>();

            ApplicationDbContext database = new ApplicationDbContext();

            List<Instructor> instructorList = database.Instructors.ToList<Instructor>();
            List<Student> studentList = database.Students.ToList<Student>();

            string inst = instructorList[0].InstructorId;
            string stu = studentList[0].StudentId;

            DateTime LDate = new DateTime(2017, 3, 20);
            Lesson lesson = new Lesson(inst, null, 1, LDate, "2 Hours", 100, "Football");
            lessonList.Add(lesson);

            inst = instructorList[2].InstructorId;

            LDate = new DateTime(2017, 3, 11);
            lesson = new Lesson(inst, stu, 1, LDate, "1 Hour", 50, "Soccer");
            lessonList.Add(lesson);



            LDate = new DateTime(2017, 3, 09);
            lesson = new Lesson(inst, null, 2, LDate, "1 Hour", 60, "Soccer");
            lessonList.Add(lesson);

            inst = instructorList[1].InstructorId;
            stu = studentList[1].StudentId;

            LDate = new DateTime(2017, 9, 21);
            lesson = new Lesson(inst, stu, 3, LDate, "1 Hour", 60, "Tennis");
            lessonList.Add(lesson);

            inst = instructorList[0].InstructorId;
            stu = studentList[2].StudentId;

            LDate = new DateTime(2017, 8, 15);
            lesson = new Lesson(inst, null, 3, LDate, "2 Hour", 60, "Football");
            lessonList.Add(lesson);

            inst = instructorList[2].InstructorId;

            LDate = new DateTime(2017, 5, 07);
            lesson = new Lesson(inst, stu, 1, LDate, "2.5 Hour", 60, "Soccer");
            lessonList.Add(lesson);

            inst = instructorList[1].InstructorId;
            stu = studentList[3].StudentId;

            LDate = new DateTime(2017, 2, 13);
            lesson = new Lesson(inst, stu, 2, LDate, "1 Hour", 60, "Tennis");
            lessonList.Add(lesson);

            LDate = new DateTime(2017, 6, 12);
            lesson = new Lesson(inst, null, 3, LDate, "1 Hour", 60, "Tennis");
            lessonList.Add(lesson);

            inst = instructorList[0].InstructorId;

            LDate = new DateTime(2017, 5, 09);
            lesson = new Lesson(inst, null, 2, LDate, "2 Hour", 60, "Football");
            lessonList.Add(lesson);


            LDate = new DateTime(2017, 4, 19);
            lesson = new Lesson(inst, stu, 3, LDate, "1 Hour", 60, "Football");
            lessonList.Add(lesson);

            return lessonList;
        }
        ApplicationDbContext db = new ApplicationDbContext();
        public bool? AddLesson(out string errorMessage)
        {
            bool? LessonAdded = null;
            errorMessage = "";
            this.LessonDate = DateTime.Today;

            try
            {
                db.Lessons.Add(this);

                Lesson lesson = new Lesson();


                db.SaveChanges();

                LessonAdded = true;
                errorMessage = "None";

            }
            catch (DbEntityValidationException dbExpection)
            {
                string dbExceptionError = "";

                foreach (var entityValidationErrors in dbExpection.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        dbExceptionError += validationError.ErrorMessage;
                    }
                }

                errorMessage = dbExceptionError;
                LessonAdded = false;
            }


            catch (Exception exception)
            {
                errorMessage = exception.InnerException.ToString();
                if (errorMessage.Contains("Lessons"))
                {
                    errorMessage = "Student is already signed up for this particular lesson";
                }

                LessonAdded = false;
            }
            return LessonAdded;
        }

        public bool? DeleteLesson()
        {
            bool? LessonDeleted = null;

            try
            {
                Lesson lesson = db.Lessons.Find(this.LId);
                db.Lessons.Remove(lesson);
                db.SaveChanges();

                LessonDeleted = true;
            }
            catch (Exception exception)
            {
                LessonDeleted = false;
            }
            return LessonDeleted;
        }

        //public static List<Lesson> SearchLesson(int? lId, int? schoolId, int? instructorId)
        //{
        //    ApplicationDbContext database = new ApplicationDbContext();
        //    List<Lesson> searchResult = new List<Lesson>();



        //    //searchResult = (from lesson in database.Lessons
        //    //                where lesson.LId.Equals(lId) || lesson.SchoolId.Equals(schoolId) 
        //    //                || lesson.InstructorId.Equals(instructorId)
        //    //                select lesson).ToList<Lesson>();

        //    if (lId != null)
        //    {
        //        searchResult = searchResult.Where(le => le.LId.Equals(lId)).ToList<Lesson>();
        //    }
        //    if (schoolId != null)
        //    {
        //        searchResult = searchResult.Where(le => le.SchoolId.Equals(schoolId)).ToList<Lesson>();
        //    }
        //    if (instructorId != null)
        //    {
        //        searchResult = searchResult.Where(le => le.InstructorId.Equals(instructorId)).ToList<Lesson>();
        //    }

        //    return searchResult;
        //}



        public bool? updateLesson()
        {
            bool? updateSucceeded = null;
            try
            {
                db.Entry(this).State = EntityState.Modified;
                db.SaveChanges();
                updateSucceeded = true;
            }
            catch
            {
                updateSucceeded = false;
            }
            return updateSucceeded;
        }




        public static List<Lesson> SearchLessonBySport(string sport)
        {
            ApplicationDbContext database = new ApplicationDbContext();
            List<Lesson> searchResult = new List<Lesson>();

            searchResult = (from lesson in database.Lessons
                            where lesson.LessonSport.Contains(sport)
                            select lesson).ToList<Lesson>();

            return searchResult;
        }









    }// end class
}// end namespace