using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Store.Core.Enums;
using Store.Data.Models;
using Store.Web.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Data
{
    public static class DbSeeder
    {
        public static IHost SeedDb(this IHost webHost)
        {
            using var scope = webHost.Services.CreateScope();
            try
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                roleManager.SeedRoles().Wait();

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                userManager.SeedUser().Wait();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                /*throw*/
                ;
            }
            return webHost;
        }
        public static async Task SeedRoles(this RoleManager<IdentityRole> roleManager)
        {

            if (await roleManager.Roles.AnyAsync()) return;
            var roleNames = new List<string>();
            roleNames.Add("Admin");
            roleNames.Add("Customer");
            roleNames.Add("Employee");

            foreach (var role in roleNames)
            {
                await roleManager.CreateAsync(new IdentityRole() { Name = role });
            }
        }

        public static async Task SeedUser(this UserManager<User> _userManeger)
        {
            if (await _userManeger.Users.AnyAsync())
            {
                return;
            }
            var user = new User();
            user.Name = "SystemDevloper";
            user.UserName = "dev@inf.ps";
            user.Email = "dev@inf.ps";
            user.CreatedAt = DateTime.Now;
            user.ImageUrl = "Adminn.jpeg";
            user.UserType = UserType.Admin;
            user.City = "test";
            user.PostalCode = "test";
            user.State = "test";
            user.PhoneNumber = "1234567890";
            user.StreetAddress="Test";

            await _userManeger.CreateAsync(user, "Admin$123123");
            await _userManeger.AddToRoleAsync(user, "Admin");

        }
    }
}
