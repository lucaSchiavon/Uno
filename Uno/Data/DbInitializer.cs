using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uno.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Uno.Data
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context,  UserManager<ApplicationUser> userManager)
        {
            //questo serve per ricreare il DB se non vi fosse, quindi eliminando il db verrà ricreato un'altra volta
            context.Database.EnsureCreated();
            if (context.Roles.Any())
            {
                return;   // DB has been seeded
            }
            //aggiunge builtin roles
           EntityEntry<IdentityRole> AdminRole=  context.Roles.Add(new IdentityRole { Name = "Administrator" });

            EntityEntry<IdentityRole> UserRole = context.Roles.Add(new Microsoft.AspNetCore.Identity.IdentityRole { Name = "User" });
            context.SaveChanges();
            if (context.Users.Any())
            {
                return;   // DB has been seeded
            }
            //aggiunge un utente amministrativo ed uno user
            var AdminUsr = new ApplicationUser
            {
                UserName = "administrator@builtin.it",
                Email = "administrator@builtin.it",
                EmailConfirmed=true
            };
            var UserUsr = new ApplicationUser
            {
                UserName = "user@builtin.it",
                Email = "user@builtin.it",
                EmailConfirmed = true
            };
            //var result=await context.Roles.
            //UserManager<ApplicationUser> Um = new UserManager<ApplicationUser>(IUserStore<ApplicationUser>);
            if (context.Users.Any())
            {
                return;   // DB has been seeded
            }
            var taskCreateAdminUsr =  userManager.CreateAsync(AdminUsr, "Admin2020!");
            taskCreateAdminUsr.Wait();
            var taskCreateUserUsr =  userManager.CreateAsync(UserUsr, "User2020!");
            taskCreateUserUsr.Wait();

            //ApplicationUser AdmObj = context.Users.First(x => x.Email == "administrator@builtin.it");
            //ApplicationUser UsrObj = userManager.Users.First(x => x.Email == "user@builtin.it");
         
            //var user2 = new ApplicationUser { UserName = "administrator@template.it", Email = "administrator@template.it"};
            //var result =  userManager.CreateAsync(user2, "Admin2020!");


            //EntityEntry<ApplicationUser> Admin= context.Users.Add(new ApplicationUser { UserName = "administrator@template.it", Email = "administrator@template.it",NormalizedUserName= "ADMINISTRATOR@TEMPLATE.IT", NormalizedEmail="ADMINISTRATOR@TEMPLATE.IT",  EmailConfirmed = true, PasswordHash="Admin2020!"  });
            //EntityEntry<ApplicationUser> User = context.Users.Add(new ApplicationUser { UserName = "user@template.it", Email = "user@template.it", NormalizedUserName = "USER@TEMPLATE.IT", NormalizedEmail = "USER@TEMPLATE.IT", EmailConfirmed = true, PasswordHash = "User2020!" });

            //context.SaveChanges();

            if (context.UserRoles.Any())
            {
                return;   // DB has been seeded
            }
            ApplicationUser AdmObj = userManager.Users.First(x => x.Email == "administrator@builtin.it");
            ApplicationUser UsrObj = userManager.Users.First(x => x.Email == "user@builtin.it");

            IdentityUserRole<string> AdminUserRole = new IdentityUserRole<string>() { UserId = AdmObj.Id, RoleId = AdminRole.Entity.Id };
            context.UserRoles.Add(AdminUserRole);
            IdentityUserRole<string> UserUserRole = new IdentityUserRole<string>() { UserId = UsrObj.Id, RoleId = UserRole.Entity.Id };
            context.UserRoles.Add(UserUserRole);

            context.SaveChanges();
            //// Look for any users.
            //if (context.Users.Any())
            //{
            //    return;   // DB has been seeded
            //}

            //var users = new ApplicationUser[]
            //{
            //    new ApplicationUser { FirstMidName = "Carson",   LastName = "Alexander",
            //        EnrollmentDate = DateTime.Parse("2010-09-01") },
            //    new ApplicationUser { FirstMidName = "Meredith", LastName = "Alonso",
            //        EnrollmentDate = DateTime.Parse("2012-09-01") }

            //};

            //foreach (ApplicationUser s in users)
            //{
            //    context.Users.Add(s);
            //}
            //context.SaveChanges();



            //var instructors = new Instructor[]
            //{
            //    new Instructor { FirstMidName = "Kim",     LastName = "Abercrombie",
            //        HireDate = DateTime.Parse("1995-03-11") },
            //    new Instructor { FirstMidName = "Fadi",    LastName = "Fakhouri",
            //        HireDate = DateTime.Parse("2002-07-06") },
            //    new Instructor { FirstMidName = "Roger",   LastName = "Harui",
            //        HireDate = DateTime.Parse("1998-07-01") },
            //    new Instructor { FirstMidName = "Candace", LastName = "Kapoor",
            //        HireDate = DateTime.Parse("2001-01-15") },
            //    new Instructor { FirstMidName = "Roger",   LastName = "Zheng",
            //        HireDate = DateTime.Parse("2004-02-12") }
            //};

            //foreach (Instructor i in instructors)
            //{
            //    context.Instructors.Add(i);
            //}
            //context.SaveChanges();

        }
    }
}
