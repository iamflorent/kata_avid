using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistence.Identity;

namespace Infrastructure.Persistence.Data;

public class ApplicationDbContextSeed
{    
    public static Ad adAwaitingPublish = new Ad()
    {
        UserId = AppIdentityDbContextSeed.DefaultUserId,
        Location = "Nice",
        PropertyType = PropertyType.Apartment,
        Title = "Jolie appart AwaitingValidation",
        Price = 250000,
        Status = Status.AwaitingValidation
    };

    public static Ad adHousePublished = new Ad()
    {
        UserId = AppIdentityDbContextSeed.DefaultUserId,
        Location = "Nice",
        PropertyType = PropertyType.House,
        Title = "Jolie maison",
        Price = 400000,
        Status = Status.Published
    };

    public static Ad adAppartPublished = new Ad()
    {
        UserId = AppIdentityDbContextSeed.DefaultUserId,
        Location = "Pekin",
        PropertyType = PropertyType.Apartment,
        Title = "Bel Appart",
        Price = 200000,
        Status = Status.Published
    };

    public static Ad adGaragePublished = new Ad()
    {
        UserId = AppIdentityDbContextSeed.DefaultUserId,
        Location = "Saint-Denis",
        PropertyType = PropertyType.Garage,
        Title = "Grand garage",
        Price = 10000,
        Status = Status.Published
    };

    public static Ad adGarageBadCityPublished = new Ad()
    {
        UserId = AppIdentityDbContextSeed.DefaultUserId,
        Location = "Night City MeteoCheck",
        PropertyType = PropertyType.Garage,
        Title = "Grand garage",
        Price = 10000,
        Status = Status.Published
    };

    public static Ad adAdminGaragePublished = new Ad()
    {
        UserId = AppIdentityDbContextSeed.AdminUserId,
        Location = "Miami",
        PropertyType = PropertyType.Garage,
        Title = "Admin Grand garage",
        Price = 10000,
        Status = Status.Published
    };
    
    static void AddSeedItems(ApplicationDbContext dbContext)
    {
        var lst = new List<Ad>()
        {
            adAwaitingPublish,
            adHousePublished,
            adGaragePublished,
            adAppartPublished,
            adAdminGaragePublished,
            adGarageBadCityPublished,
        };
        dbContext.Ads.AddRange(lst);
    }

    public static async Task SeedAsync(ApplicationDbContext dbContext)
    {
        AddSeedItems(dbContext);
        await dbContext.SaveChangesAsync();
    }

    public static void Seed(ApplicationDbContext dbContext)
    {
        AddSeedItems(dbContext);
        dbContext.SaveChanges();
    }
}
