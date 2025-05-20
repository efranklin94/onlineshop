using onlineshop.Features;
using onlineshop.Models;
using onlineshop.ViewModels;

namespace onlineshop.Repositories;

public interface IUserRepository
{
    public Task AddAsync(MyUser user, CancellationToken cancellationToken);
    public void Update(MyUser user);
    public void Update(List<MyUser> users);
    public void Delete(MyUser user);
    public Task<MyUser?> GetByIdAsync(int id, CancellationToken cancellationToken);
    public Task<(int, List<MyUser>)> GetListAsync(BaseSpecification<MyUser> specification, CancellationToken cancellationToken);
}
