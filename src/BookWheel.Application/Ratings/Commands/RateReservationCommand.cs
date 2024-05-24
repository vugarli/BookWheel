using BookWheel.Application.Exceptions;
using BookWheel.Application.Services;
using BookWheel.Domain;
using BookWheel.Domain.RatingAggregate;
using BookWheel.Domain.Repositories;
using FluentValidation;
using HybridModelBinding;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Application.Ratings.Commands
{
    public class RateReservationCommand : IRequest
    {
        [HybridBindProperty(Source.Route)]
        [Obsolete]
        public Guid ReservationId { get; set; }

        [HybridBindProperty(Source.Body)]
        public string Coment { get; set; }
        [HybridBindProperty(Source.Body)]
        public int StarCount { get; set; }
    }

    public class RateReservationCommandValidator
        : AbstractValidator<RateReservationCommand>
    {
        public RateReservationCommandValidator()
        {
            RuleFor(r => r.ReservationId)
                .NotEmpty()
                .NotEqual(Guid.Empty)
                .NotNull()
                .WithMessage("Please provide valid reservationId");

            RuleFor(r=>r.StarCount)
                .NotEmpty()
                .NotNull()
                .Must(sc=>sc<=5 && sc>0)
                .WithMessage("Please provide valid startcount");
            
            RuleFor(r=>r.Coment).NotEmpty().NotNull().WithMessage("Pleas provide valid coment!");
        }
    }

    public class RateReservationCommandHandler
        : IRequestHandler<RateReservationCommand>
    {
        public RateReservationCommandHandler
            (
                ICurrentUserService currentUserService,
                IRatingRepository ratingRepository,
                IUnitOfWork unitOfWork
            )
        {
            CurrentUserService = currentUserService;
            RatingRepository = ratingRepository;
            UnitOfWork = unitOfWork;
        }

        public ICurrentUserService CurrentUserService { get; }
        public IRatingRepository RatingRepository { get; }
        public IUnitOfWork UnitOfWork { get; }

        public async Task Handle(RateReservationCommand request, CancellationToken cancellationToken)
        {
            //todo check reservation status?
            // to rate reservation status should be Finished
            // check reservation belongs to the customer
            var customerId = Guid.Parse(CurrentUserService.GetCurrentUserId());

            var newRating = new RatingRoot(customerId,request.ReservationId,request.Coment,request.StarCount);
            
            var affected = await RatingRepository.UpsertRatingAsync(newRating);

            if (affected <= 0)
                throw new ReservationNotFoundException();
        }
    }
}
