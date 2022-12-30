namespace BookingApp.Models.Rooms
{
    public class Studio : Room
    {
        private static int bedCapacity;

        public Studio() : base(bedCapacity = 4)
        {
        }
    }
}