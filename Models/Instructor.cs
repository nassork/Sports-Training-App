using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace AthleticTrainingWebApp.Models
{
    public class Instructor : ApplicationUser
    {
        [Key]
        public string InstructorId { get; set; }
        public string ISport { get; set; }

        public byte[] InstructorPhoto { get; set; }

        [ForeignKey("InstructorId")]

        public ApplicationUser ApplicationUser { get; set; }

        public override string GetUserDetails()
        {
            string userDetails = base.GetUserDetails();

            userDetails += "Sport" + ISport;

            return userDetails;

        }


        public Instructor() { }

        public Instructor(string iLName, string email, string phoneNumber, string iSport) : base(iLName, email, phoneNumber)
        {
            this.InstructorId = base.Id;
            this.ISport = iSport;
            

        }


     


        public static List<Instructor> PopulateInstructors()
        {
            List<Instructor> instructorList = new List<Instructor>();

            Instructor instructor = new Instructor("TestInstructor", "TestInstructor@wvu.edu", "304.555.0203", "Football");
            instructorList.Add(instructor);

            instructor = new Instructor("Peace", "peace@wvu.edu", "305-213-4576", "Soccer");
            instructorList.Add(instructor);

            instructor = new Instructor("Kevin Durant", "KevinDurant@wvu.edu", "312-955-0293", "Basketball");
            instructorList.Add(instructor);

            instructor = new Instructor("James Oliver", "JamesOliver@wvu.edu", "312-955-0293", "Tennis");
            instructorList.Add(instructor);

            instructor = new Instructor("Kobe Bryant", "KobeChampionshipBryant@wvu.edu", "555-555-555", "Basketball");
            instructorList.Add(instructor);

            instructor = new Instructor("Randy Moss", "RandyMoss2@wvu.edu", "102-352-2233", "Football");
            instructorList.Add(instructor);

            instructor = new Instructor("Kevin Long", "Kevin.Long@wvu.edu", "657-353-2322", "Ping-Pong");
            instructorList.Add(instructor);

            return instructorList;
        }

        public static List<Instructor> SearchInstructorsBySport(string sport)
        {
            ApplicationDbContext database = new ApplicationDbContext();
            List<Instructor> searchResult = new List<Instructor>();

            searchResult = (from instructor in database.Instructors
                            where instructor.ISport.Contains(sport)
                            select instructor ).ToList<Instructor>();


            return searchResult;
        }

        ApplicationDbContext db = new ApplicationDbContext();

        public bool? AddingInstructor(out string errorMessage)
        {
            bool? AddingSucceeded = null;
            errorMessage = "";

            try
            {
                db.Instructors.Add(this);
                db.SaveChanges();

                AddingSucceeded = true;
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
                AddingSucceeded = false;
            }

            return AddingSucceeded;

            }

        public bool? DeleteAnInstructor()
        {


            bool? instructorDeleted = null;

            try
            {
                Instructor instructor = db.Instructors.Find(this.InstructorId);
                db.Instructors.Remove(instructor);
                db.SaveChanges();

                instructorDeleted = true;

            }

            catch (Exception exception)
            {
                instructorDeleted = false;
            }

            return instructorDeleted;

        }

        public bool? EditInstructor()
        {
            bool? EditSucceeded = null;

            try
            {
                db.Entry(this).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                EditSucceeded = true;
            }
            catch
            {
                EditSucceeded = false;
            }
            return EditSucceeded;
        }


    }
}