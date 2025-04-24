using Microsoft.AspNetCore.Mvc;
using onlineshop.DTOs;
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

            return Ok();
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken cancellationToken)
        {
            var user = await userService.GetByIdAsync(id, cancellationToken);

            return Ok(user);
        }

        [HttpGet("{query}")]
        public async Task<IActionResult> GetUsers([FromRoute] string query,CancellationToken cancellationToken)
        {
            var entities = await userService.GetListAsync(query, cancellationToken);

            return Ok(entities);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] CreateOrUpdateUserDTO input, CancellationToken cancellationToken)
        {
            await userService.UpdateAsync(id, input, cancellationToken);

            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
        {
            await userService.DeleteAsync(id, cancellationToken);

            return Ok();
        }

        [HttpPut("{id:int}/Activate")]
        public async Task<IActionResult> Activate([FromRoute] int id, CancellationToken cancellationToken)
        {
            await userService.ToggleActivationAsync(id, cancellationToken);

            return Ok();
        }

        [HttpPut("{id:int}/DeActivate")]
        public async Task<IActionResult> DeActivate([FromRoute] int id, CancellationToken cancellationToken)
        {
            await userService.ToggleActivationAsync(id, cancellationToken);

            return Ok();
        }
    }
}
