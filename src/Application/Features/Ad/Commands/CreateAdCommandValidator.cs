using FluentValidation;

namespace Application.Features.Ad.Commands
{
    public class CreateAdCommandValidator : AbstractValidator<CreateAdCommand>
    {
        public CreateAdCommandValidator()
        {
            RuleFor(v => v.Title)
                .MaximumLength(100)
                .NotEmpty();

            RuleFor(v => v.Location)
                .MaximumLength(100)
                .NotEmpty();

            RuleFor(v => v.Price)
                .GreaterThan(0)
                .NotEmpty();

            RuleFor(v => v.PropertyType)
                .NotNull();

            
        }
    }
}
