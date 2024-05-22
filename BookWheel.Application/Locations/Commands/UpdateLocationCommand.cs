using BookWheel.Domain.Exceptions;
using Dapper;
using FluentValidation;
using HybridModelBinding;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Application.Locations.Commands
{
    public class UpdateLocationCommand : IRequest
    {
        [Obsolete]
        [HybridBindProperty(Source.Route)]
        public Guid LocationId { get; set; }

        [HybridBindProperty(Source.Body)]
        public string LocationName { get; set; }
    }


    public class UpdateLocationCommandValidator : AbstractValidator<UpdateLocationCommand>
    {
        public UpdateLocationCommandValidator()
        {
            RuleFor(c => c.LocationId).NotNull().NotEmpty().NotEqual(Guid.Empty).WithMessage("Invalid location Id");

            RuleFor(c=>c.LocationName).NotNull().NotEmpty().WithMessage("Please provide location name.");
            RuleFor(c=>c.LocationName).MaximumLength(50).WithMessage("Location name length should be less than 50.");
            RuleFor(c=>c.LocationName).MinimumLength(4).WithMessage("Location name length should be more than 4.");
        }
    }

    public class UpdateLocationCommandHandler 
        : IRequestHandler<UpdateLocationCommand>
    {
        public UpdateLocationCommandHandler
            (
            IConfiguration configuration
            )
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public async Task Handle(UpdateLocationCommand request, CancellationToken cancellationToken)
        {
            using var cnn = new SqlConnection(Configuration.GetConnectionString("MSSQL"));

            var p = new { request.LocationId,request.LocationName };

            var command = """ UPDATE Location SET Name = @LocationName WHERE Id = @LocationId  """;

            var rowsaffected = await cnn.ExecuteAsync(command,p);

            if (rowsaffected is 0)
                throw new LocationNotFoundException(request.LocationId);
        }
    }


}
