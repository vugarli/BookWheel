using BookWheel.Application.Exceptions;
using BookWheel.Application.Locations.Dtos;
using BookWheel.Application.Reservations.Dtos;
using BookWheel.Application.Services;
using Dapper;
using FluentValidation;
using HybridModelBinding;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using System.Threading.Tasks;

namespace BookWheel.Application.Reservations.Queries
{
    public class GetCurrentCustomerReservationByIdQuery
        : IRequest<ReservationDto>
    {
        [HybridBindProperty(Source.Route)]
        [Obsolete]
        public Guid ReservationId { get; set; }
    }



    public class GetCurrentCustomerReservationByIdQueryValidator
        : AbstractValidator<GetCurrentCustomerReservationByIdQuery>
    {
        public GetCurrentCustomerReservationByIdQueryValidator()
        {
            RuleFor(r=>r.ReservationId).NotEmpty().NotEqual(Guid.Empty).WithMessage("Reservation Id is invalid!");
        }
    }


    public class GetCurrentCustomerReservationByIdQueryHandler
        : IRequestHandler<GetCurrentCustomerReservationByIdQuery, ReservationDto>
    {
        public GetCurrentCustomerReservationByIdQueryHandler
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

        public async Task<ReservationDto> Handle(GetCurrentCustomerReservationByIdQuery request, CancellationToken cancellationToken)
        {
            using var cnn = new SqlConnection(Configuration.GetConnectionString("MSSQL"));

            var customerId = CurrentUserService.GetCurrentUserId();

            var p = new { customerId, request.ReservationId};

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
                         WHERE Reservation.UserId = @customerId AND Reservation.Id = @ReservationId
                """;

            var reservationDto = (await cnn
                .QueryAsync<ReservationDto, RatingDto, ReservationDto>
                (query,
                (reservation, rating) => 
                { reservation.Rating = rating;
                    return reservation;
                }
                ,p)
                ).FirstOrDefault();

            if (reservationDto is null)
                throw new ReservationNotFoundException();

            return reservationDto;
        }
    }
}
