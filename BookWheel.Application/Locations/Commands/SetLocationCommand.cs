using BookWheel.Application.LocationServices.Commands;
using BookWheel.Application.Services;
using BookWheel.Domain.LocationAggregate;
using BookWheel.Domain.Services;
using BookWheel.Domain.Value_Objects;
using BookWheel.UnitTests.Builders;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Application.Locations.Commands
{
    public class SetLocationCommand 
        : IRequest
    {
        public TimeOnly Start { get; set; }
        public TimeOnly End { get; set; }
        public string Name { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
    }

    public class SetLocationCommandValidator
        : AbstractValidator<SetLocationCommand>
    {
        public SetLocationCommandValidator()
        {
            RuleFor(c => c.Name).NotEmpty().NotNull().WithMessage("Name must be provided!");
            // coordinates valid if:
            // -90 <= lat <= 90
            // -180 <= long <= 180
            RuleFor(c => c.Lat).NotEmpty().NotNull().Must(c=> c>=-90 && c<= 90).WithMessage("Latitude is invalid!");
            RuleFor(c => c.Long).NotEmpty().NotNull().Must(c=> c>=-180 && c<= 180).WithMessage("Longitude is invalid!");
        }
    }



    public class SetLocationCommandHandler
       : IRequestHandler<SetLocationCommand>
    {
        private OwnerLocationSetter _ownerLocationSetter { get; }
        private ICurrentUserService _currentUserService { get; }

        public SetLocationCommandHandler
            (
            OwnerLocationSetter ownerLocationSetter,
            ICurrentUserService currentUserService
            )
        {
            _ownerLocationSetter = ownerLocationSetter;
            _currentUserService = currentUserService;
        }


        public async Task Handle
            (
            SetLocationCommand request,
            CancellationToken cancellationToken
            )
        {
            var workingHours = new TimeOnlyRange(request.Start,request.End);
            
            var ownerId = Guid.Parse(_currentUserService.GetCurrentUserId());

            var locationBuilder = new LocationBuilder(ownerId);
            locationBuilder.WithWorkingTimeRange(workingHours);
            locationBuilder.WithLatLong(request.Lat,request.Long);
            locationBuilder.WithName(request.Name);
            
            var location = locationBuilder.Build();

            await _ownerLocationSetter.SetLocationToOwnerAsync(ownerId,location,cancellationToken);
        }
    }

}
