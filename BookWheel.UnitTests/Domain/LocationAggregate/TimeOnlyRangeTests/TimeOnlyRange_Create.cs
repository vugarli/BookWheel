using BookWheel.Domain.Value_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.UnitTests.Domain.LocationAggregate.TimeOnlyRangeTests
{
    public class TimeOnlyRange_Create
    {

        [Fact]
        public void TimeOnlyRangeThrowsExceptionIfInvalid()
        {
            void Action() => new TimeOnlyRange("03:13","02:00");

            Assert.Throws<ArgumentException>(Action);
        }


    }
}
