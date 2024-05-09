using BookWheel.Domain;
using BookWheel.Domain.RatingAggregate;
using BookWheel.Domain.Repositories;
using BookWheel.Infrastructure.Specification;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Infrastructure.Repositories
{
    public class RatingRepository : IRatingRepository
    {
        public ApplicationDbContext _dbContext { get; }
        private IConfiguration _configuration { get; }
        public IQueryable<RatingRoot> Queryable { get => _dbContext.Set<RatingRoot>(); }
        public RatingRepository
            (
            ApplicationDbContext dbContext,
            IConfiguration configuration
            )
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public async Task AddRatingAsync(RatingRoot rating)
        {
            await _dbContext.AddAsync(rating);
        }

        public async Task<RatingRoot?> GetRatingBySpecificationAsync(Specification<RatingRoot> spec)
        => await Queryable.ApplySpecification(spec).FirstOrDefaultAsync();

        public async Task<IList<RatingRoot?>> GetRatingsBySpecificationAsync(Specification<RatingRoot> spec)
        => await Queryable.ApplySpecification(spec).ToListAsync<RatingRoot?>();

        public async Task<int> UpsertRatingAsync
            (
            RatingRoot rating
            )
        {
            var cnn = new SqlConnection(_configuration.GetConnectionString("MSSQL"));
            
            await cnn.OpenAsync();
            var p = new { rating.UserId,rating.ReservationId,rating.StarCount,rating.Comment };
            
            using var transaction = await cnn.BeginTransactionAsync();
            
            var query = """
            DECLARE @check int;

            SELECT @check = COUNT(*) FROM Reservation WHERE Id = @ReservationId AND UserId = @UserId

            IF @check = 1
            BEGIN
            UPDATE dbo.Ratings WITH (UPDLOCK, SERIALIZABLE)
              SET StarCount = @StarCount,
              Ratings.Comment = @Comment
              WHERE ReservationId = @ReservationId;

            IF @@ROWCOUNT = 0
            BEGIN
              INSERT dbo.Ratings (UserId,ReservationId,Comment,StarCount)
              VALUES(@UserId,@ReservationId,@Comment,@StarCount);
            END
            END
            """;

            var i = await cnn.ExecuteAsync(query,p,transaction);
            await transaction.CommitAsync();
            await cnn.CloseAsync();

            return i;
        }
    }
}
