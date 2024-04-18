using BookWheel.Domain.Value_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.UnitTests.Domain.LocationAggregate.TimeRangeTests
{
    public class TimeRange_Create
    {


        [Fact]
        public void TimeRangeThrowsExceptionWhenDatePartsNotEqual()
        {
            var start = DateTime.Now;
            var end = DateTime.Now.AddDays(1);

            void Action() => new TimeRange(start, end);

            Assert.Throws<Exception>(Action);
        }
        
        [Fact]
        public void TimeRangeThrowsExceptionWhenNotValidStartEndOrder()
        {
            var start = DateTime.Now.AddHours(1);
            var end = DateTime.Now;

            void Action() => new TimeRange(start, end);

            Assert.Throws<ArgumentException>(Action);
        }

    }
}
