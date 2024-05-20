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
                IUnitOfWork unitOfWork
            )
        {
            LocationRepository = locationRepository;
            UnitOfWork = unitOfWork;
        }

        public ILocationRepository LocationRepository { get; }
        public IUnitOfWork UnitOfWork { get; }

        public async Task Handle(CloseLocationCommand request, CancellationToken cancellationToken)
        {
            var spec = new GetLocationByIdSpecification(request.LocationId);
            var location = await LocationRepository.GetLocationBySpecificationAsync(spec);

            if (location is null)
                throw new LocationNotFoundException(request.LocationId);

            location.CloseLocation();

            await UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
