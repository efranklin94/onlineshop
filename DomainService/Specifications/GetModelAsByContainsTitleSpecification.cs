using onlineshop.Features;
using onlineshop.Models;

namespace onlineshop.Specifications;

public class GetUsersByContainsFirstNameAndLastNameSpecification : BaseSpecification<MyUser>
{
    public GetUsersByContainsFirstNameAndLastNameSpecification(string? q, OrderType? orderType, int? pageSize, int? pageNumber)
    {
        AddCriteria(x => x.IsActive);

        if (!string.IsNullOrEmpty(q))
        {
            AddCriteria(x => x.FirstName.Contains(q) || x.LastName.Contains(q));
        }

        if (orderType.HasValue)
        {
            AddOrderBy(x => x.Id, orderType.Value);
        }

        if (pageSize.HasValue && pageNumber.HasValue)
        {
            AddPagination(pageSize.Value, pageNumber.Value);
        }
    }
}

