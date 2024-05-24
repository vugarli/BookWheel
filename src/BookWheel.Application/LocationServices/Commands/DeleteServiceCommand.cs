using BookWheel.Application.Services;
using BookWheel.Domain;
using BookWheel.Domain.Exceptions;
using BookWheel.Domain.Repositories;
using BookWheel.Domain.Specifications.Location;
using FluentValidation;
using HybridModelBinding;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Application.LocationServices.Commands
{
    public class DeleteServiceCommand : IRequest
    {
        [HybridBindProperty(Source.Route, order: 10)]
        [Obsolete]
        public Guid ServiceId { get; set; }

        [HybridBindProperty(Source.Route, order: 10)]
        [Obsolete]
        public Guid LocationId { get; set; }
    }

    public class DeleteServiceCommandValidator
   : AbstractValidator<DeleteServiceCommand>
    {
        public DeleteServiceCommandValidator()
        {
            RuleFor(c => c.ServiceId).NotEmpty().NotNull().WithMessage("ServiceId must be provided!");
            RuleFor(c => c.LocationId).NotEmpty().NotNull().WithMessage("LocationId must be provided!");

            RuleFor(c => c.LocationId).NotEqual(Guid.Empty).WithMessage("LocationId is invalid!");
            RuleFor(c => c.ServiceId).NotEqual(Guid.Empty).WithMessage("ServiceId is invalid!");   
        }
    }


    public class DeleteServiceCommandHandler : IRequestHandler<DeleteServiceCommand>
    {
        public IUnitOfWork UnitOfWork { get; }
        public ILocationRepository LocationRepository { get; }
        public ICurrentUserService CurrentUserService { get; }

        public DeleteServiceCommandHandler
            (
            ILocationRepository locationRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork
            )
        {
            LocationRepository = locationRepository;
            CurrentUserService = currentUserService;
            UnitOfWork = unitOfWork;
        }


        public async Task Handle(DeleteServiceCommand request, CancellationToken cancellationToken)
        {
            var ownerId = Guid.Parse(CurrentUserService.GetCurrentUserId());

            var spec = new GetOwnerLocationByIdSpecification(request.LocationId, ownerId);
            var locaiton = await LocationRepository.GetLocationBySpecificationAsync(spec);

            if (locaiton is null)
                throw new LocationNotFoundException(request.LocationId);

            locaiton.RemoveService(request.ServiceId);

            await UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
