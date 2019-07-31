using Microsoft.AspNetCore.Identity;
using WebAppTry3.Models;


namespace WebAppTry3
{
    public static class UserAndRoleDataInitializer
    {
        
        public static void SeedData(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        private static void SeedUsers(UserManager<User> userManager)
        {
            if (userManager.FindByEmailAsync("user@gmail.com").Result == null)
            {
                User user = new User();
                user.UserName = "user@gmail.com";
                user.Email = "user@gmail.com";
                user.FullName = "user@gmail.com";

                IdentityResult result = userManager.CreateAsync(user, "userpass").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "user").Wait();
                }
            }


            if (userManager.FindByEmailAsync("admin@gmail.com").Result == null)
            {
                User user = new User();
                user.UserName = "admin@gmail.com";
                user.Email = "admin@gmail.com";
                user.FullName = "admin@gmail.com";

                IdentityResult result = userManager.CreateAsync(user, "adminpass").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "admin").Wait();
                }
            }
            
        }

        private static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("user").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "user";
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }


            if (!roleManager.RoleExistsAsync("admin").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "admin";
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }
        }
        
    }
}