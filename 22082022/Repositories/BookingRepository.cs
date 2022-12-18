using BookingApp.Models.Bookings.Contracts;
using BookingApp.Repositories.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace BookingApp.Repositories
{
    public class BookingRepository : IRepository<IBooking>
    {
        private List<IBooking> bookings;

        public void AddNew(IBooking booking)
        {
            bookings.Add(booking);
        }

        public IBooking Select(string bookingNumberToString) => bookings.FirstOrDefault(x => x.BookingNumber == int.Parse(bookingNumberToString));

        public IReadOnlyCollection<IBooking> All() => bookings;

        public BookingRepository()
        {
            bookings = new List<IBooking>();
        }
    }
}