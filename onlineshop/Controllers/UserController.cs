using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using onlineshop.DTOs;
using onlineshop.Models;
using onlineshop.Models.ViewModels;

namespace onlineshop.Controllers
{
    [ApiController]
    [Route("Users")]
    public class UserController(MyDbContext dbContext) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrUpdateUserDTO input, CancellationToken cancellationToken)
        {
            var user = MyUser.Create(input.FirstName, input.LastName, input.PhoneNumber);

            await dbContext.AddAsync(user, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            
            return Ok();
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken cancellationToken)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(user => user.Id == id, cancellationToken);

            if (user == null)
            {
                return NotFound();
            }   

            return Ok(user);
        }

        [HttpGet("{query}")]
        public async Task<IActionResult> GetUsers([FromRoute] string query,CancellationToken cancellationToken)
        {
            var users = await dbContext.Users
            .Where(user => user.FirstName.Contains(query) || user.LastName.Contains(query))
            .Select(user => new GetUsersVM 
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                IsActive = user.IsActive,
                FullName = user.FirstName + " " + user.LastName
            })
            .ToListAsync(cancellationToken);

            return Ok(users);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] CreateOrUpdateUserDTO input, CancellationToken cancellationToken)
        {
            var user = await dbContext.Users.FindAsync(id, cancellationToken);
            if (user == null)
            {
                return NotFound();
            }

            user.Update(input.FirstName, input.LastName, input.PhoneNumber);
            await dbContext.SaveChangesAsync(cancellationToken);

            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
        {
            var user = await dbContext.Users.FindAsync(id ,cancellationToken); 
            if (user == null) 
            { 
                return NotFound(); 
            }

            dbContext.Users.Remove(user);
            await dbContext.SaveChangesAsync(cancellationToken);

            return Ok();
        }

        [HttpPut("{id:int}/Activate")]
        public async Task<IActionResult> Activate([FromRoute] int id, CancellationToken cancellationToken)
        {
            var user = await dbContext.Users.FindAsync(id, cancellationToken);
            if (user == null)
            {
                return NotFound();
            }

            user.Activate();
            await dbContext.SaveChangesAsync(cancellationToken);

            return Ok();
        }

        [HttpPut("{id:int}/DeActivate")]
        public async Task<IActionResult> DeActivate([FromRoute] int id, CancellationToken cancellationToken)
        {
            var user = await dbContext.Users.FindAsync(id, cancellationToken);
            if (user == null)
            {
                return NotFound();
            }

            user.DeActivate();
            await dbContext.SaveChangesAsync(cancellationToken);

            return Ok();
        }
    }
}
