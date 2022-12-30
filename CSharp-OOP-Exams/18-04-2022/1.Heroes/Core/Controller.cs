using Heroes.Core.Contracts;
using Heroes.Models.Contracts;
using Heroes.Models.Heroes;
using Heroes.Models.Map;
using Heroes.Models.Weapons;
using Heroes.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Heroes.Core
{
    public class Controller : IController
    {
        private HeroRepository heroes;
        private WeaponRepository weapons;

        public Controller()
        {
            heroes = new HeroRepository();
            weapons = new WeaponRepository();
        }

        public string AddWeaponToHero(string weaponName, string heroName)
        {
            var selectedHero = heroes.FindByName(heroName);
            var selectedWeapon = weapons.FindByName(weaponName);

            if (selectedHero == null)
                throw new InvalidOperationException($"Hero {heroName} does not exist.");

            if (selectedWeapon == null)
                throw new InvalidOperationException($"Weapon {weaponName} does not exist.");

            if (selectedHero.Weapon != null)
                throw new InvalidOperationException($"Hero {heroName} is well-armed.");

            selectedHero.AddWeapon(selectedWeapon);
            weapons.Remove(selectedWeapon);
            return $"Hero {heroName} can participate in battle using a {selectedWeapon.GetType().Name.ToLower()}.";
        }

        public string CreateHero(string type, string name, int health, int armour)
        {
            if (heroes.FindByName(name) != null)
                throw new InvalidOperationException($"The hero {name} already exists.");

            if (type != "Knight" && type != "Barbarian")
                throw new InvalidOperationException("Invalid hero type.");
            else if (type == "Knight")
            {
                heroes.Add(new Knight(name, health, armour));
                return $"Successfully added Sir {name} to the collection.";
            }
            else
            {
                heroes.Add(new Barbarian(name, health, armour));
                return $"Successfully added Barbarian { name } to the collection.";
            }
        }

        public string CreateWeapon(string type, string name, int durability)
        {
            if (weapons.FindByName(name) != null)
                throw new InvalidOperationException($"The weapon {name} already exists.");

            if (type != "Mace" && type != "Claymore")
                throw new InvalidOperationException("Invalid weapon type.");
            else if (type == "Mace")
                weapons.Add(new Mace(name, durability));
            else
                weapons.Add(new Claymore(name, durability));

            return $"A {type.ToLower()} {name} is added to the collection.";
        }

        public string HeroReport()
        {
            var sortedHeroes = heroes.Models.OrderBy(x => x.GetType().Name).ThenByDescending(x => x.Health).ThenBy(x => x.Name);

            StringBuilder sb = new StringBuilder();

            foreach (var hero in sortedHeroes)
            {
                string weaponName = "Unarmed";

                if (hero.Weapon != null)
                    weaponName = hero.Weapon.Name;

                sb.AppendLine($"{hero.GetType().Name}: {hero.Name}");
                sb.AppendLine($"--Health: {hero.Health}");
                sb.AppendLine($"--Armour: {hero.Armour}");
                sb.AppendLine($"--Weapon: {weaponName}");
            }

            return sb.ToString().TrimEnd();
        }

        public string StartBattle()
        {
            Map map = new Map();
            List<IHero> readyHeroes = heroes.Models.Where(x => x.IsAlive && x.Weapon != null).ToList();
            return map.Fight(readyHeroes);
        }
    }
}