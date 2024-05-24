using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Domain.Filters
{
    public interface IFilter<T>
    {
        public IQueryable<T> Apply(IQueryable<T> query);

    }
}
