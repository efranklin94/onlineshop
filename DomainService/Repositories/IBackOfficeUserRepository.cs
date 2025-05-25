using DomainModel.Models;
using onlineshop.Features;

namespace DomainService.Repositories;

public interface IBackOfficeUserRepository
{
    public Task<BackOfficeUser?> GetAsync(BaseSpecification<BackOfficeUser> specification, CancellationToken cancellationToken);
    public void Update(BackOfficeUser backOfficeUser);
}

