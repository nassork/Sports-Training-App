using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AthleticTrainingWebApp.Models
{
    public class School
    {
        [Key]
        public int SchoolId { get; set; }
        public string SchoolName { get; set; }
        public string SchoolAddress { get; set; }

        public byte[] SchoolPhoto { get; set; }

        public School() { }



        public School(string schoolName, string schoolAddress)
        {
            this.SchoolName = schoolName;
            this.SchoolAddress = schoolAddress;
        }

        public static List<School> PopulateSchools()
        {
            List<School> schoolList = new List<School>();

            School school = new School("University High School", "131 Bakers Ridge Rd, Morgantown, WV 26508");
            schoolList.Add(school);

            school = new School("Suncrest Middle School", "360 Baldwin St, Morgantown, WV 26505");
            schoolList.Add(school);

            school = new School("Trinity Christian School", "200 Trinity Way, Morgantown, WV 26505");
            schoolList.Add(school);

            school = new School("Morgantown HighSchool", "109 Wilson Ave, Morgantown, WV 26501");
            schoolList.Add(school);

            school = new School("South Middle School", "500 E Pkwy Dr, Morgantown, WV 26501");
            schoolList.Add(school);

            school = new School("Westwood Middle School", "670 River Rd, Morgantown, WV 26501");
            schoolList.Add(school);

            school = new School("Mylan Park Elementary School", "901 Mylan Park Ln, Morgantown, WV 26501");
            schoolList.Add(school);

            school = new School("Eastwood Elementary School", "677 201st INF/FA Memorial Way, Morgantown, WV 26505");
            schoolList.Add(school);

            return schoolList;
        }

        public static List<School> SearchSchoolByName(string schoolName)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<School> searchResult = new List<School>();

            searchResult = (from school in db.Schools
                            where school.SchoolName.Contains(schoolName)
                            select school).ToList<School>();

            return searchResult;
        }

        ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "Administrator")]
        public bool? AddingSchool(out string errorMessage)
        {
            bool? AddingSucceeded = null;
            errorMessage = "";

            try
            {
                db.Schools.Add(this);
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


        [Authorize(Roles = "Administrator")]
        public bool? DeleteSchool()
        {


            bool? schoolDeleted = null;

            try
            {
                School school = db.Schools.Find(this.SchoolId);
                db.Schools.Remove(school);
                db.SaveChanges();

                schoolDeleted = true;

            }

            catch (Exception exception)
            {
                schoolDeleted = false;
            }

            return schoolDeleted;

        }

        [Authorize(Roles = "Administrator")]
        public bool? UpdateSchool()
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
}//end namespace