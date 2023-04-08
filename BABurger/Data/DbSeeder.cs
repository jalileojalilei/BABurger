using BABurger.Data;
using BABurger.Constants;
using BABurger.Models;
using Microsoft.AspNetCore.Identity;
using System;

namespace BABurger.Data
{
    public static class DbSeeder
    {
        // Identity Seed
        public static async Task SeedRolesAnAdminAsync(IServiceProvider service)
        {
            // Seed Roles
            var userManager = service.GetService<UserManager<ApplicationUser>>();
            var roleManager = service.GetService<RoleManager<IdentityRole>>();

            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.User.ToString()));

            // Creating Admin
            var user = new ApplicationUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                Name = "admin",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            var userInDb = await userManager.FindByEmailAsync(user.Email);
            if (userInDb == null)
            {
                await userManager.CreateAsync(user, "Admin@123");
                await userManager.AddToRoleAsync(user, Roles.Admin.ToString());
            }

            // Creating Test User
            var testuser = new ApplicationUser
            {
                UserName = "test@gmail.com",
                Email = "test@gmail.com",
                Name = "test",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            var userInDbTest = await userManager.FindByEmailAsync(testuser.Email);
            if (userInDbTest == null)
            {
                await userManager.CreateAsync(testuser, "Test@123");
                await userManager.AddToRoleAsync(testuser, Roles.User.ToString());
            }
        }     

        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using(var serviceScope = applicationBuilder.ApplicationServices.CreateScope()) 
            { 
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                context.Database.EnsureCreated();

                // Topping
                if(!context.Toppings.Any())
                {
                    context.Toppings.AddRange(new List<Topping>()
                    {
                        new Topping()
                        {
                            Name = "Ketchup",
                            Price = 0.50
                        },
                        new Topping()
                        {
                            Name = "Mayonnaise",
                            Price = 0.50
                        },
                        new Topping()
                        {
                            Name = "Onion Ring",
                            Price = 2.00
                        },
                        new Topping()
                        {
                            Name = "Salad",
                            Price = 3.00
                        },
                    });
                    context.SaveChanges();
                }

                // Menu
                if (!context.Menus.Any())
                {
                    context.Menus.AddRange(new List<Menu>()
                    {
                        new Menu()
                        {
                            Name = "Sodexo",
                            Price = 10.00
                        },
                        new Menu()
                        {
                            Name = "Eco",
                            Price = 15.00
                        },
                        new Menu()
                        {
                            Name = "BigMac",
                            Price = 20.00
                        },
                    });
                    context.SaveChanges();
                }
            }
        }
    }
}
