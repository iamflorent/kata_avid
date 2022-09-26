using Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Persistence.Identity;

public class AppIdentityDbContextSeed
{
    public const string DefaultUserName = "demouser@aviv.com";    
    public const string AdminUserName = "admin@aviv.com";
    public const string DEFAULT_PASSWORD = "Pass@word1***";
    public static string DefaultUserId { get; private set; } = string.Empty;
    public static string AdminUserId { get; private set; } = string.Empty;
    
    

    public static async Task SeedRoleAsync(RoleManager<IdentityRole> roleManager)
    {
        if (!await roleManager.RoleExistsAsync(nameof(Role.Administrator)))
        {
            await roleManager.CreateAsync(new IdentityRole(nameof(Role.Administrator)));
        }
        if (!await roleManager.RoleExistsAsync(nameof(Role.User)))
        {
            await roleManager.CreateAsync(new IdentityRole(nameof(Role.User)));
        }
    }
    public static async Task SeedAdminAsync(UserManager<ApplicationUser> userManager)
    {
        var adminUser = new ApplicationUser { UserName = AdminUserName, Email = AdminUserName };
        await userManager.CreateAsync(adminUser, DEFAULT_PASSWORD);
        adminUser = await userManager.FindByNameAsync(AdminUserName);
        AdminUserId = adminUser.Id;

        await userManager.AddToRoleAsync(adminUser, nameof(Role.Administrator));
        await userManager.AddToRolesAsync(adminUser, new string[2] { nameof(Role.Administrator), nameof(Role.User) });

    }

    public static async Task SeedUserAsync(UserManager<ApplicationUser> userManager)
    {
        var defaultUser = new ApplicationUser { UserName = DefaultUserName, Email = DefaultUserName };

        await userManager.CreateAsync(defaultUser, DEFAULT_PASSWORD);
        defaultUser = await userManager.FindByNameAsync(DefaultUserName);
        DefaultUserId = defaultUser.Id;

        await userManager.AddToRoleAsync(defaultUser, nameof(Role.User));
    }
}
