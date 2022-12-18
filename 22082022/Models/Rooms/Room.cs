using BookingApp.Models.Rooms.Contracts;
using System;

namespace BookingApp
{
    public abstract class Room : IRoom
    {
        private int bedCapacity;
        private double pricePerNight = 0;

        public int BedCapacity
        { 
            get { return bedCapacity; } 
            private set { bedCapacity = value; }
        }

        public double PricePerNight
        {
            get { return pricePerNight; } 
            private set 
            {
                if (value < 0)
                    throw new ArgumentException(Utilities.Messages.ExceptionMessages.PricePerNightNegative);

                pricePerNight = value; 
            }
        }

        public void SetPrice(double price)
        {
            PricePerNight = price;
        }

        public Room(int bedCapacity)
        {
            BedCapacity = bedCapacity;
        }
    }
}