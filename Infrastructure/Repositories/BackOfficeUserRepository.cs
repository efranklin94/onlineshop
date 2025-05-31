using DomainModel.Models;
using DomainService.Repositories;
using Microsoft.EntityFrameworkCore;
using onlineshop.Data;
using onlineshop.Features;
using onlineshop.Helpers;

namespace Infrastructure.Repositories;

public class BackOfficeUserRepository(MyDbContext db) : IBackOfficeUserRepository
{
    private readonly DbSet<BackOfficeUser> set = db.Set<BackOfficeUser>();

    public async Task<BackOfficeUser?> GetAsync(BaseSpecification<BackOfficeUser> specification, CancellationToken cancellationToken)
    {
        var query = set.Include(x => x.Roles).ThenInclude(x => x.Permissions).Specify(specification);

        var backOfficeUser = await query.FirstOrDefaultAsync(cancellationToken);

        return backOfficeUser;
    }

    public void Update(BackOfficeUser backOfficeUser)
    {
        set.Update(backOfficeUser);
    }
}