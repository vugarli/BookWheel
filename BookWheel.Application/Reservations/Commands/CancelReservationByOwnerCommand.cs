using BookWheel.Application.Services;
using BookWheel.Domain;
using BookWheel.Domain.Exceptions;
using BookWheel.Domain.Repositories;
using BookWheel.Domain.Specifications.Location;
using FluentValidation;
using HybridModelBinding;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Application.Reservations.Commands
{
    public class CancelReservationByOwnerCommand : IRequest
    {
        [FromRoute] 
        public Guid LocationId { get; set; }

        [FromRoute]
        public Guid ReservationId { get; set; }
    }


    public class CancelReservationByOwnerCommandValidator
        : AbstractValidator<CancelReservationByOwnerCommand>
    {
        public CancelReservationByOwnerCommandValidator()
        {
            RuleFor(c => c.LocationId).NotEmpty().NotNull().WithMessage("Please provide LocationId");
            RuleFor(c => c.ReservationId).NotEmpty().NotNull().WithMessage("Please provide ReservationId");
            RuleFor(c => c.ReservationId).NotEqual(Guid.Empty).WithMessage("ReservationId is invalid!");
            RuleFor(c => c.LocationId).NotEqual(Guid.Empty).WithMessage("LocationId is invalid!");
        }
    }

    public class CancelReservationByOwnerCommandHandler : IRequestHandler<CancelReservationByOwnerCommand>
    {
        public CancelReservationByOwnerCommandHandler
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

        public async Task Handle(CancelReservationByOwnerCommand request, CancellationToken cancellationToken)
        {
            var ownerId = Guid.Parse(CurrentUserService.GetCurrentUserId());

            var spec = new GetOwnerLocationByIdSpecification(request.LocationId,ownerId);

            var location = await LocationRepository.GetLocationBySpecificationAsync(spec);

            if (location is null)
                throw new LocationNotFoundException(request.LocationId);


            location.CancelReservationByOwner(request.ReservationId);

            await UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
