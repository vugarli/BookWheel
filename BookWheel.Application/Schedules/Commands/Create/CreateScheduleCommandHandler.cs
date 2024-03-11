using AutoMapper;
using BookWheel.Application.Specifications.Locations;
using BookWheel.Domain.LocationAggregate;
using BookWheel.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Application.Schedules.Commands.Create
{
    public class CreateScheduleCommandHandler
        : IRequestHandler<CreateScheduleCommand>
    {
        public ILocationRepository _locationRepository { get; }
        public IMapper _mapper { get; }

        public CreateScheduleCommandHandler
            (
            ILocationRepository locationRepository,
            IMapper mapper
            )
        {
            _locationRepository = locationRepository;
            _mapper = mapper;
        }


        public async Task Handle
            (
            CreateScheduleCommand request,
            CancellationToken cancellationToken
            )
        {
            var schedule = _mapper.Map<Schedule>(request);
            var spec = new GetLocationByIdSpecification(schedule.LocationId);

            var location = await _locationRepository.GetLocationBySpecificationAsync(spec);

            location.
        }
    }
}
