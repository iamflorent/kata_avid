using Application.Contracts.Identity;
using Application.Contracts.Persistence;
using Domain.Enums;
using MediatR;

namespace Application.Features.Ad.Commands
{
    public record CreateAdCommand : IRequest<int>
    {
        public string? Title { get; init; }
        public string? Location { get; init; }
        public decimal Price { get; init; }
        public PropertyType? PropertyType { get; init; }
    }

    public class CreateAdCommandHandler : IRequestHandler<CreateAdCommand, int>
    {

        private readonly IApplicationDbContext _context;

        public CreateAdCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService) 
        {
            _context = context;
            CurrentUserService = currentUserService;
        }

        public ICurrentUserService CurrentUserService { get; }

        public async Task<int> Handle(CreateAdCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var userId = await CurrentUserService.GetUserId();

            var entity = new Domain.Entities.Ad()
            {
                UserId = userId,
                Location = request.Location,
                PropertyType = request.PropertyType ?? PropertyType.House ,
                Title = request.Title,
            };

            _context.Ads.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}
