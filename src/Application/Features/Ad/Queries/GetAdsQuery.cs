using Application.Contracts.Identity;
using Application.Contracts.Persistence;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Application.Features.Ad.Queries
{
    public class GetAdsQuery : IRequest<IEnumerable<Domain.Entities.Ad>>
    {
        public GetAdsQuery(PropertyType? propertyType = null, string? userId = null)
        {
            UserId = userId;
            PropertyType = propertyType;
        }

        public string? UserId { get; }
        public PropertyType? PropertyType { get; }
    }


    public class GetAdsQueryHandler : IRequestHandler<GetAdsQuery, IEnumerable<Domain.Entities.Ad>>
    {
        private IApplicationDbContext _context;

        public GetAdsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            CurrentUserService = currentUserService;
        }

        public ICurrentUserService CurrentUserService { get; }

        public async Task<IEnumerable<Domain.Entities.Ad>> Handle(GetAdsQuery request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }
#warning complete unit test for user list and admin list
            Expression<Func<Domain.Entities.Ad, bool>> userFilter = request.UserId != null ?  x => x.UserId == request.UserId : y=> y.Status == Status.Published || CurrentUserService.IsInRole(nameof(Role.Administrator)) ;
            Expression<Func<Domain.Entities.Ad, bool>> propertyTypeFilter = request.PropertyType.HasValue ?  x => x.PropertyType == request.PropertyType : x=> true;

            return await _context.Ads
                .Where(propertyTypeFilter)
                .Where(userFilter)
                .ToArrayAsync();

        }
    }
}
