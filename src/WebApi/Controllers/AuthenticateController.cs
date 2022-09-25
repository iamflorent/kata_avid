using Application.Common.Exceptions;
using Application.Contracts.Identity;
using Infrastructure.Persistence.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi.ApiModels;

namespace WebApi.Controllers
{
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenClaimsService _tokenClaimsService;

        public AuthenticateController(SignInManager<ApplicationUser> signInManager,
            ITokenClaimsService tokenClaimsService)
        {
            _signInManager = signInManager;
            _tokenClaimsService = tokenClaimsService;
        }

        /// <summary>
        /// demouser@aviv.com Pass@word1
        /// </summary>
        /// <param name="request">demouser@aviv.com Pass@word1</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        [HttpPost("api/authenticate")]
        [ProducesResponseType(typeof(AuthenticateResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Authenticate(AuthenticateRequest request, CancellationToken cancellationToken = default)
        {
            var response = new AuthenticateResponse();

            var result = await _signInManager.PasswordSignInAsync(request.Username, request.Password, false, true);

           
            response.Username = request.Username;

            if (!result.Succeeded)
            {
                return Unauthorized();
            }

            response.Token = await _tokenClaimsService.GetTokenAsync(request.Username);
            return Ok(response);
        }
    }
}
