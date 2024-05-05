using BookWheel.Domain;
using BookWheel.Domain.Repositories;
using BookWheel.Domain.Specifications.Location;
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


    public class DeleteServiceCommandHandler : IRequestHandler<DeleteServiceCommand>
    {
        public IUnitOfWork UnitOfWork { get; }
        public ILocationRepository LocationRepository { get; }
        public DeleteServiceCommandHandler
            (
            ILocationRepository locationRepository,
            IUnitOfWork unitOfWork
            )
        {
            LocationRepository = locationRepository;
            UnitOfWork = unitOfWork;
        }


        public async Task Handle(DeleteServiceCommand request, CancellationToken cancellationToken)
        {
            var spec = new GetLocationByIdSpecification(request.LocationId);
            var locaiton = await LocationRepository.GetLocationBySpecificationAsync(spec);

            if (locaiton is null)
                throw new Exception("Location not found");

            locaiton.RemoveService(request.ServiceId);

            await UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
