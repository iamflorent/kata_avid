namespace Application.Contracts.Identity;

public interface ICurrentUserService
{
    public string? Email { get; }

    Task<string?> GetUserId();
    bool IsInRole(string role);
}