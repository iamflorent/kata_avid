using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Contracts.Persistence;

public interface IApplicationDbContext
{
    DbSet<Ad> Ads { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
