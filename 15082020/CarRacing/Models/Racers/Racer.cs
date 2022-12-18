using CarRacing.Models.Cars.Contracts;
using CarRacing.Models.Racers.Contracts;
using System;
using System.Text;

namespace CarRacing.Models.Racers
{
    public abstract class Racer : IRacer
    {
        private string username;
        private string racingBehavior;
        private int drivingExperience;
        private ICar car;

        public Racer(string username, string racingBehavior, int drivingExperience, ICar car)
        {
            Username = username;
            RacingBehavior = racingBehavior;
            DrivingExperience = drivingExperience;
            Car = car;
        }

        public string Username
        {
            get { return username; }
            private set 
            { 
                if (string.IsNullOrWhiteSpace(value)) 
                    throw new ArgumentException(Utilities.Messages.ExceptionMessages.InvalidRacerName);
                
                username = value; 
            }
        }
        public string RacingBehavior
        {
            get { return racingBehavior; }
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException(Utilities.Messages.ExceptionMessages.InvalidRacerBehavior);

                racingBehavior = value;
            }
        }
        public int DrivingExperience
        {
            get { return drivingExperience; }
            protected set
            {
                if (value < 0 || value > 100)
                    throw new ArgumentNullException(Utilities.Messages.ExceptionMessages.InvalidRacerDrivingExperience);

                drivingExperience = value;
            }
        }
        public ICar Car 
        { 
            get { return car; } 
            private set 
            {
                if (value == null)
                    throw new ArgumentException(Utilities.Messages.ExceptionMessages.InvalidRacerCar);

                car = value;
            }
        }
        public bool IsAvailable() => car.FuelAvailable >= car.FuelConsumptionPerRace;
        public void Race()
        {
            car.Drive();

            if (GetType().Name == "StreetRacer")
                DrivingExperience += 5;
            else
                DrivingExperience += 10;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"{GetType().Name}: {Username}");
            sb.AppendLine($"--Driving behavior: {RacingBehavior}");
            sb.AppendLine($"--Driving experience: {DrivingExperience}");
            sb.AppendLine($"--Car: {car.Make} {car.Model} ({car.VIN})");

            return sb.ToString().TrimEnd();
        }
    }
}