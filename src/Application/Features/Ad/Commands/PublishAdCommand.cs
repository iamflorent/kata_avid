using Application.Common.Exceptions;
using Application.Contracts.Identity;
using Application.Contracts.Persistence;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Ad.Commands
{
    public class PublishAdCommand : IRequest
    {
        public PublishAdCommand(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }

    public class PublishAdCommandHandler : IRequestHandler<PublishAdCommand>
    {
        private readonly IApplicationDbContext _context;
        public ICurrentUserService CurrentUserService { get; }

        public PublishAdCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            CurrentUserService = currentUserService;
        }

        public async Task<Unit> Handle(PublishAdCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (!CurrentUserService.IsInRole(nameof(Role.Administrator)))
            {
                throw new ForbiddenAccessException();
            }

            var ad = await _context.Ads.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);


            if (ad == null)
            {
                throw new NotFoundException($"Ad {request.Id} not found");
            }

            if (ad.Status != Status.Published)
            {
                ad.Status = Status.Published;

                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
            throw new ValidationException(nameof(ad.Status), "ad status is already published");
        }
    }
}
