using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using BookWheel.Domain.Entities;

namespace BookWheel.Domain.RatingAggregate
{
    public class RatingRoot : BaseEntity<int>
    {
        private RatingRoot()
        {
            
        }
        public RatingRoot
            (
            Guid userId,
            Guid reservationId,
            string? comment,
            int starCount
            )
        {
            UserId = Guard.Against.Default(userId);
            ReservationId = Guard.Against.Default(reservationId);
            Comment = Guard.Against.NullOrEmpty(comment);
            StarCount = Guard.Against.OutOfRange(starCount,nameof(starCount),1,5);
        }

        public Guid UserId { get; private set; }
        public Guid ReservationId { get; private set; }

        public string? Comment { get; private set; }
        public int StarCount { get; private set; }
    }
}
