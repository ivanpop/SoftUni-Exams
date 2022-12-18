namespace BookingApp.Models.Rooms
{
    public class DoubleBed : Room
    {
        private static int bedCapacity;

        public DoubleBed() : base(bedCapacity = 2)
        {
        }
    }
}