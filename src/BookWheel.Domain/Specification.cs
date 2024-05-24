using System.Linq.Expressions;
using BookWheel.Domain.Filters;

namespace BookWheel.Domain
{
    public class Specification<T>
        
    {
        public Expression<Func<T, bool>> Criteria { get; }

        public List<Expression<Func<T, object>>> Includes { get; set; } = new();
        
        // quick workaround :(
        public List<string> StrIncludes { get; set; } = new();

        public IFilter<T>[] Filters { get; set; }


        public Specification(Expression<Func<T,bool>> criteria)
        {
            Criteria = criteria;
        }

        public void AddInclude(string includeExpression)
        {
            StrIncludes.Add(includeExpression);
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
