using BookWheel.Domain.Value_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.UnitTests.Domain.LocationAggregate.TimeOnlyRangeTests
{
    public class TimeOnlyRange_Overlapping
    {

        [Theory]
        [MemberData(nameof(OverlappingData))]
        public void TimeOnlyRangeDoesOverlap(TimeOnlyRange A, TimeOnlyRange B)
        => Assert.True(A.DoesOverlap(B));

        [Theory]
        [MemberData(nameof(NonOverlappingData))]
        public void TimeOnlyRangeDoesNotOverlap(TimeOnlyRange A, TimeOnlyRange B)
        => Assert.False(A.DoesOverlap(B));

        public static IEnumerable<object[]> OverlappingData =>
        new List<object[]>
        {
            new object[] {
                new TimeOnlyRange("02:21","03:21"),
                new TimeOnlyRange("02:21","06:21")
            },
            new object[] {
                new TimeOnlyRange("05:21","08:59"),
                new TimeOnlyRange("06:21","09:21")
            },
        };

        public static IEnumerable<object[]> NonOverlappingData =>
        new List<object[]>
        {
            new object[] {
                new TimeOnlyRange("02:21","03:21"),
                new TimeOnlyRange("03:22","06:21")
            },
            new object[] {
                new TimeOnlyRange("05:21","23:59"),
                new TimeOnlyRange("02:21","03:21")
            },
        };

    }
}
