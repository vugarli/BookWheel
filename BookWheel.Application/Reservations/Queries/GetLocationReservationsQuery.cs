using BookWheel.Application.Reservations.Dtos;
using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Application.Reservations.Queries
{
    public record GetLocationReservationsQuery(Guid locationId)
        : IRequest<IList<ReservationDto>>
    {
    }

    public class GetLocationReservationsQueryHandler
        : IRequestHandler<GetLocationReservationsQuery, IList<ReservationDto>>
    {
        private IConfiguration _configuration { get; }
        public GetLocationReservationsQueryHandler
            (
            IConfiguration configuration
            )
        {
            _configuration = configuration;
        }


        public async Task<IList<ReservationDto>> Handle(GetLocationReservationsQuery request, CancellationToken cancellationToken)
        {
            using var cnn = new SqlConnection(_configuration.GetConnectionString("MSSQL"));

            var p = new { request.locationId };

            var query = """ SELECT * FROM dbo.Reservation WHERE LocationId = @locationId """;

            var reservationDtos = await cnn.QueryAsync<ReservationDto>(query, p);
            return reservationDtos.ToList();
        }
    }



}
