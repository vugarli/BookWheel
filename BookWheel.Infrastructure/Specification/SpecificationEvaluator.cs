using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookWheel.Application.Specifications;
using BookWheel.Domain.Entities;
using BookWheel.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;


namespace BookWheel.Infrastructure.Specifications
{
    public static class SpecificationEvaluator
    {

        public static IQueryable<T> 
            EvaluateSpecification<T>(Specification<T> specification,IQueryable<T> queryable)
            where T : BaseEntity
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

            return queryable;
        }


    }
}
