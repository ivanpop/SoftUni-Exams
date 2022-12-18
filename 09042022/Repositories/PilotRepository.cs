using Formula1.Models.Contracts;
using Formula1.Repositories.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace Formula1.Repositories
{
    public class PilotRepository : IRepository<IPilot>
    {
        private List<IPilot> pilots;

        public PilotRepository()
        {
            pilots = new List<IPilot>();
        }

        public IReadOnlyCollection<IPilot> Models => pilots;
        public void Add(IPilot model)
        {
            pilots.Add(model);
        }

        public IPilot FindByName(string name) => pilots.FirstOrDefault(x => x.FullName == name);
        public bool Remove(IPilot model) => pilots.Remove(model);
    }
}