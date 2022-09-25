using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Application.Contracts.Identity;

namespace Infrastructure.Persistence.Identity;

public class AppIdentityDbContextSeed
{
    public const string DefaultUserName = "demouser@aviv.com";    
    public const string AdminUserName = "admin@aviv.com";
    public const string DEFAULT_PASSWORD = "Pass@word1***";
    public static string DefaultUserId { get; private set; } = string.Empty;
    public static string AdminUserId { get; private set; } = string.Empty;
    
    static readonly SemaphoreSlim semaphoreSlim = new(1,1);
    public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        await semaphoreSlim.WaitAsync();//WebApi.Tests.Fixtures : lock & avoid  CustomWebApplicationFactory parallel seed 

        try
        {
            await roleManager.CreateAsync(new IdentityRole(nameof(Role.Administrator)));
            await roleManager.CreateAsync(new IdentityRole(nameof(Role.User)));


            var defaultUser = new ApplicationUser { UserName = DefaultUserName, Email = DefaultUserName };

            var defaultUserResult = await userManager.CreateAsync(defaultUser, DEFAULT_PASSWORD);
            defaultUser = await userManager.FindByNameAsync(DefaultUserName);
            DefaultUserId = defaultUser.Id;

            var adminUser = new ApplicationUser { UserName = AdminUserName, Email = AdminUserName };
            var adminResult = await userManager.CreateAsync(adminUser, DEFAULT_PASSWORD);
            adminUser = await userManager.FindByNameAsync(AdminUserName);
            AdminUserId = adminUser.Id;

            await userManager.AddToRoleAsync(defaultUser, nameof(Role.User));

            await userManager.AddToRoleAsync(adminUser, nameof(Role.Administrator));
            await userManager.AddToRolesAsync(adminUser, new string[2] { nameof(Role.Administrator), nameof(Role.User) });

        }        
        finally
        {
            semaphoreSlim.Release();
        }
        
    }
}
