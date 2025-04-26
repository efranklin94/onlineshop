using onlineshop.Models;

namespace onlineshop.Repositories
{
    public interface IUserRepository
    {
        public Task AddAsync(MyUser user, CancellationToken cancellationToken);
        public void Update(MyUser user);
        public void Delete(MyUser user);
        public Task<MyUser?> GetByIdAsync(int id, CancellationToken cancellationToken);
        public Task<List<MyUser>> GetListAsync(string query, CancellationToken cancellationToken);
    }
}
