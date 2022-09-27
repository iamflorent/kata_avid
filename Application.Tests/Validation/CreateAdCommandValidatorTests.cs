using Application.Features.Ad.Commands;
using Bogus;
using FluentAssertions;

namespace Application.Tests.Validation
{
    public class CreateAdCommandValidatorTests
    {
        public CreateAdCommandValidatorTests()
        {
            Validator = new CreateAdCommandValidator();            
        }

        CreateAdCommandValidator Validator { get; } = new CreateAdCommandValidator();
        Faker Faker { get; } = new Faker();


        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Price_Should_Not_Be_Negativ_Or_Equal_Zero(decimal price)
        {
            var command = new CreateAdCommand()
            {
                Location = Faker.Address.City(),
                PropertyType = Domain.Enums.PropertyType.Apartment,
                Price = price,
                Title = "test price"
            };

            var actual = Validator.Validate(command);

            actual.IsValid.Should().Be(false);            
            actual.Errors.Select(x=> x.PropertyName).Distinct().Should().HaveCount(1);
            actual.Errors.First().PropertyName.Should().Be(nameof(CreateAdCommand.Price));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Title_Should_Not_Be_Null_Or_Empty(string title)
        {
            var command = new CreateAdCommand()
            {
                Location = Faker.Address.City(),
                PropertyType = Domain.Enums.PropertyType.Apartment,
                Price = 10,
                Title = title
            };

            var actual = Validator.Validate(command);

            actual.IsValid.Should().Be(false);
            actual.Errors.Select(x => x.PropertyName).Distinct().Should().HaveCount(1);
            actual.Errors.First().PropertyName.Should().Be(nameof(CreateAdCommand.Title));
        }

        [Fact]
        public void Title_Should_Not_Be_Greater_Than_100_Char()
        {
            int lenght = 100 + 1;
            var command = new CreateAdCommand()
            {
                Location = Faker.Address.City(),
                PropertyType = Domain.Enums.PropertyType.Apartment,
                Price = 10,
                Title = Faker.Random.String(lenght, lenght)
            };

            var actual = Validator.Validate(command);

            actual.IsValid.Should().Be(false);
            actual.Errors.Select(x => x.PropertyName).Distinct().Should().HaveCount(1);
            actual.Errors.First().PropertyName.Should().Be(nameof(CreateAdCommand.Title));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(100)]
        public void Title_Should_Be_Between_1_And_100_Char(int titleLenght)
        {
            
            var command = new CreateAdCommand()
            {
                Location = Faker.Address.City(),
                PropertyType = Domain.Enums.PropertyType.Apartment,
                Price = 10,
                Title = Faker.Random.String(titleLenght, titleLenght)
            };

            var actual = Validator.Validate(command);

            actual.IsValid.Should().Be(true);
        }
    }
}
