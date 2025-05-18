using onlineshop.Features;

namespace onlineshop.Helpers;

public static class SpecificationHelper
{
    public static IQueryable<TEntity> Specify<TEntity>(
        this IQueryable<TEntity> query,
        BaseSpecification<TEntity> specification)
    where TEntity : class
    {
        var queryable = query;

        if (specification.Criteria != null)
        {
            queryable = queryable.Where(specification.Criteria);
        }

        if (specification.OrderByExpression != null)
        {
            var orderedQuery = queryable.OrderBy(specification.OrderByExpression);
            queryable = orderedQuery;
        }

        if (specification.OrderByDescendingExpression != null)
        {
            var orderedQuery = queryable.OrderByDescending(specification.OrderByDescendingExpression);
            queryable = orderedQuery;
        }

        return queryable;
    }
}
