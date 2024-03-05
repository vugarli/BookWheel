using BookWheel.Application.Filters;
using BookWheel.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Application.Specifications
{
    public class Specification<T>
        where T : BaseEntity
    {
        public Expression<Func<T, bool>> Criteria { get; }

        public List<Expression<Func<T, object>>> Includes { get; set; } = new();

        public IFilter<T>[] Filters { get; set; }


        public Specification(Expression<Func<T,bool>> criteria)
        {
            Criteria = criteria;
        }

        public void AddInclude(Expression<Func<T,object>> includeExpression)
        { 
            Includes.Add(includeExpression);
        }

        public void SetFilters(params IFilter<T>[] filters)
        {
            Filters = filters;
        }



    }
}
