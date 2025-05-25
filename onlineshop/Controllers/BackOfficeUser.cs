using API.DTOs;
using Application.Commands.BackOfficeUser.Login;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using onlineshop.Features;

namespace API.Controllers
{
    [ApiController]
    [Route("BackOfficeUsers")]
    public class BackOfficeUserController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDTO input, CancellationToken cancellationToken)
        {
            var command = new LoginCommand(input.Username, input.Password);
            var token = await mediator.Send(command, cancellationToken);

            return Ok(BaseResult.Success(token));
        }
    }
}
