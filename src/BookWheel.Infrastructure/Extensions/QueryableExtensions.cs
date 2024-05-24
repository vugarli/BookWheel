using BookWheel.Domain.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Infrastructure.Extensions
{
    public static class QueryableExtensions
    {

        public static IQueryable<T> ApplyFilters<T>(this IQueryable<T> query,params IFilter<T>[] filters)
        {
            if (filters.Any())
                foreach (var filter in filters)
                    query = filter.Apply(query);

            return query;
        }

    }
}
