using BookWheel.Domain.LocationAggregate;
using BookWheel.Domain.Value_Objects;
using BookWheel.UnitTests.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.UnitTests.Domain.LocationAggregate.LocationReservationReadModel
{
    public class LocationReservationReadModelTests
    {

        [Fact]
        public void GetsValidAvailableTimeSlots()
        {
            var reservation1 = new ReservationBuilder(Guid.NewGuid(), Guid.NewGuid(), new(), new TimeRange("1/12/2023 09:00", "1/12/2023 10:30")).Build();

            var reservation2 = new ReservationBuilder(Guid.NewGuid(), Guid.NewGuid(), new(), new TimeRange("1/12/2023 09:00", "1/12/2023 10:20")).Build();


            var locationReadModel = new LocationReservationsReadModel();
            locationReadModel.WorkingHours = new TimeOnlyRange("09:00","18:00");
            locationReadModel.ActiveReservations.Add(reservation1);
            locationReadModel.ActiveReservations.Add(reservation2);

            var timeslots = locationReadModel.GetAvailableTimeSlots();


            Assert.Equal(1, timeslots.Count);
        }


    }
}
