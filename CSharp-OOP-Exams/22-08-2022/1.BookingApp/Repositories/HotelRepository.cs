using BookingApp.Models.Hotels.Contacts;
using BookingApp.Repositories.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace BookingApp.Repositories
{
    public class HotelRepository : IRepository<IHotel>
    {
        private List<IHotel> hotels;

        public void AddNew(IHotel hotel)
        {
            hotels.Add(hotel);
        }

        public IHotel Select(string hotelName) => hotels.FirstOrDefault(x => x.FullName == hotelName);

        public IReadOnlyCollection<IHotel> All() => hotels;

        public HotelRepository()
        {
            hotels = new List<IHotel>();
        }
    }
}