using System.Collections;
using System.Linq.Dynamic.Core;

namespace TechChallengeDgtallab.Core.Extensions;

public static class IQueryableExtension
{
    public static IQueryable<T> FilterByProperty<T>(this IQueryable<T> query, string searchTerm, string filterBy)
    {
        var parts = filterBy.Split('.');
        if (parts.Length > 1)
        {
            var parentPropertyName = parts[0];
            var childPropertyName = parts[1];

            var parentProperty = typeof(T).GetProperty(parentPropertyName);
            if (parentProperty == null)
                return query;

            if (typeof(IEnumerable).IsAssignableFrom(parentProperty.PropertyType) &&
                parentProperty.PropertyType != typeof(string))
            {
                var predicate = $"{parentPropertyName}.Any({childPropertyName}.ToLower().Contains(@0.ToLower()))";
                query = query.Where(predicate, searchTerm);
            }
            else
            {
                var predicate = $"{parentPropertyName}.{childPropertyName}.ToLower().Contains(@0.ToLower())";
                query = query.Where(predicate, searchTerm);
            }

            return query;
        }

        var propertyInfo = typeof(T).GetProperty(filterBy);
        if (propertyInfo == null) return query;

        return propertyInfo.PropertyType == typeof(string) 
            ? query.Where($"{filterBy}.ToLower().Contains(@0.ToLower())", searchTerm) 
            : query;
    }
}