using Microsoft.EntityFrameworkCore;
using onlineshop.Models;

namespace onlineshop.Repositories
{
    public class UserRepository(MyDbContext db) : IUserRepository
    {
        private readonly DbSet<MyUser> set = db.Set<MyUser>();

        public async Task AddAsync(MyUser modelA, CancellationToken cancellationToken)
        {
            await set.AddAsync(modelA, cancellationToken);
        }

        public void Update(MyUser modelA)
        {
            set.Update(modelA);
        }

        public void Delete(MyUser modelA)
        {
            set.Remove(modelA);
        }

        public async Task<MyUser?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await set.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<List<MyUser>> GetListAsync(CancellationToken cancellationToken)
        {
            return await set.AsNoTracking().ToListAsync(cancellationToken);
        }
    }
}
