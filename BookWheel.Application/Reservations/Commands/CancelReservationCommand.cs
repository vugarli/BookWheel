using BookWheel.Application.Services;
using BookWheel.Domain;
using BookWheel.Domain.Repositories;
using BookWheel.Domain.Specifications.Location;
using HybridModelBinding;
using MediatR;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Application.Reservations.Commands
{
    public class CancelReservationCommand : IRequest
    {
        [HybridBindProperty(Source.Route)]
        [Obsolete]
        public Guid LocationId { get; set; }

        [HybridBindProperty(Source.Route)]
        [Obsolete]
        public Guid ReservationId { get; set; }
    }

    public class CancelReservationCommandHandler : IRequestHandler<CancelReservationCommand>
    {
        public CancelReservationCommandHandler
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

        public async Task Handle(CancelReservationCommand request, CancellationToken cancellationToken)
        {
            var spec = new GetLocationByIdSpecification(request.LocationId);
            var location = await LocationRepository.GetLocationBySpecificationAsync(spec);

            if (location is null)
                throw new Exception("Location not found!");

            var role = CurrentUserService.GetCurrentUserType();

            if(role == "Owner")
            {
                location.CancelReservationOwner(request.ReservationId);
            }else if(role == "Customer")
            {
                location.CancelReservationCustomer(request.ReservationId);
            }

            await UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
