using System.Linq.Expressions;

namespace onlineshop.Features;

public class BaseSpecification<TEntity>
{
    public Expression<Func<TEntity, bool>>? Criteria { get; set; }
    public Expression<Func<TEntity, object>>? OrderByExpression { get; set; }
    public Expression<Func<TEntity, object>>? OrderByDescendingExpression { get; set; }

    protected BaseSpecification()
    {
    }

    protected void AddCriteria(Expression<Func<TEntity, bool>> predict)
    {
        if (Criteria is null)
        {
            Criteria = predict;
        }
        else
        {
            var left = Criteria.Parameters[0];
            var visitor = new ReplaceParameterVisitor(predict.Parameters[0], left);
            var right = visitor.Visit(predict.Body);
            Criteria = Expression.Lambda<Func<TEntity, bool>>(Expression.AndAlso(Criteria.Body, right), left);
        }
    }

    protected void AddOrderBy(Expression<Func<TEntity, object>> expression, OrderType orderType)
    {
        if (orderType == OrderType.Ascending)
        {
            OrderByExpression = expression;
        }
        else
        {
            OrderByDescendingExpression = expression;
        }
    }

    private class ReplaceParameterVisitor(ParameterExpression oldParameter, ParameterExpression newParameter) : ExpressionVisitor
    {
        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (ReferenceEquals(node, oldParameter))
            {
                return newParameter;
            }

            return base.VisitParameter(node);
        }
    }
}
