using BookWheel.Domain;
using BookWheel.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace BookWheel.Infrastructure.Specification
{
    public static class SpecificationEvaluator
    {

        public static IQueryable<T> 
            EvaluateSpecification<T>(Specification<T> specification,IQueryable<T> queryable)
            where T : class
        {
            if (specification.Criteria is not null)
            {
                queryable = queryable.Where(specification.Criteria);
            }

            if (specification.Filters is not null)
                queryable = queryable.ApplyFilters(specification.Filters);

            if (specification.Includes.Any())
            {
                queryable = specification.Includes.Aggregate(
                queryable,
                (current, includeExpression) => current.Include(includeExpression)
                );
            }

            if (specification.StrIncludes.Any())
            {
                queryable = specification.StrIncludes.Aggregate
                    (
                        queryable,
                        (current, includestr) => current.Include(includestr)
                    );
            }

            return queryable;
        }


    }
}
