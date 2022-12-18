using CarRacing.Models.Cars.Contracts;
using System;

namespace CarRacing.Models.Cars
{
    public abstract class Car : ICar
    {
        private string make;
        private string model;
        private string vin;
        private int horsePower;
        private double fuelAvailable;
        private double fuelConsumptionPerRace;

        public Car(string make, string model, string VIN, int horsePower, double fuelAvailable, double fuelConsumptionPerRace)
        {
            Make = make;
            Model = model;
            this.VIN = VIN;
            HorsePower = horsePower;
            FuelAvailable = fuelAvailable;
            FuelConsumptionPerRace = fuelConsumptionPerRace;
        }

        public double FuelConsumptionPerRace
        {
            get { return fuelConsumptionPerRace; }
            private set 
            { 
                if (value < 0)
                    throw new ArgumentException(Utilities.Messages.ExceptionMessages.InvalidCarFuelConsumption);

                fuelConsumptionPerRace = value; 
            }
        }
        public double FuelAvailable
        {
            get { return fuelAvailable; }
            private set 
            {
                if (value < 0)
                    fuelAvailable = 0;
                else 
                    fuelAvailable = value;
            }
        }
        public int HorsePower
        {
            get { return horsePower; }
            protected set 
            { 
                if (value < 0)
                    throw new ArgumentException(Utilities.Messages.ExceptionMessages.InvalidCarHorsePower);

                horsePower = value;
            }
        }
        public string VIN
        {
            get { return vin; }
            private set 
            { 
                if (value.Length != 17)
                    throw new ArgumentException(Utilities.Messages.ExceptionMessages.InvalidCarVIN);

                vin = value;
            }
        }
        public string Model
        {
            get { return model; }
            private set 
            { 
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException(Utilities.Messages.ExceptionMessages.InvalidCarModel);

                model = value;
            }
        }
        public string Make
        {
            get { return make; }
            private set 
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException(Utilities.Messages.ExceptionMessages.InvalidCarMake);

                make = value;
            }
        }
        public void Drive()
        {
            FuelAvailable -= FuelConsumptionPerRace;

            if (GetType().Name == "TunedCar")
                HorsePower = (int)(HorsePower * 0.97);
        }
    }
}