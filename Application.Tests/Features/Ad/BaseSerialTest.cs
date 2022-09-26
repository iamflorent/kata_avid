using Application.Contracts.Persistence;
using Application.Tests.Fixtures;
using Application.Tests.Mocks;
using Infrastructure.Persistence.Data;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Tests.Features.Ad
{
    [Collection("Serial")]
    [CollectionDefinition("Serial", DisableParallelization = true)]//CurrentUserServiceMock only work if parallelization is disable
    public abstract class BaseSerialTest : IClassFixture<ApplicationFixture>
    {
        protected readonly IMediator _mediator;
        protected readonly IApplicationDbContext ApplicationDbContext;

        protected CurrentUserServiceMock CurrentUserServiceMock { get; }
        public BaseSerialTest(ApplicationFixture applicationFixture)
        {
            _mediator = applicationFixture.ServiceProvider.GetRequiredService<IMediator>();
            ApplicationDbContext = applicationFixture.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            CurrentUserServiceMock = applicationFixture.ServiceProvider.GetRequiredService<CurrentUserServiceMock>();
        }
    }
}
