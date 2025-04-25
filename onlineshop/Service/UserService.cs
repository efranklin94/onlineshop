using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using onlineshop.DTOs;
using onlineshop.Models;
using onlineshop.ViewModels;

namespace onlineshop.Service
{
    public class UserService(MyDbContext db, IMemoryCache memoryCache) : IUserService
    {
        public async Task CreateAsync(CreateOrUpdateUserDTO user, CancellationToken cancellationToken)
        {
            var userEntity = MyUser.Create(user.FirstName, user.LastName, user.PhoneNumber);

            await db.AddAsync(userEntity, cancellationToken);
            await db.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(int id, CreateOrUpdateUserDTO user, CancellationToken cancellationToken)
        {
            var userEntity = await db.Users.FirstOrDefaultAsync(a => a.Id == id, cancellationToken)
                ?? throw new Exception($"user with id {id} not found.");

            userEntity.Update(user.FirstName, user.LastName, user.PhoneNumber);
            await db.SaveChangesAsync(cancellationToken);

            memoryCache.Remove(id);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var userEntity = await db.Users.FirstOrDefaultAsync(a => a.Id == id, cancellationToken)
                ?? throw new Exception($"user with id {id} not found.");

            db.Remove(userEntity);
            await db.SaveChangesAsync(cancellationToken);

            memoryCache.Remove(id);
        }

        public async Task<MyUser> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var userEntity = memoryCache.Get<MyUser>(id);

            if (userEntity is null)
            {
                userEntity = await db.Users.FirstOrDefaultAsync(a => a.Id == id, cancellationToken)
                    ?? throw new Exception($"user with id {id} not found.");

                memoryCache.Set(id, userEntity, DateTime.Now.AddSeconds(30));
            }

            return userEntity;
        }

        public async Task<List<GetUsersVM>> GetListAsync(string? query, CancellationToken cancellationToken)
        {
            var users = memoryCache.Get<List<GetUsersVM>>("UserList_" + query);

            if (users is null)
            {
                users = await db.Users
                    .Where(
                        user => query == null || query == string.Empty
                    || (user.FirstName.Contains(query) 
                    || (user.LastName.Contains(query))))
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

                memoryCache.Set("UserList_" + query, users, DateTime.Now.AddSeconds(30));
            }
            return users;
        }

        public async Task ToggleActivationAsync(int id, CancellationToken cancellationToken)
        {
            var userEntity = await db.Users.FirstOrDefaultAsync(a => a.Id == id, cancellationToken)
                ?? throw new Exception($"user with id {id} not found.");

            userEntity.SetIsActive(!userEntity.IsActive);
            await db.SaveChangesAsync(cancellationToken);

            memoryCache.Remove(id);
        }
    }
}
