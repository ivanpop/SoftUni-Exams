using PlanetWars.Models.Weapons.Contracts;
using System.Collections.Generic;

namespace PlanetWars.Repositories
{
    public class WeaponRepository : IRepository
    {
        private List<IWeapon> models;

        public WeaponRepository()
        {
            models = new List<IWeapon>();
        }


		public IReadOnlyCollection<IWeapon> Models => models;

        public IWeapon FindByName(string weaponTypeName)
        {
            IWeapon weapon = null;

            foreach (var gun in models) 
            {
                if (gun.GetType().Name == weaponTypeName)
                    weapon = gun;
            }

            return weapon;
        }

        public void AddItem(IWeapon weapon) 
        {
            models.Add(weapon);
        }

        public bool RemoveItem(string weaponTypeName)
        {
            IWeapon weapon = FindByName(weaponTypeName);
            return models.Remove(weapon);
        }
    }
}