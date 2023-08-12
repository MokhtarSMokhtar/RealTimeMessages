using API.Data;
using API.Entities;
using DatingApi.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace DatingApi.Data
{
    public class Seed
    {
        public static async Task SeedUser(UserManager<AppUser> userManager,RoleManager<AppRole> roleManager)
        {
            if (await userManager.Users.AnyAsync())
            {
                return;
            }
            var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");
            var option = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);

            var roles = new List<AppRole>
            {
                new AppRole{Name ="Member"},
                new AppRole{Name ="Admin"},
                new AppRole{Name ="Moderator"},

            };

            if(!await roleManager.Roles.AnyAsync())
            {
                foreach (var role in roles)
                {
                    await roleManager.CreateAsync(role);
                }

            }



            foreach (var user in users)
            {
                user.UserName = user.UserName.ToLower();
                await userManager.CreateAsync(user,"Pass-1234");
                await userManager.AddToRoleAsync(user, "Member");
            }
            var admin = new AppUser
            { 
                UserName = "admin",
            };
            await userManager.CreateAsync(admin, "Pass-1234");

            await userManager.AddToRolesAsync(admin, new[] { "Moderator", "Admin" });
        }
    }
}
