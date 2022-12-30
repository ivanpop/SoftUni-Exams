using PlanetWars.Models.MilitaryUnits.Contracts;
using System.Collections.Generic;

namespace PlanetWars.Repositories
{
    public class UnitRepository : IRepository
    {
        private List<IMilitaryUnit> models;

        public UnitRepository()
        {
            models = new List<IMilitaryUnit>();
        }

        public IReadOnlyCollection<IMilitaryUnit> Models => models;

        public IMilitaryUnit FindByName(string weaponTypeName)
        {
            IMilitaryUnit weapon = null;

            foreach (var gun in models)
            {
                if (gun.GetType().Name == weaponTypeName)
                    weapon = gun;
            }

            return weapon;
        }

        public void AddItem(IMilitaryUnit unit)
        {
            models.Add(unit);
        }

        public bool RemoveItem(string unitTypeName)
        {
            IMilitaryUnit weapon = FindByName(unitTypeName);
            return models.Remove(weapon);
        }

    }
}