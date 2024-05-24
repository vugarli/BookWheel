using BookWheel.Domain.RatingAggregate;
using BookWheel.UnitTests.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.UnitTests.Domain.RatingAggregate
{
    public class RatingCreate
    {

        [Theory]
        [InlineData(0)]
        [InlineData(10)]
        [InlineData(7)]
        public void ThrowsExceptionWhenProvidedInvalidStartCount(int starcount)
        {
            void Action()
            {
                var rating = new RatingBuilder(Guid.NewGuid()).WithStars(starcount).Build();
            }

            Assert.Throws<ArgumentOutOfRangeException>(Action);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(5)]
        public void DoesNOTThrowExceptionWhenProvidedValidStartCount(int starcount)
        {
            void Action()
            {
                var rating = new RatingBuilder(Guid.NewGuid()).WithStars(starcount).Build();
            }

            var exception = Record.Exception(Action);
            Assert.Null(exception);
        }

    }
}
