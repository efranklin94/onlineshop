using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using onlineshop.Data;
using onlineshop.Models;

namespace onlineshop.Repositories
{
    public class UserRepository(MyDbContext db) : IUserRepository
    {
        private readonly DbSet<MyUser> set = db.Set<MyUser>();

        public async Task AddAsync(MyUser user, CancellationToken cancellationToken)
        {
            await set.AddAsync(user, cancellationToken);
        }

        public void Update(MyUser user)
        {
            set.Update(user);
        }

        public void Delete(MyUser user)
        {
            set.Remove(user);
        }

        public async Task<MyUser?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await set.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<List<MyUser>> GetListAsync(string? query, CancellationToken cancellationToken)
        {
            var usersQuery = db.Users.AsNoTracking();

            if (!string.IsNullOrEmpty(query))
            {
                usersQuery = usersQuery.Where(u =>
                    u.FirstName.Contains(query) ||
                    u.LastName.Contains(query)
                );
            }

            return await usersQuery.ToListAsync(cancellationToken);
        }
    }
}
