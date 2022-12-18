using PlanetWars.Models.Weapons.Contracts;
using PlanetWars.Utilities.Messages;
using System;

namespace PlanetWars.Models.Weapons
{
    public abstract class Weapon : IWeapon
    {
        private double price;
        private int destructionLevel;

        public Weapon(int destructionLevel, double price)
        {
            DestructionLevel = destructionLevel;
            Price = price;
        }

        public double Price 
        { 
            get => price;
            private set => price = value;
        }

        public int DestructionLevel 
        {
            get { return destructionLevel; }
            private set
            {
                if (value < 1)
                    throw new ArgumentException(ExceptionMessages.TooLowDestructionLevel);
                else if (value > 10)
                    throw new ArgumentException(ExceptionMessages.TooHighDestructionLevel);
                else destructionLevel = value;
            }
        }
    }
}