using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedRoles(RoleManager<AppRole> roleManager,DataContext context,UserManager<AppUser> userManager)
        {
         


            var roles = new List<AppRole>
            {
                new AppRole{Name = "Member"},
                new AppRole{Name = "Admin"},
                new AppRole{Name = "Moderator"},
            };
            var categories = new List<Category>
            {
                new Category{CategoryName="Computers"},
                new Category{CategoryName="Mobile Phones"},
                 new Category{CategoryName="Headphone"},
                 new Category{CategoryName="Games"},
                 new Category{CategoryName="Programming"}
             };
            if(context.Category.Count()==0)
            {
                foreach (var category in categories)
             {
                await context.Category.AddAsync(category);
                await context.SaveChangesAsync();
             }
            }
             

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            var admin = new AppUser
            {
                UserName="admin"
            };

            await userManager.CreateAsync(admin, "14021998");
            await userManager.AddToRolesAsync(admin, new[] {"Admin", "Moderator"});

        }
    }
}