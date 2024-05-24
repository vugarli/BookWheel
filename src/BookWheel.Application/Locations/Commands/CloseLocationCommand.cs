using BookWheel.Application.Services;
using BookWheel.Domain;
using BookWheel.Domain.Exceptions;
using BookWheel.Domain.Repositories;
using BookWheel.Domain.Specifications.Location;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Application.Locations.Commands
{
    public class CloseLocationCommand : IRequest
    {
        [FromRoute]
        public Guid LocationId { get; set; }
    }

    public class  CloseLocationCommandValidator : AbstractValidator<CloseLocationCommand>
    {
        public CloseLocationCommandValidator()
        {
            RuleFor(l=>l.LocationId).NotEmpty().NotEqual(Guid.Empty).NotNull();
        }
    }
    
    public class CloseLocationCommandHandler : IRequestHandler<CloseLocationCommand>
    {
        public CloseLocationCommandHandler
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

        public async Task Handle(CloseLocationCommand request, CancellationToken cancellationToken)
        {
            var ownerId = Guid.Parse(CurrentUserService.GetCurrentUserId());

            var spec = new GetOwnerLocationByIdSpecification(request.LocationId,ownerId);
            var location = await LocationRepository.GetLocationBySpecificationAsync(spec);

            if (location is null)
                throw new LocationNotFoundException(request.LocationId);

            location.CloseLocation();

            await UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
