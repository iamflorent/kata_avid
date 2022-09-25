using Application.Tests.Fixtures;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Tests.Features.Location
{
    public class GetMeteoInfoQueryTests : IClassFixture<ApplicationFixture>
    {        
        private readonly IMediator _mediator;
        public GetMeteoInfoQueryTests(ApplicationFixture applicationFixture)
        {

            _mediator = applicationFixture.ServiceProvider.GetRequiredService<IMediator>();
        }
    }
}
