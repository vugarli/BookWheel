using Dapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Application.Locations.Queries
{
    public class SearchLocationByNameQuery : IRequest<IList<LocationDto>>
    {
        public string SearchTerm { get; set; }
    }


    public class SearchLocationByNameQueryValidator: AbstractValidator<SearchLocationByNameQuery>
    {
        public SearchLocationByNameQueryValidator()
        {
            RuleFor(q=>q.SearchTerm).NotEmpty().NotNull().WithMessage("Please provide valid searchterm!");   
        }
    }


    public class SearchLocationByNameQueryHandler : IRequestHandler<SearchLocationByNameQuery, IList<LocationDto>>
    {

        public SearchLocationByNameQueryHandler
            (
            IConfiguration configuration
            )
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public async Task<IList<LocationDto>> Handle(SearchLocationByNameQuery request, CancellationToken cancellationToken)
        {
            using var cnn = new SqlConnection(Configuration.GetConnectionString("MSSQL"));

            var p = new {searchterm = request.SearchTerm };

            var query = """
                 SELECT loc.Id,
                loc.Name as LocationName,
                OwnerId,
                BoxCount,
                userr.Name as OwnerName,
                Coordinates.Lat as Lat,
                Coordinates.Long as Long,
                    (Select AVG(CAST(rate.StarCount AS float))
                from Ratings rate
                left join Reservation res
                on rate.ReservationId = res.Id WHERE res.LocationId = loc.Id) AS Rating
                FROM dbo.Location loc
                left join dbo.ApplicationUserRoot userr
                on loc.OwnerId = userr.Id
                WHERE loc.Name LIKE '%'+@searchterm+'%'; 
                """;

            var dtos = await cnn.QueryAsync<LocationDto>(query,p);

            return dtos.ToList();
        }
    }




}
