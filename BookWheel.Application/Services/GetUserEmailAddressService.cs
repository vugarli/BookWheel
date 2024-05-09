using Ardalis.GuardClauses;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Application.Services
{
    public class GetUserEmailAddressService
    {
        public GetUserEmailAddressService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public async Task<string> GetEmailAsync(Guid userId)
        {
            var cnn = new SqlConnection(Configuration.GetConnectionString("MSSQL"));

            var p = new { userId };

            var query = """ SELECT Email FROM ApplicationUserRoot WHERE Id = @userId; """;

            var email = await cnn.QueryFirstOrDefaultAsync<string>(query, p);

            Guard.Against.NullOrEmpty(email);

            return email;
        }

    }
}
