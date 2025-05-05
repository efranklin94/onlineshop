using Microsoft.Extensions.Caching.Memory;
using onlineshop.Data;
using onlineshop.DTOs;
using onlineshop.Exceptions;
using onlineshop.Features;
using onlineshop.Models;
using onlineshop.Specifications;
using onlineshop.ViewModels;

namespace onlineshop.Service
{
    public class UserService(IUnitOfWork unitOfWork, IMemoryCache memoryCache) : IUserService
    {
        private const string UserListCachePrefix = "UserList_";

        private static readonly List<string> cacheKeys = [];

        public async Task CreateAsync(CreateOrUpdateUserDTO user, CancellationToken cancellationToken)
        {
            var userEntity = MyUser.Create(user.FirstName, user.LastName, user.PhoneNumber, user.Email);

            await unitOfWork.UserRepository.AddAsync(userEntity, cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);

            DeleteUserListCache();
        }

        public async Task UpdateAsync(int id, CreateOrUpdateUserDTO user, CancellationToken cancellationToken)
        {
            var userEntity = await unitOfWork.UserRepository.GetByIdAsync(id, cancellationToken)
                ?? throw new NotFoundException($"user with id {id} not found.");

            userEntity.Update(user.FirstName, user.LastName, user.PhoneNumber);
            
            unitOfWork.UserRepository.Update(userEntity);
            await unitOfWork.CommitAsync(cancellationToken);

            memoryCache.Remove(id);
            DeleteUserListCache();
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var userEntity = await unitOfWork.UserRepository.GetByIdAsync(id, cancellationToken)
                ?? throw new NotFoundException($"user with id {id} not found.");

            unitOfWork.UserRepository.Delete(userEntity);
            await unitOfWork.CommitAsync(cancellationToken);

            memoryCache.Remove(id);
            DeleteUserListCache();
        }

        public async Task<MyUser> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var userEntity = memoryCache.Get<MyUser>(id);

            if (userEntity is null)
            {
                userEntity = await unitOfWork.UserRepository.GetByIdAsync(id, cancellationToken)
                    ?? throw new NotFoundException($"user with id {id} not found.");

                memoryCache.Set(id, userEntity, DateTime.Now.AddSeconds(5));
            }

            return userEntity;
        }

        public async Task<List<GetUsersVM>> GetListAsync(string? query, OrderType orderType, CancellationToken cancellationToken)
        {
            var cacheKey = $"{UserListCachePrefix}{query}{orderType}";

            var entities = memoryCache.Get<List<GetUsersVM>>(cacheKey);

            if (entities is null)
            {
                var specification = new GetModelAsByContainsFirstNameAndLastNameSpecification(query, orderType);

                var users = await unitOfWork.UserRepository.GetListAsync(specification, cancellationToken);

                entities = users.Select(UserMapToViewModel).ToList();

                memoryCache.Set(cacheKey, entities, DateTime.Now.AddSeconds(5));
                cacheKeys.Add(cacheKey);
            }
            return entities;
        }

        public async Task ToggleActivationAsync(int id, CancellationToken cancellationToken)
        {
            var userEntity = await unitOfWork.UserRepository.GetByIdAsync(id, cancellationToken)
                ?? throw new NotFoundException($"user with id {id} not found.");

            userEntity.SetIsActive(!userEntity.IsActive);

            unitOfWork.UserRepository.Update(userEntity);
            await unitOfWork.CommitAsync(cancellationToken);

            memoryCache.Remove(id);
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
