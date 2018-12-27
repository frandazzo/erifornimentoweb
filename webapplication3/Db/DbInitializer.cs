using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication3.Domain;
using WebApplication3.Domain.Security;

namespace WebApplication3.Db
{
  
    public class DbInitializer
    {

        private static void DisplayStates(IEnumerable<EntityEntry> entries)
        {
            foreach (var entry in entries)
            {
                Console.WriteLine($"Entity: {entry.Entity.GetType().Name},State: { entry.State.ToString()}");
            }
        }


        //This example just creates an Administrator role and one Admin users
        static async Task InitializeConfigurationsData(ApplicationContext context)
        {
            Configurazione c = new Configurazione()
            {
               
                Benzina = "",
                Diesel = "",
                Metano = "",
                Gpl= "",
                Gestionale = ""
            };

            context.Update(c);

           
            await context.SaveChangesAsync();
        }
        public static async Task Initialize(ApplicationContext context,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            
           // context.Database.EnsureCreated();
            context.Database.Migrate();
            // Look for any users.
            if (!context.Users.Any())
            {
                await InitializeIdentityData(userManager, roleManager);
            }

            if (!context.Configurations.Any())
            {
                await InitializeConfigurationsData(context);
            }

        }

        private static async Task InitializeIdentityData(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            bool exist = await roleManager.RoleExistsAsync("ADMIN");
            if (!exist)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "ADMIN";

                await roleManager.CreateAsync(role);
            }
            exist = await roleManager.RoleExistsAsync("USER");
            if (!exist)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "USER";

                await roleManager.CreateAsync(role);
            }

            var user1 = await userManager.FindByNameAsync("administrator");
            if (user1 == null)
            {
                User user = new User();
                user.UserName = "admin@erifornimento.it";
                user.Email = "admin@erifornimento.it";
                user.NormalizedUserName = "ADMINISTRATOR";
                user.Email = "admin@erifornimento.it";
                user.IsActive = true;
                user.EmailConfirmed = true;
                user.Name = "administrator";



                IdentityResult result = await userManager.CreateAsync
                                                    (user, "cicciobello");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user,
                                        "ADMIN");
                }
            }
        }
    }
}