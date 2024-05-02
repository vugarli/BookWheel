using BookWheel.Domain.RatingAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.UnitTests.Builders
{
    public class RatingBuilder
    {
        private Guid Id { get; set; } = Guid.NewGuid();
        private Guid UserId { get; set; } = Guid.NewGuid();
        private Guid LocationId { get; set; }
        private Guid ReservationId { get; set; }
        private string Comment { get; set; } = "No comment";
        private int Stars { get; set; } = 4;


        public RatingBuilder(Guid locationId,Guid reservationId)
        {
            LocationId = locationId;
            ReservationId = reservationId;
        }

        public RatingBuilder WithComment(string comment)
        {
            Comment = comment;
            return this;
        }

        public RatingBuilder WithStars(int stars)
        {
            Stars = stars;
            return this;
        }
        public RatingBuilder WithId(Guid id)
        {
            Id = id;
            return this;
        }


        public RatingRoot Build()
        {
            var rating =  new RatingRoot(UserId,ReservationId,LocationId,Comment,Stars);
            return rating;
        }

    }
}
