namespace BookingApp.Models.Rooms
{
    public class Apartment : Room
    {
        private static int bedCapacity;

        public Apartment() : base(bedCapacity = 6)
        {
        }
    }
}