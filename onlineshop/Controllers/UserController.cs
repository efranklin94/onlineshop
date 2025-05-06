using Microsoft.AspNetCore.Mvc;
using onlineshop.DTOs;
using onlineshop.Features;
using onlineshop.Service;

namespace onlineshop.Controllers
{
    [ApiController]
    [Route("Users")]
    public class UserController(IUserService userService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrUpdateUserDTO input, CancellationToken cancellationToken)
        {
            await userService.CreateAsync(input, cancellationToken);

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
            await userService.DeleteAsync(id, cancellationToken);

            return Ok(BaseResult.Success());
        }

        [HttpPut("{id:int}/ToggleActivation")]
        public async Task<IActionResult> ToggleActivation([FromRoute] int id, CancellationToken cancellationToken)
        {
            await userService.ToggleActivationAsync(id, cancellationToken);

            return Ok(BaseResult.Success());
        }
    }
}
