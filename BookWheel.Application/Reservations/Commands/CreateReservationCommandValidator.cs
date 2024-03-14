using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Application.Reservations.Commands
{
    public class CreateReservationCommandValidator
        : AbstractValidator<CreateReservationCommand>
    {
        public CreateReservationCommandValidator()
        {
            RuleFor(c=>c.ScheduleId).NotNull().WithMessage("ScheduleId must be provided!");
            RuleFor(c=>c.LocationId).NotNull().WithMessage("LocationId must be provided!");
        }
    }
}
