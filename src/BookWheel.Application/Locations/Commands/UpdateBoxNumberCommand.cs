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
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Application.Locations.Commands
{
    public class UpdateBoxNumberCommand : IRequest
    {
        [Obsolete]
        [HybridBindProperty(Source.Route)]
        public Guid LocationId { get; set; }

        [HybridBindProperty(Source.Body)]
        public int BoxNumber { get; set; }
    }

    public class UpdateBoxNumberCommandValidator 
        : AbstractValidator<UpdateBoxNumberCommand>
    {
        public UpdateBoxNumberCommandValidator()
        {
            RuleFor(c=>c.LocationId).NotEmpty().NotNull().NotEqual(Guid.Empty);
            RuleFor(c=>c.BoxNumber).NotEmpty().NotNull().Must(a=>a>0).WithMessage("Invalid box number!");
        }
    }

    public class UpdateBoxNumberCommandHandler 
        : IRequestHandler<UpdateBoxNumberCommand>
    {

        public UpdateBoxNumberCommandHandler
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

        public ILocationRepository LocationRepository { get; }
        public ICurrentUserService CurrentUserService { get; }
        public IUnitOfWork UnitOfWork { get; }

        public async Task Handle(UpdateBoxNumberCommand request, CancellationToken cancellationToken)
        {
            var ownerId = Guid.Parse(CurrentUserService.GetCurrentUserId());

            var spec = new GetOwnerLocationByIdSpecification(request.LocationId, ownerId);
            var location = await LocationRepository.GetLocationBySpecificationAsync(spec);

            if (location is null)
                throw new LocationNotFoundException(request.LocationId);

            location.UpdateBoxCount(request.BoxNumber);

            await UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }


}
