using onlineshop.DTOs;
using onlineshop.Features;
using onlineshop.Models;
using onlineshop.ViewModels;

namespace onlineshop.Service
{
    public interface IUserService
    {
        public Task CreateAsync(CreateOrUpdateUserDTO user, CancellationToken cancellationToken);
        public Task UpdateAsync(int id, CreateOrUpdateUserDTO user, CancellationToken cancellationToken);
        public Task DeleteAsync(int id, CancellationToken cancellationToken);
        public Task<MyUser> GetByIdAsync(int id, CancellationToken cancellationToken);
        public Task<List<GetUsersVM>> GetListAsync(string? query, OrderType orderType, CancellationToken cancellationToken);
        public Task ToggleActivationAsync(int id, CancellationToken cancellationToken);
    }
}
