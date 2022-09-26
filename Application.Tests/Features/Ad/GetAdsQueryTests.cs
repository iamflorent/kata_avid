using Application.Contracts.Persistence;
using Application.Features.Ad.Queries;
using Application.Tests.Fixtures;
using Application.Tests.Mocks;
using Domain.Enums;
using FluentAssertions;
using Infrastructure.Persistence.Data;
using Infrastructure.Persistence.Identity;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Tests.Features.Ad
{
    

    //[Collection("Serial")]
    public class GetAdsQueryTests : BaseSerialTest
    {
        
        public GetAdsQueryTests(ApplicationFixture applicationFixture) : base(applicationFixture)
        {
            
        }

        [Fact]
        public async Task GetAds_With_Not_Admin_Role_Should_Return_Published_Ads()
        {
            //prepare            
            CurrentUserServiceMock.SetRole(Role.User);
            var command = new GetAdsQuery();

            //act
            var ads = await _mediator.Send(command);

            //assert
            ads.Where(x => x.Status == Status.AwaitingValidation).Should().HaveCount(0);
        }

        [Fact]
        public async Task GetAds_With_UserId_Should_Return_User_Ads()
        {
            //prepare
            CurrentUserServiceMock.SetRole(Role.User);
            string userId = AppIdentityDbContextSeed.DefaultUserId;            
            var command = new GetAdsQuery(null, userId);

            //act
            var ads = await _mediator.Send(command);

            //assert
            ads.Where(x => x.UserId != userId).Should().HaveCount(0);
        }

        [Fact]        
        public async Task GetAds_With_Admin_Role_Should_Return_Ads()
        {
            //prepare            
            CurrentUserServiceMock.SetRole(Role.Administrator);
            var command = new GetAdsQuery();

            //act
            var ads = await _mediator.Send(command);

            //assert
            int expected = ApplicationDbContext.Ads.Count();
            ads.Should().HaveCount(expected);
        }

    }
}
