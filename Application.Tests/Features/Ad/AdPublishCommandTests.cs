using Application.Common.Exceptions;
using Application.Features.Ad.Commands;
using Application.Features.Ad.Queries;
using Application.Tests.Fixtures;
using Application.Tests.Mocks;
using Domain.Enums;
using FluentAssertions;
using FluentAssertions.Specialized;
using Infrastructure.Persistence.Data;

namespace Application.Tests.Features.Ad
{

    public class AdPublishCommandTests : BaseSerialTest
    {
        
        public AdPublishCommandTests(ApplicationFixture applicationFixture) : base(applicationFixture)
        {            
            
        }

        [Fact]
        public async Task Publish_Ad_With_Role_User_Should_Throw_ForbiddenAccessException()
        {
            //prepare            
            CurrentUserServiceMock.SetRole(Role.User);
            var command = new PublishAdCommand(ApplicationDbContextSeed.adAwaitingPublish.Id);

            //act
            Func<Task> act = async () => await _mediator.Send(command);            

            //assert
            await act.Should().ThrowAsync<ForbiddenAccessException>();

        }

        [Fact]
        public async Task Publish_Ad_With_Role_Admin_Should_Not_Throw_ForbiddenAccessException()
        {
            //prepare            
            CurrentUserServiceMock.SetRole(Role.Administrator);
            var createCommand = new CreateAdCommand()
            {
                Location = "marseille",
                PropertyType = Domain.Enums.PropertyType.House,
                Title = "Jolie maison",
                Price = 10
            };

            int id = await _mediator.Send(createCommand);
            var command = new PublishAdCommand(id);

            //act
            Func<Task> act = async () => await _mediator.Send(command);

            //assert
            await act.Should().NotThrowAsync<ForbiddenAccessException>();

        }

        [Fact]
        public async Task Publish_Not_Existing_Ad_Should_Throw_NotFoundException()
        {
            //prepare
            CurrentUserServiceMock.SetRole(Role.Administrator);
            var command = new PublishAdCommand(1000000);

            //act
            Func<Task> act = async () => await _mediator.Send(command);

            //assert
            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task Publish_Ad_With_Status_AwaitingPublish_Should_Have_Status_Published()
        {
            //prepare
            CurrentUserServiceMock.SetRole(Role.Administrator);
            var createCommand = new CreateAdCommand()
            {
                Location = "marseille",
                PropertyType = Domain.Enums.PropertyType.House,
                Title = "Jolie maison",
                Price = 10
            };

            int id = await _mediator.Send(createCommand);

            //act
            await _mediator.Send(new PublishAdCommand(id));

            //assert
            Domain.Entities.Ad actual = await _mediator.Send(new GetAdQuery(id));
            actual.Status.Should().Be(Domain.Enums.Status.Published);

        }

        [Fact]
        public async Task Publish_Ad_With_Status_Published_Should_Throw_ValidationException()
        {
            //prepare
            CurrentUserServiceMock.SetRole(Role.Administrator);
            var command = new PublishAdCommand(ApplicationDbContextSeed.adGaragePublished.Id);

            //act
            Func<Task> act = async () => await _mediator.Send(command);

            //assert
            ExceptionAssertions<ValidationException> exceptionAssertions = await act.Should().ThrowAsync<ValidationException>();
            exceptionAssertions.Subject.First().Errors.Should().HaveCount(1);
            var error = exceptionAssertions.Subject.First().Errors.First();
            error.Key.Should().Be(nameof(Domain.Entities.Ad.Status));
            error.Value.Should().HaveCount(1);
            error.Value.First().Should().Be("ad status is already published");
        }
    }
}
