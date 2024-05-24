using BookWheel.Domain.Exceptions;
using BookWheel.Domain.Repositories;
using BookWheel.Domain.Specifications.Location;
using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Application.Locations.Queries
{
    public record GetLocationTimeSlotsQuery(Guid locationId) : IRequest<IList<TimeOnly>>
    {
    }


    public class GetLocationTimeSlotsQueryHandler : IRequestHandler<GetLocationTimeSlotsQuery, IList<TimeOnly>>
    {

        public IConfiguration _configuration { get; }
        public ILocationRepository LocationRepository { get; }

        public GetLocationTimeSlotsQueryHandler(IConfiguration configuration,ILocationRepository locationRepository)
        {
            _configuration = configuration;
            LocationRepository = locationRepository;
        }

        public async Task<IList<TimeOnly>> Handle(GetLocationTimeSlotsQuery request, CancellationToken cancellationToken)
        {
            var spec = new GetLocationByIdSpecification(request.locationId);
            var locaiton = await LocationRepository.GetLocationBySpecificationAsync(spec);

            if (locaiton is null)
                throw new LocationNotFoundException(request.locationId);

            return locaiton.GetTimeSlots();
        }


        //public async Task<IList<TimeSpan>> Handle(GetLocationTimeSlotsQuery request, CancellationToken cancellationToken)
        //{
        //    using var con = new SqlConnection(_configuration.GetConnectionString("MSSQL"));

        //    var p = new { request.locationId };

        //    //TODO reservation status: only check active reservations
        //    var query = """     
        //                                 WITH RecursiveCTE AS (
        //            SELECT WorkingTimeRange_Start, WorkingTimeRange_End, WorkingTimeRange_Start AS HourlyDate
        //            FROM Location where Id = @locationId
        //            UNION ALL
        //            SELECT WorkingTimeRange_Start, WorkingTimeRange_End, DATEADD(HOUR, 1, HourlyDate)
        //            FROM RecursiveCTE
        //            WHERE HourlyDate < WorkingTimeRange_End
        //        ), WorkingHours as (
        //        SELECT HourlyDate
        //        FROM RecursiveCTE), FF as (SELECT MIN(CAST(r.ReservationTimeInterval_Start as time)) as a,HourlyDate as starttime, MIN(CAST(r.ReservationTimeInterval_End as time)) as mintime
        //                                   from WorkingHours as wh
        //                                            left join Reservation r on DATEDIFF(minute ,CAST(HourlyDate as time),
        //                                                                       CAST(r.ReservationTimeInterval_Start as time)) BETWEEN 0 AND 59
        //                                   where r.LocationId = @locationId
        //                                   group by HourlyDate), g as(
        //        select DISTINCT CASE WHEN wh.HourlyDate < mintime THEN mintime ELSE wh.HourlyDate END as b,a
        //        from WorkingHours wh left join FF on wh.HourlyDate BETWEEN FF.starttime AND FF.mintime)
        //                                 SELECT b as TimeSlots FROM(SELECT *, DATEDIFF(second ,b,LEAD(a) over( order by b)) as c  from g) as j where c<>0 OR c IS NULL
        //        """;

        //    var timeSlots = await con.QueryAsync<TimeSpan>(query,p);


        //    return timeSlots
        //        .Select(x=>new TimeSpan(x.Hours,x.Minutes,0))
        //        .ToList();
        //}
    }



}
