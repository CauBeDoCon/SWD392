using Microsoft.AspNetCore.Identity;
using SWD392.DB;


namespace SWD392.Helpers
{
    public static class RoleInitializer11
    {
        public static async Task InitializeRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roleNames = { "Admin", "Customer", "Doctor", "Manager", "Staff" };

            foreach (var roleName in roleNames)
            {
                var roleExists = await roleManager.RoleExistsAsync(roleName);
                Console.WriteLine($"Role '{roleName}' exists: {roleExists}");

                if (!roleExists)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                    Console.WriteLine($"Role '{roleName}' has been created.");
                }
            }
        }
    }
}
