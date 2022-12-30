using PlanetWars.Models.Planets.Contracts;
using System.Collections.Generic;

namespace PlanetWars.Repositories
{
    public class PlanetRepository : IRepository
    {
        private List<IPlanet> models;

        public PlanetRepository()
        {
            models = new List<IPlanet>();
        }


        public IReadOnlyCollection<IPlanet> Models => models;

        public IPlanet FindByName(string name)
        {
            IPlanet findPlanet = null;

            foreach (var planet in models)
            {
                if (planet.Name == name)
                {
                    findPlanet = planet;
                    break;
                }
            }

            return findPlanet;
        }

        public void AddItem(IPlanet planet)
        {
            models.Add(planet);
        }

        public bool RemoveItem(string planetName)
        {
            IPlanet planet = FindByName(planetName);
            return models.Remove(planet);
        }
    }
}