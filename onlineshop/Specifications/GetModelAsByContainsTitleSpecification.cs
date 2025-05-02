using onlineshop.Features;
using onlineshop.Models;

namespace onlineshop.Specifications;

public class GetModelAsByContainsFirstNameAndLastNameSpecification : BaseSpecification<MyUser>
{
    public GetModelAsByContainsFirstNameAndLastNameSpecification(string? q, OrderType orderType)
    {
        AddCriteria(x => x.IsActive);

        if (!string.IsNullOrEmpty(q))
        {
            AddCriteria(x => x.FirstName.Contains(q) || x.LastName.Contains(q));
        }

        AddOrderBy(x => x.Id, orderType);
    }
}

