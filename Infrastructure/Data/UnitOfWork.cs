using DomainService.Repositories;
using onlineshop.Repositories;

namespace onlineshop.Data
{
    public class UnitOfWork(MyDbContext db, IUserRepository userRepository, IBackOfficeUserRepository backOfficeUserRepository) : IUnitOfWork
    {
        public async Task<bool> CommitAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await db.SaveChangesAsync(cancellationToken) > 0;
        }

        public IUserRepository UserRepository { get; init; } = userRepository; 
        public IBackOfficeUserRepository BackOfficeUserRepository { get; init; } = backOfficeUserRepository;

    }
}
