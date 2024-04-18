using BookWheel.Domain.Value_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.UnitTests.Builders
{
    public static class ReservationTimeRangeProvider
    {

        /// <summary>
        /// Provides nonoverlapping test reservation times for the specified box count by replicating core datas.
        /// </summary>
        /// <param name="boxCount"></param>
        /// <returns>Overlapping timerange data</returns>
        public static TimeRange[] GetNonOverlappingReservationTimeRangesForBoxCount(int boxCount)
        {
            var data = new List<TimeRange>();

            foreach (var i in Enumerable.Range(1, boxCount))
            {
                foreach (var coreData in CoreNonOverlappingDatas)
                {
                    data.AddRange(coreData);
                }
            }

            return data.ToArray();
        }


        /// <summary>
        /// Provides overlapping test reservation times for the specified box count by replicating core datas.
        /// </summary>
        /// <param name="boxCount"></param>
        /// <returns>Overlapping timerange data</returns>
        public static TimeRange[] GetOverlappingReservationTimeRangesForBoxCount(int boxCount)
        {
            var data = new List<TimeRange>();

            foreach (var i in Enumerable.Range(1,boxCount))
            {
                foreach (var coreData in CoreOverlappingDatas)
                {
                    data.AddRange(coreData);
                }
            }
            
            return data.ToArray();
        }


        private static IEnumerable<TimeRange[]> CoreOverlappingDatas =>
        new List<TimeRange[]>
        {
            new TimeRange[] {
                new TimeRange("1/12/2023 02:21","1/12/2023 03:21"),
                new TimeRange("1/12/2023 02:21","1/12/2023 06:21"),
            },
            new TimeRange[] {
                new TimeRange("1/12/2023 05:21","1/12/2023 22:59"),
                new TimeRange("1/12/2023 02:21","1/12/2023 06:21"),
            },
        };

        private static IEnumerable<TimeRange[]> CoreNonOverlappingDatas =>
        new List<TimeRange[]>
        {
            new TimeRange[] {
                new TimeRange("1/12/2023 02:21","1/12/2023 03:21"),
                new TimeRange("1/12/2023 03:21","1/12/2023 06:21"),
            },
            new TimeRange[] {
                new TimeRange("2/12/2023 05:21","2/12/2023 06:59"),
                new TimeRange("2/12/2023 06:59","2/12/2023 07:21"),
            },
        };



    }
}
