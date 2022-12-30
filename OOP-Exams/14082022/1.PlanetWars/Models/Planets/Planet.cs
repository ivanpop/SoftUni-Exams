using System;
using PlanetWars.Models.MilitaryUnits.Contracts;
using System.Collections.Generic;
using PlanetWars.Repositories;
using PlanetWars.Utilities.Messages;
using PlanetWars.Models.Weapons.Contracts;
using System.Text;
using PlanetWars.Models.Planets.Contracts;

namespace PlanetWars.Models.Planets
{
    public class Planet : IPlanet
    {
		private string name;
		private double budget;
		private UnitRepository units;
		private WeaponRepository weapons;

		public Planet(string name, double budget)
		{
			units = new UnitRepository();
			weapons = new WeaponRepository();
			Name = name;
			Budget = budget;
		}

        public double Budget
		{
			get { return budget; }
			private set 
			{
				if (value < 0)
					throw new ArgumentException(ExceptionMessages.InvalidBudgetAmount);

				budget = value;
			}
		}

        public double MilitaryPower => Math.Round(this.CalculateMilitaryPower(), 3);

        public string Name
		{
			get { return name; }
			private set
			{
				if (string.IsNullOrWhiteSpace(value))
					throw new ArgumentException(ExceptionMessages.InvalidPlanetName);

				name = value;
			}
		}

		public IReadOnlyCollection<IMilitaryUnit> Army => units.Models;
        public IReadOnlyCollection<IWeapon> Weapons => weapons.Models;
		public void AddUnit(IMilitaryUnit unit)
		{
			units.AddItem(unit);
			CalculateMilitaryPower();
        }
        public void AddWeapon(IWeapon weapon)
        {
            weapons.AddItem(weapon);
			CalculateMilitaryPower();
        }		
		public void TrainArmy()
		{
			foreach (var unit in units.Models)
				unit.IncreaseEndurance();
		}
		public void Spend(double amount)
		{
			if (Budget < amount) 
				throw new InvalidOperationException(ExceptionMessages.UnsufficientBudget);

			Budget -= amount;
		}
		public void Profit(double amount)
		{
			Budget += amount;
		}
		public string PlanetInfo()
		{
			StringBuilder sb = new StringBuilder();

			sb.AppendLine($"Planet: {Name}");
            sb.AppendLine($"--Budget: {Budget} billion QUID");
            sb.Append("--Forces: ");

			if (units.Models.Count > 0)
			{
				foreach (var unit in units.Models)
                    sb.Append(unit.GetType().Name + ", ");
                sb.Remove(sb.Length - 2, 2);
                sb.AppendLine();
            }				
			else
				sb.AppendLine("No units");

            sb.Append("--Combat equipment: ");

            if (weapons.Models.Count > 0)
			{
				foreach (var weapon in weapons.Models)
					sb.Append(weapon.GetType().Name + ", ");
				sb.Remove(sb.Length - 2, 2);
				sb.AppendLine();
            }   
            else
                sb.AppendLine("No weapons");

            sb.AppendLine($"--Military Power: {this.MilitaryPower}");

            return sb.ToString().TrimEnd();
		}
		private double CalculateMilitaryPower()
		{
            double sum = 0;

            foreach (var endurances in units.Models)
                sum += endurances.EnduranceLevel;

            foreach (var destructionLevels in weapons.Models)
                sum += destructionLevels.DestructionLevel;

            if (units.FindByName("AnonymousImpactUnit") != null)
                sum += sum * 0.3;

            if (weapons.FindByName("NuclearWeapon") != null)
                sum += sum * 0.45;

            return Math.Round(sum, 3);
        }
    }
}