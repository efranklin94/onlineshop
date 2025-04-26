using onlineshop.Models;

namespace onlineshop.Repositories
{
    public interface IUserRepository
    {
        public Task AddAsync(MyUser modelA, CancellationToken cancellationToken);
        public void Update(MyUser modelA);
        public void Delete(MyUser modelA);
        public Task<MyUser?> GetByIdAsync(int id, CancellationToken cancellationToken);
        public Task<List<MyUser>> GetListAsync(CancellationToken cancellationToken);
    }
}
