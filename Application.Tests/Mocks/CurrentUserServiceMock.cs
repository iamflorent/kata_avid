using Application.Contracts.Identity;
using Domain.Enums;
using Infrastructure.Persistence.Identity;

namespace Application.Tests.Mocks
{
    public class CurrentUserServiceMock : ICurrentUserService
    {
        
        public string? Email => AppIdentityDbContextSeed.DefaultUserName;

        public async Task<string?> GetUserId()
        {
            return await Task.FromResult(AppIdentityDbContextSeed.DefaultUserId);
        }

        private Role Role { get; set; } = Role.Anonymous;

        public void SetRole(Role role) => Role = role;

        public bool IsInRole(string role)
        {            
            return Role.ToString() == role;
        }
    }
}
