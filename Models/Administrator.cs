using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AthleticTrainingWebApp.Models
{
    public class Administrator : ApplicationUser
    {
        [Key]

        public string AdministratorId { get; set; }

        [ForeignKey("AdministratorId")]

        public ApplicationUser ApplicationUser { get; set; }

        public Administrator() { }

        public Administrator(string administratorName, string email, string phoneNumber) : base(administratorName, email, phoneNumber)
        {
            this.AdministratorId = base.Id;
        }

        public static List<Administrator> PopulateAdministrator()
        {
            List<Administrator> administratorList = new List<Administrator>();

            Administrator administrator = new Administrator("TestAdmin", "TestAdmin@wvu.edu", "111-111-1111");

            administrator = new Administrator("Nassor Khalfani", "nbkhalfani@mix.wvu.edu", "818-309-9479");
            administratorList.Add(administrator);

            administrator = new Administrator("Abdulilah Almarzouki", "aalmarzo@mix.wvu.edu", "804-304-3304");
            administratorList.Add(administrator);

            administrator = new Administrator("Mitchel Gilmore", "mpgilmore@mix.wvu.edu", "409-321-4523");
            administratorList.Add(administrator);

            return administratorList;

        }


    }
}