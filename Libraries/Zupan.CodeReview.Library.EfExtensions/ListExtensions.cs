using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Zupan.CodeReview.Dtos.Common;

namespace Zupan.CodeReview.Library.EfExtensions
{
    public static class ListExtensions
    {
        /// <summary>
        /// Page selector
        /// </summary>
        /// <typeparam name="T">The type of results</typeparam>
        /// <param name="list">The list of results</param>
        /// <param name="properties">The properties to search for</param>
        /// <param name="predicate">The predicate to search for the function</param>
        /// <returns>A pagedset of type T</returns>
        public static PagedSet<T> SelectPage<T>(this IQueryable<T> list, Paging properties, Expression<Func<T, bool>> predicate = null)
        {
            var isDescending = properties.IsDescending != null && properties.IsDescending.Value;
            var propertyOrderName = properties.PropertyToOrderBy ?? "Id";
            var index = properties.CurrentIndex;
            var pageSize = properties.HowManyPerPage;

            var queryResult =
                (predicate != null) ?
                    list
                    .Where(predicate)
                    .OrderBy(propertyOrderName, isDescending)
                    .Skip(index * pageSize)
                    .Take(pageSize)
                    .ToList()
                    :
                    list
                    .OrderBy(propertyOrderName, isDescending)
                    .Skip(index * pageSize)
                    .Take(pageSize)
                    .ToList();

            var count = predicate == null ? list.Count() : list.Count(predicate);

            var result = new PagedSet<T>()
            {
                Total = count,
                Result = queryResult
            };

            return result;
        }

        /// <summary>
        /// Method to ordey by a respective expression.
        /// </summary>
        /// <typeparam name="TSource">Source query.</typeparam>
        /// <param name="query">The query to operate.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="isDescending">If ordey is descending.</param>
        /// <returns>Entities with given order.</returns>
        public static IOrderedQueryable<TSource> OrderBy<TSource>(this IEnumerable<TSource> query, string propertyName, bool isDescending)
        {
            var entityType = typeof(TSource);
            // Create x=>x.PropName
            var propertyInfo = entityType.GetProperty(propertyName);

            var arg = Expression.Parameter(entityType, "x");
            var property = Expression.Property(arg, propertyName);

            var selector = Expression.Lambda(property, arg);
            string orderType = isDescending ? "OrderByDescending" : "OrderBy";

            var enumarableType = typeof(Queryable);
            var method = enumarableType.GetMethods()
                 .Where(m => m.Name == orderType && m.IsGenericMethodDefinition)
                 .Where(m =>
                 {
                     var parameters = m.GetParameters().ToList();
                     //Put more restriction here to ensure selecting the right overload
                     return parameters.Count == 2;//overload that has 2 parameters
                 }).Single();

            // The linq's OrderBy<TSource, TKey> has two generic types, which provided here
            MethodInfo genericMethod = method.MakeGenericMethod(entityType, propertyInfo.PropertyType);

            /*Call query.OrderBy(selector), with query and selector: x=> x.PropName
              Note that we pass the selector as Expression to the method and we don't compile it.
              By doing so EF can extract "order by" columns and generate SQL for it.*/
            var newQuery = (IOrderedQueryable<TSource>)genericMethod.Invoke(genericMethod, new object[] { query, selector });

            return newQuery;
        }
    }
}