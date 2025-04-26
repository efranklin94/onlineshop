using onlineshop.Repositories;

namespace onlineshop.Data
{
    public interface IUnitOfWork
    {
        public Task<bool> CommitAsync(CancellationToken cancellationToken);

        public IUserRepository UserRepository { get; init; }
    }
}
