using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookWheel.Domain.Entities;

namespace BookWheel.Domain.AggregateRoots
{
    public class RatingRoot : BaseEntity
    {
        public RatingRoot
            (
            Guid userId,
            Guid reservationId,
            string? comment,
            int starCount
            )
        {
            UserId = userId;
            ReservationId = reservationId;
            Comment = comment;
            StarCount = starCount;
        }

        public Guid UserId { get; private set; }
        public Guid ReservationId { get; private set; }
        public Reservation? Reservation { get; private set; }

        public string? Comment { get; private set; }
        public int StarCount { get; private set; }
    }
}
