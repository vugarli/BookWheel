using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Application.Schedules.Commands.Create
{
    public class CreateScheduleCommandValidator : AbstractValidator<CreateScheduleCommand>
    {
        public CreateScheduleCommandValidator()
        {
            RuleFor(c=>c.Amount).Must(a=> a>0).WithMessage("Amount should be positive!");
            RuleFor(c=>c.LocationId).NotEmpty().NotEqual(Guid.Empty).WithMessage("Invalid Id");
            RuleFor(c=>c.ScheduleDateStart).NotNull().WithMessage("Date must be specified");
            RuleFor(c=>c.ScheduleDateEnd).NotNull().WithMessage("Date must be specified");
        }
    }
}
