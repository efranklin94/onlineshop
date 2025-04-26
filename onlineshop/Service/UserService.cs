using Microsoft.Extensions.Caching.Memory;
using onlineshop.Data;
using onlineshop.DTOs;
using onlineshop.Exceptions;
using onlineshop.Models;
using onlineshop.ViewModels;

namespace onlineshop.Service
{
    public class UserService(IUnitOfWork unitOfWork, IMemoryCache memoryCache) : IUserService
    {
        private const string UserListCachePrefix = "UserList_";

        // A list to store the keys 
        private static readonly List<string> cacheKeys = new List<string>();

        public async Task CreateAsync(CreateOrUpdateUserDTO user, CancellationToken cancellationToken)
        {
            var userEntity = MyUser.Create(user.FirstName, user.LastName, user.PhoneNumber);

            await unitOfWork.UserRepository.AddAsync(userEntity, cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);

            DeleteUserListCache();
        }

        public async Task UpdateAsync(int id, CreateOrUpdateUserDTO user, CancellationToken cancellationToken)
        {
            var userEntity = await unitOfWork.UserRepository.GetByIdAsync(id, cancellationToken)
                ?? throw new NotFoundException($"user with id {id} not found.");

            userEntity.Update(user.FirstName, user.LastName, user.PhoneNumber);
            await unitOfWork.CommitAsync(cancellationToken);

            DeleteUserListCache();
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var userEntity = await unitOfWork.UserRepository.GetByIdAsync(id, cancellationToken)
                ?? throw new NotFoundException($"user with id {id} not found.");

            unitOfWork.UserRepository.Delete(userEntity);
            await unitOfWork.CommitAsync(cancellationToken);

            DeleteUserListCache();
        }

        public async Task<MyUser> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var userEntity = memoryCache.Get<MyUser>(id);

            if (userEntity is null)
            {
                userEntity = await unitOfWork.UserRepository.GetByIdAsync(id, cancellationToken)
                    ?? throw new NotFoundException($"user with id {id} not found.");

                memoryCache.Set(id, userEntity, DateTime.Now.AddSeconds(300));
            }

            return userEntity;
        }

        public async Task<List<GetUsersVM>> GetListAsync(string? query, CancellationToken cancellationToken)
        {
            var cacheKey = $"{UserListCachePrefix}{query}";

            var entities = memoryCache.Get<List<GetUsersVM>>(cacheKey);

            if (entities is null)
            {
                var users = await unitOfWork.UserRepository.GetListAsync(query, cancellationToken);

                entities = users.Select(UserMapToViewModel).ToList();

                memoryCache.Set(cacheKey, entities, DateTime.Now.AddSeconds(300));
                cacheKeys.Add(cacheKey);
            }
            return entities;
        }

        public async Task ToggleActivationAsync(int id, CancellationToken cancellationToken)
        {
            var userEntity = await unitOfWork.UserRepository.GetByIdAsync(id, cancellationToken)
                ?? throw new NotFoundException($"user with id {id} not found.");

            userEntity.SetIsActive(!userEntity.IsActive);
            await unitOfWork.CommitAsync(cancellationToken);

            DeleteUserListCache();
        }

        private void DeleteUserListCache()
        {
            foreach (var key in cacheKeys)
            {
                memoryCache.Remove(key);
            }

            cacheKeys.Clear();
        }

        private GetUsersVM UserMapToViewModel(MyUser user)
        {
            return new GetUsersVM
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                IsActive = user.IsActive,
                FullName = $"{user.FirstName} {user.LastName}"
            };
        }
    }
}
