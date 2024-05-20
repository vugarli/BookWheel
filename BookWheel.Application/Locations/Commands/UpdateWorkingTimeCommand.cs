using BookWheel.Domain;
using BookWheel.Domain.Exceptions;
using BookWheel.Domain.Repositories;
using BookWheel.Domain.Specifications.Location;
using BookWheel.Domain.Value_Objects;
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
    public class UpdateWorkingTimeCommand : IRequest
    {
        [Obsolete]
        [HybridBindProperty(Source.Route)]
        public Guid LocationId { get; set; }

        [HybridBindProperty(Source.Body)]
        public TimeOnly Start { get; set; }

        [HybridBindProperty(Source.Body)]
        public TimeOnly End { get; set; }
    }

    public class  UpdateWorkingTimeCommandValidator 
        : AbstractValidator<UpdateWorkingTimeCommand>
    {
        public UpdateWorkingTimeCommandValidator()
        {
            RuleFor(c => c.Start).NotNull().NotEmpty()
                .WithMessage("Start time required!");

            RuleFor(c => c.End).NotNull().NotEmpty()
                .WithMessage("End time required!");

            RuleFor(c=>c.Start)
                .Must((command,start)=> start < command.End)
                .WithMessage("Start and End times are invalid");
        }
    }

    public class UpdateWorkingTimeCommandHandler
        : IRequestHandler<UpdateWorkingTimeCommand>
    {
        public UpdateWorkingTimeCommandHandler
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

        public async Task Handle(UpdateWorkingTimeCommand request, CancellationToken cancellationToken)
        {
            var spec = new GetLocationByIdSpecification(request.LocationId);

            var location = await LocationRepository.GetLocationBySpecificationAsync(spec);

            if (location is null)
                throw new LocationNotFoundException(request.LocationId);

            var workingTimeRange = new TimeOnlyRange(request.Start,request.End);

            location.UpdateWorkingTime(workingTimeRange);

            await UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }


}
