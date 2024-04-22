using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookWheel.Domain;

namespace BookWheel.Infrastructure.Specification
{
    public static class SpecificationEvaluatorExtensions
    {

        public static IQueryable<T> ApplySpecification<T> 
            (
            this IQueryable<T> query,
            Specification<T> spec
            ) where T : class
        {
            return SpecificationEvaluator.EvaluateSpecification(spec,query);
        }

    }
}
