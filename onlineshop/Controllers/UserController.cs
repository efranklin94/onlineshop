using API.DTOs;
using Application.Commands.UserOption.Create;
using Application.Commands.UserOption.Delete;
using Application.Commands.UserTag.Create;
using Application.Commands.UserTag.Delete;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using onlineshop.Commands.User.Create;
using onlineshop.Commands.User.Delete;
using onlineshop.DTOs;
using onlineshop.Features;
using onlineshop.Service;

namespace onlineshop.Controllers
{
    [ApiController]
    [Route("Users")]
    public class UserController(IUserService userService, IMediator mediator) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrUpdateUserDTO input, CancellationToken cancellationToken)
        {
            var command = new CreateUserCommand(input);
            await mediator.Send(command, cancellationToken);

            return Ok(BaseResult.Success());
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken cancellationToken)
        {
            var user = await userService.GetByIdAsync(id, cancellationToken);

            return Ok(BaseResult.Success(user));
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] string? query, [FromQuery] OrderType? orderType, [FromQuery] int? pageSize, [FromQuery] int? pageNumber, CancellationToken cancellationToken)
        {
            var entities = await userService.GetListAsync(query, orderType, pageSize, pageNumber, cancellationToken);

            return Ok(BaseResult.Success(entities));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] CreateOrUpdateUserDTO input, CancellationToken cancellationToken)
        {
            await userService.UpdateAsync(id, input, cancellationToken);

            return Ok(BaseResult.Success());
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
        {
            var command = new DeleteUserCommand(id);
            await mediator.Send(command, cancellationToken);

            return Ok(BaseResult.Success());
        }

        [HttpPut("{id:int}/ToggleActivation")]
        public async Task<IActionResult> ToggleActivation([FromRoute] int id, CancellationToken cancellationToken)
        {
            await userService.ToggleActivationAsync(id, cancellationToken);

            return Ok(BaseResult.Success());
        }

        [HttpPost("{id:int}/Options")]
        public async Task<IActionResult> CreateOption([FromRoute] int id, [FromBody] CreateUserOptionDTO input, CancellationToken cancellationToken)
        {
            var command = new CreateUserOptionCommand(id, input.Description);
            await mediator.Send(command, cancellationToken);

            return Ok(BaseResult.Success());
        }

        [HttpDelete("{id:int}/Options/{optionId:guid}")]
        public async Task<IActionResult> DeleteOption([FromRoute] int id, [FromRoute] Guid optionId, CancellationToken cancellationToken)
        {
            var command = new DeleteUserOptionCommand(id, optionId);
            await mediator.Send(command, cancellationToken);

            return Ok(BaseResult.Success());
        }

        [HttpPost("{id:int}/Tags")]
        public async Task<IActionResult> CreateTag([FromRoute] int id, [FromBody] CreateUserTagDTO input, CancellationToken cancellationToken)
        {
            var command = new CreateUserTagCommand(id, input.Title, input.Priority);
            await mediator.Send(command, cancellationToken);

            return Ok(BaseResult.Success());
        }

        [HttpDelete("{id:int}/Tags")]
        public async Task<IActionResult> DeleteTag([FromRoute] int id, [FromQuery] string title, [FromQuery] int priority, CancellationToken cancellationToken)
        {
            var command = new DeleteUserTagCommand(id, title, priority);
            await mediator.Send(command, cancellationToken);

            return Ok(BaseResult.Success());
        }
    }
}
