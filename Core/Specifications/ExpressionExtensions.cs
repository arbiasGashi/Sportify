using System.Linq.Expressions;

namespace Core.Specifications;

public static class ExpressionExtensions
{
    public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
    {
        // Combine the two expressions using a parameter replacer
        var parameter = Expression.Parameter(typeof(T));

        // Replace the parameters in the expressions to match
        var leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
        var left = leftVisitor.Visit(expr1.Body);

        var rightVisitor = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
        var right = rightVisitor.Visit(expr2.Body);

        // Create the combined expression using Expression.AndAlso
        var body = Expression.AndAlso(left!, right!);

        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }

    private class ReplaceExpressionVisitor : ExpressionVisitor
    {
        private readonly Expression _oldValue;
        private readonly Expression _newValue;

        public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
        {
            _oldValue = oldValue;
            _newValue = newValue;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return node == _oldValue ? _newValue : base.VisitParameter(node);
        }
    }
}
