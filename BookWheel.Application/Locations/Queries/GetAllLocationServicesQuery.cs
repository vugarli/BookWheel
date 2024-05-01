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
    public record GetAllLocationServicesQuery(Guid LocationId) : IRequest<IList<ServiceDto>> {}


    public class GetAllLocationServicesQueryHandler
        : IRequestHandler<GetAllLocationServicesQuery, IList<ServiceDto>>
    {
        private IConfiguration _configuration { get; }
        public GetAllLocationServicesQueryHandler
            (IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public async Task<IList<ServiceDto>> Handle(GetAllLocationServicesQuery request, CancellationToken cancellationToken)
        {
            using var cnn = new SqlConnection(_configuration.GetConnectionString("MSSQL"));
            var p = new { request.LocationId };

            var query = """
                SELECT *
                FROM dbo.Service serv
                WHERE serv.LocationId = @LocationId;
                """;

            var serviceDtos = await cnn.QueryAsync<ServiceDto>(query,p);
            return serviceDtos.ToList();
        }
    }
}
