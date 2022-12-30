using BookingApp.Core.Contracts;
using BookingApp.Models.Bookings;
using BookingApp.Models.Bookings.Contracts;
using BookingApp.Models.Hotels;
using BookingApp.Models.Hotels.Contacts;
using BookingApp.Models.Rooms;
using BookingApp.Models.Rooms.Contracts;
using BookingApp.Repositories;
using BookingApp.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace BookingApp.Core
{
    public class Controller : IController
    {
        private IRepository<IHotel> hotels;

        public Controller()
        {
            hotels = new HotelRepository();
        }

        public string AddHotel(string hotelName, int category)
        {
            if (hotels.Select(hotelName) == default)
            {
                hotels.AddNew(new Hotel(hotelName, category));
                return String.Format(Utilities.Messages.OutputMessages.HotelSuccessfullyRegistered, category, hotelName);
            }
            else
                return String.Format(Utilities.Messages.OutputMessages.HotelAlreadyRegistered, category, hotelName);
        }

        public string BookAvailableRoom(int adults, int children, int duration, int category)
        {
            if (hotels.All().FirstOrDefault(x => x.Category == category) == default)
                return string.Format(Utilities.Messages.OutputMessages.CategoryInvalid, category);

            var orderedHotels = hotels.All().OrderBy(x => x.FullName);

            Dictionary<string, IRoom> selectedRooms = new Dictionary<string, IRoom>();
            
            foreach (var hotel in orderedHotels)
            {
                var selectedRoom = hotel.Rooms.All().Where(x => x.PricePerNight > 0).Where(z => z.BedCapacity >= adults + children).OrderBy(y => y.BedCapacity).FirstOrDefault();

                if (selectedRoom != default)
                    selectedRooms.Add(hotel.FullName, selectedRoom);
            }

            if (selectedRooms.Count > 0)
            {
                var selectedRoom = selectedRooms.OrderBy(x => x.Value.BedCapacity).First();
                int bookingNumber = hotels.All().Sum(x => x.Bookings.All().Count) + 1;
                var selectedRoomToIRoom = hotels.Select(selectedRoom.Key).Rooms.All().Where(x => x.PricePerNight == selectedRoom.Value.PricePerNight && x.BedCapacity == selectedRoom.Value.BedCapacity).First();
                hotels.All().Where(x => x.FullName == selectedRoom.Key).First().Bookings.AddNew(new Booking(selectedRoomToIRoom, duration, adults, children, bookingNumber));
                return string.Format(Utilities.Messages.OutputMessages.BookingSuccessful, bookingNumber, selectedRoom.Key);
            }

            return Utilities.Messages.OutputMessages.RoomNotAppropriate;
        }

        public string HotelReport(string hotelName)
        {
            var selectedHotel = hotels.Select(hotelName);

            if (selectedHotel == null)
                return string.Format(Utilities.Messages.OutputMessages.HotelNameInvalid, hotelName);
            
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Hotel name: {selectedHotel.FullName}");
            sb.AppendLine($"--{selectedHotel.Category} star hotel");
            sb.AppendLine($"--Turnover: {selectedHotel.Turnover:F2} $");
            sb.AppendLine("--Bookings:").AppendLine();

            if (selectedHotel.Bookings.All().Count == 0)
                sb.AppendLine("none");
            else
                foreach (var booking in selectedHotel.Bookings.All())
                    sb.AppendLine(booking.BookingSummary()).AppendLine();

            return sb.ToString().TrimEnd();
        }

        public string SetRoomPrices(string hotelName, string roomTypeName, double price)
        {
            if (hotels.Select(hotelName) == default)
                return string.Format(Utilities.Messages.OutputMessages.HotelNameInvalid, hotelName);

            if (roomTypeName != "Apartment" && roomTypeName != "DoubleBed" && roomTypeName != "Studio")
                throw new ArgumentException(Utilities.Messages.ExceptionMessages.RoomTypeIncorrect);

            if (hotels.Select(hotelName).Rooms.Select(roomTypeName) == null)
                return Utilities.Messages.OutputMessages.RoomTypeNotCreated;
            else
                if (hotels.Select(hotelName).Rooms.Select(roomTypeName).PricePerNight != 0)
                    throw new InvalidOperationException(Utilities.Messages.ExceptionMessages.CannotResetInitialPrice);
                else
                {
                    hotels.Select(hotelName).Rooms.Select(roomTypeName).SetPrice(price);                    
                    return string.Format(Utilities.Messages.OutputMessages.PriceSetSuccessfully, roomTypeName, hotelName);
                }
        }

        public string UploadRoomTypes(string hotelName, string roomTypeName)
        {
            if (roomTypeName != "Apartment" && roomTypeName != "DoubleBed" && roomTypeName != "Studio")
                throw new ArgumentException(Utilities.Messages.ExceptionMessages.RoomTypeIncorrect);

            if (hotels.Select(hotelName) == default)
                return string.Format(Utilities.Messages.OutputMessages.HotelNameInvalid, hotelName);
            else
            {
                if (hotels.Select(hotelName).Rooms.Select(roomTypeName) != default)
                    return Utilities.Messages.OutputMessages.RoomTypeAlreadyCreated;
                else
                {
                    switch (roomTypeName)
                    {
                        case "Apartment": hotels.Select(hotelName).Rooms.AddNew(new Apartment()); break;
                        case "Studio": hotels.Select(hotelName).Rooms.AddNew(new Studio()); break;
                        case "DoubleBed": hotels.Select(hotelName).Rooms.AddNew(new DoubleBed()); break;
                    }

                    return string.Format(Utilities.Messages.OutputMessages.RoomTypeAdded, roomTypeName, hotelName);
                }
            }
        }
    }
}