using DomainModel.Models;
using DomainService;
using DomainService.Repositories;
using Microsoft.EntityFrameworkCore;
using onlineshop.Data;
using onlineshop.Features;
using onlineshop.Helpers;

namespace Infrastructure.Repositories;

public class BackOfficeUserRepository(MyDbContext db, ICurrentUser currentUser) : IBackOfficeUserRepository
{
    private readonly DbSet<BackOfficeUser> set = db.Set<BackOfficeUser>();

    public async Task<BackOfficeUser?> GetAsync(BaseSpecification<BackOfficeUser> specification, CancellationToken cancellationToken)
    {
        var query = GetGeneralQueryable().Include(x => x.Roles).ThenInclude(x => x.Permissions).Specify(specification);

        var backOfficeUser = await query.FirstOrDefaultAsync(cancellationToken);

        return backOfficeUser;
    }

    private IQueryable<BackOfficeUser> GetQueryable(IQueryable<BackOfficeUser> query)
    {
        if (currentUser.HasGodAccess)
        {
            query = query.IgnoreQueryFilters();
        }

        return query;
    }

    private IQueryable<BackOfficeUser> GetGeneralQueryable()
    {
        var query = set.AsQueryable();

        return GetQueryable(query);
    }


    public void Update(BackOfficeUser backOfficeUser)
    {
        set.Update(backOfficeUser);
    }
}