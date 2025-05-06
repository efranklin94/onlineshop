using Microsoft.EntityFrameworkCore;
using onlineshop.Data;
using onlineshop.Features;
using onlineshop.Helpers;
using onlineshop.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace onlineshop.Repositories;

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

    public async Task<(int, List<MyUser>)> GetListAsync(BaseSpecification<MyUser> specification, CancellationToken cancellationToken)
    {
        var usersQuery = db.Users.AsNoTracking().Specify(specification);

        var totalCount = 0;

        if (specification.IsPaginationEnabled)
        {
            totalCount = await usersQuery.CountAsync(cancellationToken);
            usersQuery = usersQuery.Skip(specification.Skip).Take(specification.Take);
        }

        var users = await usersQuery.ToListAsync(cancellationToken);

        return (totalCount, users);
    }
}
