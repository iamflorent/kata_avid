using Infrastructure.Persistence.Identity;
using Swashbuckle.AspNetCore.Filters;
using WebApi.ApiModels;

namespace WebApi.Controllers.Examples
{
    public class AuthenticateRequestExample : IMultipleExamplesProvider<AuthenticateRequest>
    {
        IEnumerable<SwaggerExample<AuthenticateRequest>> IMultipleExamplesProvider<AuthenticateRequest>.GetExamples()
        {
            yield return new SwaggerExample<AuthenticateRequest>()
            {
                Description = "simple user",
                Name = "simple user",
                Value = new AuthenticateRequest()
                {
                    Password = AppIdentityDbContextSeed.DEFAULT_PASSWORD,
                    Username = AppIdentityDbContextSeed.DefaultUserName
                },
            };

            yield return new SwaggerExample<AuthenticateRequest>()
            {
                Description = "admin user",
                Name = "admin user",
                Value = new AuthenticateRequest()
                {
                    Password = AppIdentityDbContextSeed.DEFAULT_PASSWORD,
                    Username = AppIdentityDbContextSeed.AdminUserName
                },
            };
        }
    }
}
