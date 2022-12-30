using PlanetWars.Core.Contracts;
using PlanetWars.Models.MilitaryUnits;
using PlanetWars.Models.Planets;
using PlanetWars.Models.Planets.Contracts;
using PlanetWars.Models.Weapons;
using PlanetWars.Repositories;
using PlanetWars.Utilities.Messages;
using System;
using System.Linq;
using System.Text;

namespace PlanetWars.Core
{
    public class Controller : IController
    {
        private PlanetRepository planets;

        public Controller()
        {
            planets = new PlanetRepository();
        }

        public string AddUnit(string unitTypeName, string planetName)
        {
            var selectedPlanet = planets.FindByName(planetName);

            if (selectedPlanet == null)
                throw new InvalidOperationException(string.Format(ExceptionMessages.UnexistingPlanet, planetName));

            if (unitTypeName != "AnonymousImpactUnit" && unitTypeName != "SpaceForces" && unitTypeName != "StormTroopers")
                throw new InvalidOperationException(string.Format(ExceptionMessages.ItemNotAvailable, unitTypeName));

            foreach (var item in selectedPlanet.Army)
                if (item.GetType().Name == unitTypeName)
                    throw new InvalidOperationException(string.Format(ExceptionMessages.UnitAlreadyAdded, unitTypeName, planetName));                    

            switch (unitTypeName)
            {
                case "AnonymousImpactUnit":
                    selectedPlanet.Spend(30);
                    selectedPlanet.AddUnit(new AnonymousImpactUnit());                    
                    break;

                case "SpaceForces":
                    selectedPlanet.Spend(11);
                    selectedPlanet.AddUnit(new SpaceForces());                    
                    break;

                case "StormTroopers":
                    selectedPlanet.Spend(2.5);
                    selectedPlanet.AddUnit(new StormTroopers());                    
                    break;
            }

            return string.Format(OutputMessages.UnitAdded, unitTypeName, planetName);
        }

        public string AddWeapon(string planetName, string weaponTypeName, int destructionLevel)
        {
            var selectedPlanet = planets.FindByName(planetName);

            if (selectedPlanet == null)
                throw new InvalidOperationException(string.Format(ExceptionMessages.UnexistingPlanet, planetName));

            if (weaponTypeName != "BioChemicalWeapon" && weaponTypeName != "NuclearWeapon" && weaponTypeName != "SpaceMissiles")
                throw new InvalidOperationException(string.Format(ExceptionMessages.ItemNotAvailable, weaponTypeName));

            foreach (var item in selectedPlanet.Weapons)
                if (item.GetType().Name == weaponTypeName)
                    throw new InvalidOperationException(string.Format(ExceptionMessages.WeaponAlreadyAdded, weaponTypeName, planetName));

            switch (weaponTypeName)
            {
                case "BioChemicalWeapon":
                    if (destructionLevel >= 1 && destructionLevel <= 10)
                        selectedPlanet.Spend(3.2);
                    selectedPlanet.AddWeapon(new BioChemicalWeapon(destructionLevel));                        
                    break;

                case "NuclearWeapon":
                    if (destructionLevel >= 1 && destructionLevel <= 10)
                        selectedPlanet.Spend(15);
                    selectedPlanet.AddWeapon(new NuclearWeapon(destructionLevel));
                    break;

                case "SpaceMissiles":
                    if (destructionLevel >= 1 && destructionLevel <= 10)
                        selectedPlanet.Spend(8.75);
                    selectedPlanet.AddWeapon(new SpaceMissiles(destructionLevel));
                    break;
            }

            return string.Format(OutputMessages.WeaponAdded, planetName, weaponTypeName);
        }

        public string CreatePlanet(string name, double budget)
        {
            IPlanet newPlanet = new Planet(name, budget);

            if (planets.FindByName(name) != null)
                return string.Format(OutputMessages.ExistingPlanet, name);
            else
            {
                planets.AddItem(newPlanet);
                return string.Format(OutputMessages.NewPlanet, name);
            }
        }

        public string ForcesReport()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("***UNIVERSE PLANET MILITARY REPORT***");

            foreach (var item in planets.Models.OrderByDescending(x => x.MilitaryPower).ThenBy(x => x.Name))
            {
                sb.AppendLine(item.PlanetInfo());
            }

