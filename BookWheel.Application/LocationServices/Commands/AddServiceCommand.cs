using BookWheel.Domain;
using BookWheel.Domain.LocationAggregate;
using BookWheel.Domain.Repositories;
using BookWheel.Domain.Specifications.Location;
using HybridModelBinding;
using MediatR;
using NetTopologySuite.LinearReferencing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Application.LocationServices.Commands
{
    public class AddServiceCommand : IRequest
    {
        [HybridBindProperty(Source.Body, order: 10)]
        public string Name { get; set; }
        [HybridBindProperty(Source.Body, order: 10)]
        public string Description { get; set; }
        [HybridBindProperty(Source.Body, order: 10)]
        public decimal Price { get; set; }
        [HybridBindProperty(Source.Body, order: 10)]
        public int MinuteDuration { get; set; }

        [HybridBindProperty(Source.Route, order: 10)]
        [Obsolete]
        public Guid LocationId { get; set; }
    }


    public class AddServiceCommandHandler : IRequestHandler<AddServiceCommand>
    {
        public ILocationRepository LocationRepository { get; }
        public IUnitOfWork UnitOfWork { get; }
        public AddServiceCommandHandler(ILocationRepository locationRepository, IUnitOfWork unitOfWork)
        {
            LocationRepository = locationRepository;
            UnitOfWork = unitOfWork;
        }

        public async Task Handle(AddServiceCommand request, CancellationToken cancellationToken)
        {
            var spec = new GetLocationByIdSpecification(request.LocationId);
            var location = await LocationRepository.GetLocationBySpecificationAsync(spec);

            if (location is null)
                throw new Exception("Location not found!");

            // check for owner id, current user service

            var newService = new Service
                (
                Guid.NewGuid(),
                request.Name,
                request.Description,
                request.Price,
                request.MinuteDuration,
                request.LocationId
                );

            location.AddService(newService);

            await UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }


}
