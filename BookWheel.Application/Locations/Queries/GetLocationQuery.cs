using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Application.Locations.Queries
{
    public record GetLocationQuery(Guid locationId):IRequest<LocationDto>
    {
    }

    public class GetLocationQueryHandler :
        IRequestHandler<GetLocationQuery, LocationDto>
    {
        private IConfiguration _configuration { get; }
        public GetLocationQueryHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<LocationDto> Handle(GetLocationQuery request, CancellationToken cancellationToken)
        {
            using var cnn = new SqlConnection(_configuration.GetConnectionString("MSSQL"));

            var p = new {request.locationId};

            var query = """
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
                on rate.ReservationId = res.Id WHERE res.LocationId = @locationId) AS Rating
                FROM dbo.Location loc
                left join dbo.ApplicationUserRoot userr
                on loc.OwnerId = userr.Id
                WHERE loc.Id = @locationId;
                """;

            var locationDto = await cnn.QueryAsync<LocationDto>(query,p);

            return locationDto.FirstOrDefault();
        }
    }
}
