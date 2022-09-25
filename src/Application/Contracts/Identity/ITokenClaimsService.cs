namespace Application.Contracts.Identity;

public interface ITokenClaimsService
{
    Task<string> GetTokenAsync(string userName);
}