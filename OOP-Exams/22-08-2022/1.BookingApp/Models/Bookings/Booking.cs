using BookingApp.Models.Bookings.Contracts;
using BookingApp.Models.Rooms.Contracts;
using System;
using System.Text;

namespace BookingApp.Models.Bookings
{
    public class Booking : IBooking
    {
        private IRoom room;
        private int residenceDuration;
        private int adultsCount;
        private int childrenCount;
        private int bookingNumber;

        public int ResidenceDuration
        {
            get { return residenceDuration; }
            private set
            {
                if (value <= 0)
                    throw new ArgumentException(Utilities.Messages.ExceptionMessages.DurationZeroOrLess);

                residenceDuration = value;
            }
        }

        public int AdultsCount
        {
            get { return adultsCount; }
            private set
            {
                if (value < 1)
                    throw new ArgumentException(Utilities.Messages.ExceptionMessages.AdultsZeroOrLess);

                adultsCount = value;
            }
        }

        public int ChildrenCount
        {
            get { return childrenCount; }
            private set
            {
                if (value < 0)
                    throw new ArgumentException(Utilities.Messages.ExceptionMessages.ChildrenNegative);

                childrenCount = value;
            }
        }

        public int BookingNumber
        {
            get { return bookingNumber; }
            private set { bookingNumber = value; }
        }

        public IRoom Room
        {
            get { return room; }
            private set { room = value; }
        }

        public Booking(IRoom room, int residenceDuration, int adultsCount, int childrenCount, int bookingNumber)
        {
            Room = room;
            ResidenceDuration = residenceDuration;
            AdultsCount = adultsCount;
            ChildrenCount = childrenCount;
            BookingNumber = bookingNumber;
        }

        private double TotalPaid() => Math.Round(ResidenceDuration * Room.PricePerNight, 2);

        public string BookingSummary()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Booking number: {BookingNumber}");
            sb.AppendLine($"Room type: {Room.GetType().Name}");
            sb.AppendLine($"Adults: {AdultsCount} Children: {ChildrenCount}");
            sb.AppendLine($"Total amount paid: {TotalPaid():F2} $");
            return sb.ToString().Trim();
        }
    }
}