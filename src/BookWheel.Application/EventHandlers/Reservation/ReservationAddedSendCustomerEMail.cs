using BookWheel.Application.Services;
using BookWheel.Domain.Events;
using BookWheel.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Application.EventHandlers.Reservation
{
    public class ReservationAddedSendCustomerEMail
        : INotificationHandler<ReservationAddedEvent>
    {

        public ReservationAddedSendCustomerEMail
            (
            IEmailSender emailSender,
            GetUserEmailAddressService getUserEmailAddressService
            )
        {
            EmailSender = emailSender;
            GetUserEmailAddressService = getUserEmailAddressService;
        }

        public IEmailSender EmailSender { get; }
        public GetUserEmailAddressService GetUserEmailAddressService { get; }

        public async Task Handle(ReservationAddedEvent notification, CancellationToken cancellationToken)
        {
            var email = await GetUserEmailAddressService.GetEmailAsync(notification.CustomerId);

            var body = $"Dear {email}, your reservation for {notification.LocationName} successfully processed. Reservation date:{notification.ReservationTimeRange.Start}-{notification.ReservationTimeRange.Start}.";

            await EmailSender.SendEmailAsync(email, "support@bookwheel.com", "Reservation processed", body);
        }
    }
}
