using BookingApp.Models.Rooms.Contracts;
using BookingApp.Repositories.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace BookingApp.Repositories
{
    public class RoomRepository : IRepository<IRoom>
    {
        private List<IRoom> rooms;

        public void AddNew (IRoom room)
        {
            rooms.Add(room);
        }

        public IRoom Select(string roomTypeName) => rooms.FirstOrDefault(x => x.GetType().Name == roomTypeName);

        public IReadOnlyCollection<IRoom> All() => rooms;

        public RoomRepository()
        {
            rooms= new List<IRoom>();
        }
    }
}