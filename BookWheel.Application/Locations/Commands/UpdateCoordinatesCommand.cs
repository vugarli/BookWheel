using AutoMapper.Configuration.Annotations;
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

namespace BookWheel.Application.Locations.Commands
{
    public class UpdateCoordinatesCommand : IRequest
    {
        [Obsolete]
        [HybridBindProperty(Source.Route)]
        public Guid LocationId { get; set; }

        [HybridBindProperty(Source.Body)]
        public double Latitude { get; set; }
        [HybridBindProperty(Source.Body)]
        public double Longitude { get; set; }
    }


    public class UpdateCoordinatesCommandValidator : AbstractValidator<UpdateCoordinatesCommand>
    {
        public UpdateCoordinatesCommandValidator()
        {
            RuleFor(c=>c.LocationId)
                .NotEqual(Guid.Empty)
                .NotNull();
            // coordinates valid if:
            // -90 <= lat <= 90
            // -180 <= long <= 180
            RuleFor(c => c.Latitude).NotEmpty()
                .NotNull().Must(c => c >= -90 && c <= 90)
                .WithMessage("Latitude is invalid!");

            RuleFor(c => c.Longitude).NotEmpty()
                .NotNull().Must(c => c >= -180 && c <= 180)
                .WithMessage("Longitude is invalid!");
        }
    }

    public class UpdateCoordinatesCommandHandler : IRequestHandler<UpdateCoordinatesCommand>
    {
        public UpdateCoordinatesCommandHandler
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

        public async Task Handle(UpdateCoordinatesCommand request, CancellationToken cancellationToken)
        {
            var spec = new GetLocationByIdSpecification(request.LocationId);
            var location = await LocationRepository.GetLocationBySpecificationAsync(spec);

            if (location is null)
                throw new LocationNotFoundException(request.LocationId);

            location.UpdateCoordinates(request.Latitude,request.Longitude);

            await UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }



}
