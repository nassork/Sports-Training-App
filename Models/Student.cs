using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Validation;

namespace AthleticTrainingWebApp.Models
{
    public class Student : ApplicationUser
    {
        [Key]
        public string StudentId { get; set; }

        public DateTime DOB { get; set; }

        public int studentCredits { get; set; }

        public byte[] StudentPhoto { get; set; }

        [ForeignKey("StudentId")]

        public ApplicationUser ApplicationUser { get; set; }

        public Student() { }

        public Student(string sLName, string email, string phoneNumber, DateTime dob, int credits) : base(sLName, email, phoneNumber)
        {
            this.StudentId = base.Id;
            this.DOB = dob;
            this.studentCredits = credits;
        }

        public override string GetUserDetails()
        {
            string userDetails = base.GetUserDetails();

            userDetails += "dateofbirth" + DOB;

            return userDetails;

        }

        public static List<Student> PopulateStudents()
        {
            List<Student> studentList = new List<Student>();

            DateTime dateofbirth = new DateTime(1996, 6, 10);
            Student student = new Student("TestStudent", "TestStudentr@wvu.edu", "304.555.0203", dateofbirth, 500);
            studentList.Add(student);

            dateofbirth = new DateTime(1995, 10, 16);
            student = new Student("Jacob James", "JJ@gmail.com","821.333.6464", dateofbirth, 400);
            studentList.Add(student);

            dateofbirth = new DateTime(1995, 6, 30);
            student = new Student("Sarah James", "sjames@mix.wvu.edu", "828.319.9379", dateofbirth, 100);
            studentList.Add(student);

            dateofbirth = new DateTime(1994, 10, 11);
            student = new Student("Jeremy Oconnel", "JeremyO@aol.com", "804.364.3326", dateofbirth, 600);
            studentList.Add(student);


            return studentList;
        }

        public static List<Student> SearchStudentsByLastName(string sLName)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<Student> searchResult = new List<Student>();

            searchResult = (from student in db.Students
                            where student.UserFullName.Contains(sLName)
                            select student).ToList<Student>();


            return searchResult;
        }



        ApplicationDbContext db = new ApplicationDbContext();

        public bool? AddStudent(out string errorMessage)
        {
            bool? AddSucceeded = null;
            errorMessage = "";

            try
            {
                db.Students.Add(this);
                db.SaveChanges();

                AddSucceeded = true;
                errorMessage = "None";
            }

            catch (DbEntityValidationException dbException)
            {
                string dbExceptionError = "";
                foreach (var entityValidationErrors in dbException.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        dbExceptionError += validationError.ErrorMessage;
                    }
                }
                errorMessage = dbExceptionError;
                AddSucceeded = false;
            }

            return AddSucceeded;
        }//end AddStudent

        public  bool? DeleteAStudent()
        {


            bool? studentDeleted = null;

            try
            {
                Student student = db.Students.Find(this.StudentId);
                db.Students.Remove(student);
                db.SaveChanges();

                studentDeleted = true;

            }

            catch (Exception exception)
            {
                studentDeleted = false;
            }

            return studentDeleted;

        }

        public bool? UpdateStudent()
        {
            bool? editSucceeded = null;

            try
            {
                db.Entry(this).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                editSucceeded = true;
            }
            catch
            {
                editSucceeded = false;
            }
            return editSucceeded;
        
        }
    }// end class
}// end namespace


