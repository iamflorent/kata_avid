using Application.Features.Location.Queries.GetMeteoInfo;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController, Authorize]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IMediator _mediator;
        public WeatherForecastController(IMediator mediator) => _mediator = mediator;


        [HttpGet("{location}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<GetMeteoInfoDto> Get(string location, CancellationToken cancellationToken)
        {
            return await _mediator.Send(new GetMeteoInfoQuery(location), cancellationToken);
        }
    }
}