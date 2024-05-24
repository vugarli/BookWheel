using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Application.Locations.Queries
{
    public record GetAllLocationsQuery : IRequest<IList<LocationDto>> {}

    public class GetAllLocationsQueryHandler : IRequestHandler<GetAllLocationsQuery, IList<LocationDto>>
    {
        private IConfiguration _configuration { get; }
        public GetAllLocationsQueryHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public async Task<IList<LocationDto>> Handle(GetAllLocationsQuery request, CancellationToken cancellationToken)
        {
            using (IDbConnection cnn = new SqlConnection(_configuration.GetConnectionString("MSSQL")))
            {
                string sql = """
                    SELECT loc.Id,
                    loc.Name as LocationName,
                    OwnerId,
                    BoxCount,
                    userr.Name as OwnerName,
                    Coordinates.Lat as Lat,
                    Coordinates.Long as Long,
                        IsClosed,
                        (Select AVG(CAST(rate.StarCount AS float))
                    from Ratings rate
                    left join Reservation res
                    on rate.ReservationId = res.Id WHERE res.LocationId = loc.Id) AS Rating
                    FROM dbo.Location loc
                    left join dbo.ApplicationUserRoot userr
                    on loc.OwnerId = userr.Id
                    """;

                var locationDtos = await cnn
                    .QueryAsync<LocationDto>(sql);

                return locationDtos.ToList();
            }
        }
    }
}
