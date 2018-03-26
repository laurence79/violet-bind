using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace VioletBind
{
    /// <summary>
    /// Property paths extension methods.
    /// </summary>
    /// <typeparam name="TSource">The type which starts the property path</typeparam>
    public static class PropertyPaths<TSource>
    {
        /// <summary>
        /// Get the property paths in the specified expression.
        /// </summary>
        /// <returns>The paths.</returns>
        /// <param name="expression">Expression.</param>
        /// <typeparam name="TResult">The 1st type parameter.</typeparam>
        public static IReadOnlyList<PropertyPath> Get<TResult>(Expression<Func<TSource, TResult>> expression)
        {
            var visitor = new PropertyVisitor();
            visitor.Visit(expression.Body);
            return visitor.Paths;
        }

        private class PropertyVisitor : ExpressionVisitor
        {
            private readonly List<List<PropertyInfo>> _paths = new List<List<PropertyInfo>>();

            private List<PropertyInfo> _currentPath;

            public IReadOnlyList<PropertyPath> Paths
            {
                get
                {
                    return _paths.Select(p =>
                    {
                        return new PropertyPath(p.ToArray().Reverse().ToArray());
                    }).ToList();
                }
            }

            public override Expression Visit(Expression node)
            {
                if (node.NodeType != ExpressionType.MemberAccess)
                {
                    _currentPath = null;
                }

                return base.Visit(node);
            }

            protected override Expression VisitMember(MemberExpression node)
            {
                if (node.Member is PropertyInfo pi)
                {
                    if (_currentPath == null)
                    {
                        _currentPath = new List<PropertyInfo>();
                        _paths.Add(_currentPath);
                    }

                    _currentPath.Add(pi);
                }

                return base.VisitMember(node);
            }
        }
    }
}
