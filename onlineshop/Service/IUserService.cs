using onlineshop.DTOs;
using onlineshop.Models;
using onlineshop.Models.ViewModels;

namespace onlineshop.Service
{
    public interface IUserService
    {
        public Task CreateAsync(CreateOrUpdateUserDTO user, CancellationToken cancellationToken);
        public Task UpdateAsync(int id, CreateOrUpdateUserDTO user, CancellationToken cancellationToken);
        public Task DeleteAsync(int id, CancellationToken cancellationToken);
        public Task<MyUser> GetByIdAsync(int id, CancellationToken cancellationToken);
        public Task<List<GetUsersVM>> GetListAsync(string query, CancellationToken cancellationToken);
        public Task ToggleActivationAsync(int id, CancellationToken cancellationToken);
    }
}
