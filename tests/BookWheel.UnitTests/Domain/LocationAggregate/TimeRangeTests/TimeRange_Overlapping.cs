using BookWheel.Domain.Value_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.UnitTests.Domain.LocationAggregate.TimeRangeTests
{
    public class TimeRange_Overlapping
    {
        [Theory]
        [MemberData(nameof(OverlappingData))]
        public void TimeRangeDoesOverlap(TimeRange A,TimeRange B)
            => Assert.True(A.DoesOverlap(B));

        [Theory]
        [MemberData(nameof(NonOverlappingData))]
        public void TimeRangeDoesNotOverlap(TimeRange A, TimeRange B)
            => Assert.False(A.DoesOverlap(B));

        public static IEnumerable<object[]> OverlappingData =>
        new List<object[]>
        {
            new object[] { 
                new TimeRange("1/12/2023 02:21","1/12/2023 03:21"),
                new TimeRange("1/12/2023 02:21","1/12/2023 06:21")
            },
            new object[] { 
                new TimeRange("1/12/2023 05:21","1/12/2023 23:59"),
                new TimeRange("1/12/2023 02:21","1/12/2023 06:21")
            },
        };

        public static IEnumerable<object[]> NonOverlappingData =>
        new List<object[]>
        {
            new object[] {
                new TimeRange("1/12/2023 02:21","1/12/2023 03:21"),
                new TimeRange("1/12/2023 04:21","1/12/2023 06:21")
            },
            new object[] {
                new TimeRange("1/12/2023 03:21","1/12/2023 04:21"),
                new TimeRange("1/12/2023 02:21","1/12/2023 02:33")
            },
        };

    }
}
