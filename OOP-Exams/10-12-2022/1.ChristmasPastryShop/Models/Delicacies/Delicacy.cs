using ChristmasPastryShop.Models.Delicacies.Contracts;
using System;

namespace ChristmasPastryShop.Models.Delicacies
{
    public abstract class Delicacy : IDelicacy
    {
        private string name;
        private double price;

        public Delicacy(string delicacyName, double price)
        {
            Name = delicacyName;
            Price = price;
        }

        public string Name { 
            get { return name; } 
            private set 
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException(Utilities.Messages.ExceptionMessages.NameNullOrWhitespace);

                name = value;
            }
        }

        public double Price
        {
            get { return price; }
            private set { price = value; } 
        }

        public override string ToString()
        {
            return $"{Name} - {Price:f2} lv";
        }
    }
}