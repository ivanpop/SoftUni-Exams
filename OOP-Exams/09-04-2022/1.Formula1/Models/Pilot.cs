using Formula1.Models.Contracts;
using Formula1.Utilities;
using System;

namespace Formula1.Models
{
    public class Pilot : IPilot
    {
        private string fullname;
        private bool canRace = false;
        private IFormulaOneCar car;
        private int numberOfWins;

        public Pilot(string fullname)
        {
            FullName = fullname;
        }

        public string FullName
        {
            get { return fullname; }
            private set {
                if (string.IsNullOrWhiteSpace(value) || value.Length < 5)
                    throw new ArgumentException(string.Format(ExceptionMessages.InvalidPilot, value));
                fullname = value;
            }
        }
        public IFormulaOneCar Car 
        {
            get { return car; }
            private set
            {
                if (value == null)
                    throw new NullReferenceException(ExceptionMessages.InvalidCarForPilot);

                car = value;
            }
        }
        public int NumberOfWins
        {
            get { return numberOfWins; }
            private set { numberOfWins = value; }
        }
        public bool CanRace { 
            get { return canRace; }
            private set { canRace = value; }
        }
        public void AddCar(IFormulaOneCar car)
        {
            Car = car;
            CanRace = true;
        }
        public void WinRace()
        {
            NumberOfWins++;
        }
        public override string ToString() => ($"Pilot {FullName} has {NumberOfWins} wins.");
    }
}