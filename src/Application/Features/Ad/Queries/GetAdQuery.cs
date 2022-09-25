using Application.Common.Exceptions;
using Application.Contracts.Identity;
using Application.Contracts.Persistence;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Ad.Queries
{
    public class GetAdQuery : IRequest<Domain.Entities.Ad>
    {
        public GetAdQuery(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
    

    public class GetAdQueryHandler : IRequestHandler<GetAdQuery, Domain.Entities.Ad>
    {
        private IApplicationDbContext _context;

        public GetAdQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            CurrentUserService = currentUserService;
        }

        public ICurrentUserService CurrentUserService { get; }

        public async Task<Domain.Entities.Ad> Handle(GetAdQuery request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            

            var result = await _context.Ads.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);


            if (result == null)
            {
                throw new NotFoundException($"Ad {request.Id} not found");
            }
            else if(result.Status == Status.AwaitingValidation)
            {
                throw new NotFoundException($"Ad {request.Id} awaiting validation");
            }

            return result;


        }
    }
}
