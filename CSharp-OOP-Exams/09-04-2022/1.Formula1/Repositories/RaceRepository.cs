using Formula1.Models.Contracts;
using Formula1.Repositories.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace Formula1.Repositories
{
    public class RaceRepository : IRepository<IRace>
    {
        private List<IRace> raceList;

        public RaceRepository()
        {
            raceList = new List<IRace>();
        }

        public IReadOnlyCollection<IRace> Models => raceList;
        public void Add(IRace model)
        {
            raceList.Add(model);
        }
        public IRace FindByName(string name) => raceList.FirstOrDefault(x => x.RaceName == name);
        public bool Remove(IRace model) => raceList.Remove(model);
    }
}