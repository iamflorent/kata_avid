using Application.Features.Ad.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Location.Queries.GetMeteoInfo
{
    
    public class GetMeteoInfoQueryValidator : AbstractValidator<GetMeteoInfoQuery>
    {
        public GetMeteoInfoQueryValidator()
        {
            RuleFor(v => v.location)                
                .NotEmpty();



        }
    }
}
