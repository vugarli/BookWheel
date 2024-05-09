using BookWheel.Application.Services;
using BookWheel.Domain.Events;
using BookWheel.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Application.EventHandlers.Reservation
{

    public class ReservationCancelledCustomerSendEmail
        : INotificationHandler<ReservationCancelledCustomerEvent>
    {
        public ReservationCancelledCustomerSendEmail
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

        public async Task Handle(ReservationCancelledCustomerEvent notification, CancellationToken cancellationToken)
        {
            var email = await GetUserEmailAddressService.GetEmailAsync(notification.CustomerId);

            var body = $"Dear {email}, you cancelled your reservation for {notification.LocationName}. Reservation date was:{notification.ReservationTimeRange.Start}-{notification.ReservationTimeRange.Start}.";

            await EmailSender.SendEmailAsync(email, "support@bookwheel.com", "Reservation cancellation successful.", body);
        }
    }
}
