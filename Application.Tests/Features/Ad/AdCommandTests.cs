using Application.Common.Exceptions;
using Application.Contracts.Identity;
using Application.Features.Ad.Commands;
using Application.Features.Ad.Queries;
using Application.Tests.Fixtures;
using Application.Tests.Mocks;
using FluentAssertions;
using FluentAssertions.Specialized;
using Infrastructure.Persistence.Data;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Application.Tests.Features.Ad
{
    public class AdCommandTests : IClassFixture<ApplicationFixture>
    {        
        private readonly IMediator _mediator;
        
        public AdCommandTests(ApplicationFixture applicationFixture)
        {
            _mediator = applicationFixture.ServiceProvider.GetRequiredService<IMediator>();
        }

        #region Create

        [Fact]
        public async Task Create_Ad_With_Default_Value_Should_Return_Id()
        {
            //prepare
            var createCommand = new CreateAdCommand()
            {
                Location = "marseille",
                PropertyType = Domain.Enums.PropertyType.House,
                Title = "Jolie maison",
                Price = 15
            };

            //act
            int id = await _mediator.Send(createCommand);

            
            id.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task Create_Ad_With_Empty_Value_Should_Return_ValidationException()
        {
            //prepare
            var command = new CreateAdCommand()
            {
                Location = string.Empty,
                Title = string.Empty,
                PropertyType = null
            };

            Func<Task> act = async () => await _mediator.Send(command);

            //assert
            ExceptionAssertions<ValidationException> exceptionAssertions = await act.Should().ThrowAsync<ValidationException>();
            exceptionAssertions.Subject.First().Errors.Should().HaveCount(4);
        }
        #endregion Create
                

        #region Get
        [Fact]
        public async Task Get_Published_Ad_Should_Return_Expected_Value()
        {
            //prepare
            var command = new GetAdQuery(ApplicationDbContextSeed.adGaragePublished.Id);

            //act
            Domain.Entities.Ad actual =  await _mediator.Send(command);

            //assert
            var expected = ApplicationDbContextSeed.adGaragePublished;
            actual?.Location.Should().Be(expected.Location);
            actual?.PropertyType.Should().Be(expected.PropertyType);
            actual?.Title.Should().Be(expected.Title);
            actual?.Status.Should().Be(expected.Status);
        }

        [Fact]
        public async Task Get_Not_Existing_Ad_Should_Throw_NotFoundException()
        {
            //prepare
            var command = new GetAdQuery(1000000);

            //act
            Func<Task> act = async () => await _mediator.Send(command);

            //assert
            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task Get_Ad_Not_Published_Should_Throw_NotFoundException()
        {
            //prepare
            var createCommand = new CreateAdCommand()
            {
                Location = "marseille",
                PropertyType = Domain.Enums.PropertyType.House,
                Title = "Jolie maison",
                Price = 15
            };
            int id = await _mediator.Send(createCommand);
            
            var command = new GetAdQuery(id);

            //act
            Func<Task> act = async () => await _mediator.Send(command);

            //assert
            await act.Should().ThrowAsync<NotFoundException>();
        }


        

        #endregion Get
    }
}
