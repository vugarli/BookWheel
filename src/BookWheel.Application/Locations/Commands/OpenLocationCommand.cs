﻿using BookWheel.Domain.Exceptions;
using BookWheel.Domain.Repositories;
using BookWheel.Domain.Specifications.Location;
using BookWheel.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using BookWheel.Application.Services;

namespace BookWheel.Application.Locations.Commands
{
    public class OpenLocationCommand : IRequest
    {
        [FromRoute]
        public Guid LocationId { get; set; }
    }

    public class OpenLocationCommandValidator : AbstractValidator<OpenLocationCommand>
    {
        public OpenLocationCommandValidator()
        {
            RuleFor(l => l.LocationId).NotEmpty().NotEqual(Guid.Empty).NotNull();
        }
    }

    public class OpenLocationCommandHandler : IRequestHandler<OpenLocationCommand>
    {
        public OpenLocationCommandHandler
            (
                ILocationRepository locationRepository,
                ICurrentUserService currentUserService,
                IUnitOfWork unitOfWork
            )
        {
            LocationRepository = locationRepository;
            CurrentUserService = currentUserService;
            UnitOfWork = unitOfWork;
        }

        public ILocationRepository LocationRepository { get; }
        public ICurrentUserService CurrentUserService { get; }
        public IUnitOfWork UnitOfWork { get; }

        public async Task Handle(OpenLocationCommand request, CancellationToken cancellationToken)
        {
            var ownerId = Guid.Parse(CurrentUserService.GetCurrentUserId());

            var spec = new GetOwnerLocationByIdSpecification(request.LocationId, ownerId);
            var location = await LocationRepository.GetLocationBySpecificationAsync(spec);

            if (location is null)
                throw new LocationNotFoundException(request.LocationId);

            location.OpenLocation();

            await UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
