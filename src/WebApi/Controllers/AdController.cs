using Application.Features.Ad.Commands;
using Application.Features.Ad.Queries;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;
using WebApi.ApiModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class AdController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AdController(IMediator mediator) => _mediator = mediator;

        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {            
            return Ok(await _mediator.Send(new GetAdsQuery()));
        }

        
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetAdQuery(id), cancellationToken);
            return Ok(new GetAdDto(result.Id, result.Title, result.Location, result.PropertyType, result.Status));
        }

        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] CreateAdDto request, CancellationToken cancellationToken)
        {
            var command = new CreateAdCommand()
            {
                Location = request.Location,
                PropertyType = request.PropertyType,
                Title = request.Title,
                Price = request.Price,
            };

            return Created(nameof(Get), await _mediator.Send(command, cancellationToken));
        }

        
        [HttpPatch("publish/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Publish(int id, CancellationToken cancellationToken)
        {
            await _mediator.Send(new PublishAdCommand(id), cancellationToken);
            return NoContent();
        }

        // DELETE api/<AdController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return Ok();
        }
    }
}
