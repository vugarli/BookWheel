using BookWheel.Application.Locations.Dtos;
using BookWheel.Application.Reservations.Dtos;
using BookWheel.Application.Services;
using Dapper;
using FluentValidation;
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
    public record GetCurrentCustomerReservationsQuery() : IRequest<IEnumerable<ReservationDto>>;



    public class GetCurrentCustomerReservationsQueryHandler : IRequestHandler<GetCurrentCustomerReservationsQuery, IEnumerable<ReservationDto>>
    {
        public GetCurrentCustomerReservationsQueryHandler
            (
            ICurrentUserService currentUserService,
            IConfiguration configuration
            )
        {
            CurrentUserService = currentUserService;
            Configuration = configuration;
        }

        public ICurrentUserService CurrentUserService { get; }
        public IConfiguration Configuration { get; }

        public async Task<IEnumerable<ReservationDto>> Handle(GetCurrentCustomerReservationsQuery request, CancellationToken cancellationToken)
        {
            using var cnn = new SqlConnection(Configuration.GetConnectionString("MSSQL"));

            var customerId = CurrentUserService.GetCurrentUserId();

            var p = new { customerId };

            var query = """
                 SELECT Reservation.Id,
                       Reservation.LocationId,
                       Reservation.UserId,
                       Reservation.ReservationTimeInterval_Start As Start,
                       Reservation.ReservationTimeInterval_End As 'End',
                       Reservation.Status,
                       Reservation.BoxNumber,
                       Reservation.PaymentDetails_AmountDue AS AmountDue,
                       Reservation.PaymentDetails_Status AS PaymentStatus,
                       Reservation.FinishedAt,
                       Reservation.CancelledAt,
                       Ratings.Id,
                       Ratings.UserId,
                       StarCount,
                       Comment,
                       ReservationId
                FROM Reservation left join Ratings on Ratings.ReservationId = Reservation.Id
                         WHERE Reservation.UserId = @customerId 
                """;

            var dtos = await cnn
                .QueryAsync<ReservationDto, RatingDto, ReservationDto>
                (query,
                (reservation, rating) =>
                {
                    reservation.Rating = rating;
                    return reservation;
                }
                , p);
            
            return dtos;
        }
    }
}
