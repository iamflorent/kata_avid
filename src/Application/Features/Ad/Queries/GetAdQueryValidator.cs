using Application.Features.Ad.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Ad.Queries
{
    public class GetAdQueryValidator : AbstractValidator<GetAdQuery>
    {
        public GetAdQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);
        }
    }
}
