using Microsoft.Extensions.Caching.Memory;
using onlineshop.Data;
using onlineshop.DTOs;
using onlineshop.Exceptions;
using onlineshop.Features;
using onlineshop.Helpers;
using onlineshop.Models;
using onlineshop.Proxies;
using onlineshop.Specifications;
using onlineshop.ViewModels;

namespace onlineshop.Service
{
    public class UserService(IUnitOfWork unitOfWork, IMemoryCache memoryCache, ITrackingCodeProxy trackingCodeProxy) : IUserService
    {
        private const string UserListCachePrefix = "UserList_";

        private static readonly List<string> cacheKeys = [];

        public async Task CreateAsync(CreateOrUpdateUserDTO user, CancellationToken cancellationToken)
        {
            var code = await trackingCodeProxy.Get(cancellationToken);

            var userEntity = MyUser.Create(user.FirstName, user.LastName, user.PhoneNumber, user.Email, code);

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

        public async Task<UserViewModel> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var userEntity = memoryCache.Get<MyUser>(id);

            if (userEntity is null)
            {
                userEntity = await unitOfWork.UserRepository.GetByIdAsync(id, cancellationToken)
                    ?? throw new NotFoundException($"user with id {id} not found.");

                memoryCache.Set(id, userEntity, DateTime.Now.AddSeconds(5));
            }

            return userEntity.ToViewModel();
        }

        public async Task<PaginationResult<UserViewModel>> GetListAsync(string? query, OrderType? orderType, int? pageSize, int? pageNumber, CancellationToken cancellationToken)
        {
            var cacheKey = $"{UserListCachePrefix}{query}{orderType}";

            var entities = memoryCache.Get<PaginationResult<UserViewModel>>(cacheKey);

            if (entities is null)
            {
                var specification = new GetUsersByContainsFirstNameAndLastNameSpecification(query, orderType, pageSize, pageNumber);

                var (totalCount, users) = await unitOfWork.UserRepository.GetListAsync(specification, cancellationToken);

                entities = PaginationResult<UserViewModel>.Create(pageSize ?? 0, pageNumber ?? 0, totalCount, users.ToViewModel());

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

        private UserViewModel UserMapToViewModel(MyUser user)
        {
            return new UserViewModel
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
