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

        [Fact]
        public void TimeOnlyRangeValidDuration()
        {
            var range = new TimeOnlyRange("03:12", "05:00");

            var durationInHours = range.DurationInHours();

            Assert.True(durationInHours==2);
        }

        [Fact]
        public void TimeOnlyGetHoursValid()
        {
            var range = new TimeOnlyRange("03:12", "05:00");

            var hours = range.GetHours();

            Assert.True(hours.Count()==3);
        }


    }
}
