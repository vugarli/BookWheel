using BookWheel.Application.Services;
using BookWheel.Domain.Repositories;
using BookWheel.Domain.Specifications.Location;
using BookWheel.Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookWheel.Domain.Exceptions;

namespace BookWheel.Application.Reservations.Commands
{
    public class CancelReservationByCustomerCommand : IRequest
    {
        [FromRoute]
        public Guid LocationId { get; set; }

        [FromRoute]
        public Guid ReservationId { get; set; }
    }


    public class CancelReservationByCustomerCommandValidator
        : AbstractValidator<CancelReservationByCustomerCommand>
    {
        public CancelReservationByCustomerCommandValidator()
        {
            RuleFor(c => c.LocationId).NotEmpty().NotNull().WithMessage("Please provide LocationId");
            RuleFor(c => c.ReservationId).NotEmpty().NotNull().WithMessage("Please provide ReservationId");
            RuleFor(c => c.ReservationId).NotEqual(Guid.Empty).WithMessage("ReservationId is invalid!");
            RuleFor(c => c.LocationId).NotEqual(Guid.Empty).WithMessage("LocationId is invalid!");
        }
    }

    public class CancelReservationByCustomerCommandHandler
        : IRequestHandler<CancelReservationByCustomerCommand>
    {

        public CancelReservationByCustomerCommandHandler
            (
            ILocationRepository locationRepository,
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService
            )
        {
            LocationRepository = locationRepository;
            UnitOfWork = unitOfWork;
            CurrentUserService = currentUserService;
        }

        public ILocationRepository LocationRepository { get; }
        public IUnitOfWork UnitOfWork { get; }
        public ICurrentUserService CurrentUserService { get; }

        public async Task Handle(CancelReservationByCustomerCommand request, CancellationToken cancellationToken)
        {
            var spec = new GetLocationByIdSpecification(request.LocationId);
            var location = await LocationRepository.GetLocationBySpecificationAsync(spec);

            if (location is null)
                throw new LocationNotFoundException(request.LocationId);

            var userId = Guid.Parse(CurrentUserService.GetCurrentUserId());

            if(!location.ActiveReservations.Any(r=>r.Id == request.ReservationId && r.UserId == userId))
                throw new ReservationNotFoundException(request.ReservationId);

            location.CancelReservationByCustomer(request.ReservationId);
            
            await UnitOfWork.SaveChangesAsync(cancellationToken);
        }

    }
}
