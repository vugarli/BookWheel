using FluentValidation;
using MediatR;

namespace BookWheel.Application.Reservations.Commands.Create
{
    public class CreateReservationCommand : IRequest
    {
        public Guid LocationId { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public List<Guid> ServiceIds { get; set; } = new(); // guard against empty services in domain
    }
    public class CreateReservationCommandValidator
        : AbstractValidator<CreateReservationCommand>
    {
        public CreateReservationCommandValidator()
        {
            RuleFor(c=>c.LocationId).NotNull().WithMessage("LocationId must be provided!");
            RuleFor(c=>c.StartDate).NotNull().WithMessage("StartDate must be provided!");
            RuleFor(c=>c.ServiceIds).NotEmpty().WithMessage("ServiceIds must be provided!");
            //TODO service ids duplicate rule
        }
    }
}
