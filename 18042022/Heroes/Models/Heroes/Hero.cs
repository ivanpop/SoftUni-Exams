using Heroes.Models.Contracts;
using Heroes.Models.Weapons;
using System;

namespace Heroes.Models.Heroes
{
    public abstract class Hero : IHero
    {
        private string name;
        private int health;
        private int armour;
        private IWeapon weapon;

        public Hero(string name, int health, int armour)
        {
            Name = name;
            Health = health;
            Armour = armour;
        }

        public string Name 
        {
            get => name;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Hero name cannot be null or empty.");

                name = value;
            }
        }
        public int Health
        {
            get => health;
            private set
            {
                health = value;

                if (health < 0)
                    throw new ArgumentException("Hero health cannot be below 0.");
            }
        }
        public int Armour
        {
            get => armour;
            private set
            {
                armour = value;

                if (value < 0)
                    throw new ArgumentException("Hero armour cannot be below 0.");
            }
        }

        public IWeapon Weapon
        {
            get => weapon;
            set
            {
                if (value == null)
                    throw new ArgumentException("Weapon cannot be null.");

                weapon = value;
            }

        }
        public bool IsAlive => Health > 0 ? true : false;
        public void AddWeapon(IWeapon weapon)
        {
            Weapon = weapon;
        }
        public void TakeDamage(int points)
        {
            if (points <= Armour)
                Armour -= points;
            else
            {
                points -= Armour;
                Armour = 0;
                if (points <= Health)
                    Health -= points;
                else
                    Health = 0;                
            }
        }
    }
}