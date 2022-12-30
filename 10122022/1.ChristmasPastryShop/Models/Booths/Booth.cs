using ChristmasPastryShop.Models.Booths.Contracts;
using ChristmasPastryShop.Models.Cocktails.Contracts;
using ChristmasPastryShop.Models.Delicacies.Contracts;
using ChristmasPastryShop.Repositories;
using ChristmasPastryShop.Repositories.Contracts;
using System;
using System.Text;

namespace ChristmasPastryShop.Models.Booths
{
    public class Booth : IBooth
    {
        private int boothId;
        private int capacity;
        private IRepository<IDelicacy> delicacyMenu;
        private IRepository<ICocktail> cocktailMenu;
        private double currentBill = 0;
        private double turnover = 0;
        private bool isReserved = false;

        public Booth(int boothId, int capacity)
        {
            BoothId = boothId;
            Capacity = capacity;
            delicacyMenu = new DelicacyRepository();
            cocktailMenu = new CocktailRepository();
        }

        public int BoothId 
        { 
            get { return boothId; }
            private set { boothId = value; }
        }
        public int Capacity 
        { 
            get { return capacity; }
            private set 
            {
                if (value <= 0)
                    throw new ArgumentException(Utilities.Messages.ExceptionMessages.CapacityLessThanOne);

                capacity = value; 
            }
        }
        public IRepository<IDelicacy> DelicacyMenu
        {
            get { return delicacyMenu; }
            private set
            {
                delicacyMenu = value;
            }
        }
        public IRepository<ICocktail> CocktailMenu
        {
            get { return cocktailMenu; }
            private set
            {
                cocktailMenu = value;
            }
        }
        public double CurrentBill 
        {
            get { return currentBill; }
            private set { currentBill = value; }
        }
        public double Turnover 
        { 
            get { return turnover; }
            private set { turnover = value; }
        }
        public bool IsReserved => isReserved;
        public void ChangeStatus()
        {
            if (IsReserved)
                isReserved = false;
            else 
                isReserved = true;
        }
        public void Charge()
        {
            Turnover += CurrentBill;
            CurrentBill = 0;
        }
        public void UpdateCurrentBill(double amount)
        {
            CurrentBill += amount;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Booth: {BoothId}");
            sb.AppendLine($"Capacity: {Capacity}");
            sb.AppendLine($"Turnover: {Turnover:f2} lv");
            sb.AppendLine("-Cocktail menu:");

            foreach (var cocktail in CocktailMenu.Models)
                sb.AppendLine($"--{cocktail.ToString()}");

            sb.AppendLine("-Delicacy menu:");

            foreach (var delicacy in DelicacyMenu.Models)
                sb.AppendLine($"--{delicacy.ToString()}");

            return sb.ToString().TrimEnd();
        }
    }
}