            return sb.ToString().TrimEnd();
        }

        public string SpaceCombat(string planetOne, string planetTwo)
        {
            var firstPlanet = planets.FindByName(planetOne);
            var secondPlanet = planets.FindByName(planetTwo);

            bool firstPlanetWin = false;
            bool secondPlanetWin = false;

            if (firstPlanet.MilitaryPower > secondPlanet.MilitaryPower) 
                firstPlanetWin = true;
            else if (firstPlanet.MilitaryPower < secondPlanet.MilitaryPower)
                secondPlanetWin = true;
            else
            {
                bool firstGotNuke = false;
                bool secondGotNuke = false;

                foreach (var weapon in firstPlanet.Weapons) 
                    if (weapon.GetType().Name == "NuclearWeapon")
                        firstGotNuke = true;

                foreach (var weapon in secondPlanet.Weapons)
                    if (weapon.GetType().Name == "NuclearWeapon")
                        secondGotNuke = true;

                if (firstGotNuke && !secondGotNuke)
                    firstPlanetWin = true;
                else if (!firstGotNuke && secondGotNuke)
                    secondPlanetWin = true;
                else
                {
                    firstPlanet.Spend(firstPlanet.Budget / 2);
                    secondPlanet.Spend(secondPlanet.Budget / 2);
                    return OutputMessages.NoWinner;
                }
            }

            if (firstPlanetWin)
            {
                firstPlanet.Spend(firstPlanet.Budget / 2);
                firstPlanet.Profit(secondPlanet.Budget / 2);

                foreach (var item in secondPlanet.Weapons)
                {
                    if (item.GetType().Name == "BioChemicalWeapon")
                        firstPlanet.Profit(3.2);
                    if (item.GetType().Name == "NuclearWeapon")
                        firstPlanet.Profit(15);
                    if (item.GetType().Name == "SpaceMissiles")
                        firstPlanet.Profit(8.75);
                }

                foreach (var item in secondPlanet.Army)
                {
                    if (item.GetType().Name == "AnonymousImpactUnit")
                        firstPlanet.Profit(30);
                    if (item.GetType().Name == "SpaceForces")
                        firstPlanet.Profit(11);
                    if (item.GetType().Name == "StormTroopers")
                        firstPlanet.Profit(2.5);
                }

                planets.RemoveItem(planetTwo);
                return string.Format(OutputMessages.WinnigTheWar, planetOne, planetTwo);
            }
            else
            {
                secondPlanet.Spend(secondPlanet.Budget / 2);
                secondPlanet.Profit(firstPlanet.Budget / 2);

                foreach (var item in firstPlanet.Weapons)
                {
                    if (item.GetType().Name == "BioChemicalWeapon")
                        secondPlanet.Profit(3.2);
                    if (item.GetType().Name == "NuclearWeapon")
                        secondPlanet.Profit(15);
                    if (item.GetType().Name == "SpaceMissiles")
                        secondPlanet.Profit(8.75);
                }

                foreach (var item in firstPlanet.Army)
                {
                    if (item.GetType().Name == "AnonymousImpactUnit")
                        secondPlanet.Profit(30);
                    if (item.GetType().Name == "SpaceForces")
                        secondPlanet.Profit(11);
                    if (item.GetType().Name == "StormTroopers")
                        secondPlanet.Profit(2.5);
                }

                planets.RemoveItem(planetOne);
                return string.Format(OutputMessages.WinnigTheWar, planetTwo, planetOne);
            }
        }

        public string SpecializeForces(string planetName)
        {
            if (planets.FindByName(planetName) == null)
                throw new InvalidOperationException(string.Format(ExceptionMessages.UnexistingPlanet, planetName));

            if (planets.FindByName(planetName).Army.Count == 0)
                throw new InvalidOperationException(ExceptionMessages.NoUnitsFound);

            foreach (var unit in planets.FindByName(planetName).Army)
                unit.IncreaseEndurance();

            planets.FindByName(planetName).Spend(1.25);

            return string.Format(OutputMessages.ForcesUpgraded, planetName);
        }

        public void Peace()
        {
            Environment.Exit(0);
        }
    }
}
