using onlineshop.Features;
using onlineshop.Models;

namespace DomainService.Specifications;

public class GetUsersWithNoTrackinCodeSpecification : BaseSpecification<MyUser>
{
    public GetUsersWithNoTrackinCodeSpecification()
    {
        AddCriteria(x => string.IsNullOrEmpty(x.TrackingCode));
    }
}
