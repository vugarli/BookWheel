using BookWheel.Application.Locations.Dtos;
using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Application.Locations.Queries
{
    public record GetLocationRatingsQuery(Guid LocationId) :IRequest<IList<RatingDto>> { }


    public class GetLocationRatingsQueryHandler :
        IRequestHandler<GetLocationRatingsQuery, IList<RatingDto>>
    {
        private IConfiguration _configuration { get; }
        public GetLocationRatingsQueryHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public async Task<IList<RatingDto>> Handle
            (
            GetLocationRatingsQuery request,
            CancellationToken cancellationToken
            )
        {
            using var tran = new SqlConnection(_configuration.GetConnectionString("MSSQL"));

            var p = new { locationId = request.LocationId };

            var query = """ SELECT * FROM dbo.Ratings WHERE LocationId = @locationId """;

            var ratingDtos = await tran.QueryAsync<RatingDto>(query,p);

            return ratingDtos.ToList();
        }
    }
}
