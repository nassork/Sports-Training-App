using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;

namespace AthleticTrainingWebApp.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string UserFullName { get; set; }

        public virtual string GetUserDetails()
        {
            string userDetails = UserFullName;

            userDetails += "is in roles ";

            ApplicationDbContext database = new ApplicationDbContext();

            IdentityRole role;

            foreach(IdentityUserRole eachRole in this.Roles)
            {
                role = database.Roles.Find(eachRole.RoleId);
                userDetails += role.Name + ' ';
            }

            return userDetails;
        }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public ApplicationUser() { }

        public ApplicationUser(string userFullName, string email, string phoneNumber)
        {
            PasswordHasher hasher = new PasswordHasher();

            this.UserFullName = userFullName;
            this.Email = email;
            this.PhoneNumber = phoneNumber;
            //required
            this.SecurityStamp = Guid.NewGuid().ToString();
            this.PasswordHash = hasher.HashPassword(userFullName);
            this.UserName = email;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Instructor> Instructors { get; set; }

        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<School> Schools { get; set; }

        public DbSet<Administrator> Administrators { get; set; }


    }
}