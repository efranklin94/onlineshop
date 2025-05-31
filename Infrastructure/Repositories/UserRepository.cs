using DomainService;
using Microsoft.EntityFrameworkCore;
using onlineshop.Data;
using onlineshop.Features;
using onlineshop.Helpers;
using onlineshop.Models;

namespace onlineshop.Repositories;

public class UserRepository(MyDbContext db, ICurrentUser currentUser) : IUserRepository
{
    private readonly DbSet<MyUser> set = db.Set<MyUser>();

    private IQueryable<MyUser> GetQueryable(IQueryable<MyUser> query)
    {
        if (currentUser.HasGodAccess)
        {
            query = query.IgnoreQueryFilters();
        }

        return query;
    }

    private IQueryable<MyUser> GetGeneralQueryable()
    {
        var query = set.AsQueryable();

        return GetQueryable(query);
    }

    private IQueryable<MyUser> GetReadOnlyQueryable()
    {
        var query = set.AsNoTracking().AsQueryable();

        return GetQueryable(query);
    }


    public async Task AddAsync(MyUser user, CancellationToken cancellationToken)
    {
        user.Create(currentUser.Username);

        await set.AddAsync(user, cancellationToken);
    }

    public void Update(MyUser user)
    {
        user.Update(currentUser.Username);

        set.Update(user);
    }

    public void Update(List<MyUser> users)
    {
        set.UpdateRange(users);
    }

    public void Delete(MyUser user)
    {
        user.Delete(currentUser.Username);

        set.Remove(user);
    }

    public async Task<MyUser?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await GetGeneralQueryable()
            .Include(x => x.userOptions)
            .Include(x => x.userTags)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<(int, List<MyUser>)> GetListAsync(BaseSpecification<MyUser> specification, CancellationToken cancellationToken)
    {
        var usersQuery = GetReadOnlyQueryable().AsNoTracking().Specify(specification);

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
