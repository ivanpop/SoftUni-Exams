using CarRacing.Core.Contracts;
using CarRacing.Models.Cars;
using CarRacing.Models.Cars.Contracts;
using CarRacing.Models.Maps;
using CarRacing.Models.Maps.Contracts;
using CarRacing.Models.Racers;
using CarRacing.Models.Racers.Contracts;
using CarRacing.Repositories;
using System;
using System.Linq;
using System.Text;

namespace CarRacing.Core
{
    public class Controller : IController
    {
        private CarRepository cars;
        private RacerRepository racers;
        private IMap map;

        public Controller()
        {
            cars = new CarRepository();
            racers = new RacerRepository();
            map = new Map();
        }

        public string AddCar(string type, string make, string model, string VIN, int horsePower)
        {
            if (type != "SuperCar" && type != "TunedCar")
                throw new ArgumentException(Utilities.Messages.ExceptionMessages.InvalidCarType);

            if (type == "SuperCar")
                cars.Add(new SuperCar(make, model, VIN, horsePower));
            else
                cars.Add(new TunedCar(make, model, VIN, horsePower));

            return string.Format(Utilities.Messages.OutputMessages.SuccessfullyAddedCar, make, model, VIN);
        }
        public string AddRacer(string type, string username, string carVIN)
        {
            ICar newCar = cars.FindBy(carVIN);

            if (newCar == null)
                throw new ArgumentException(Utilities.Messages.ExceptionMessages.CarCannotBeFound);

            if (type != "ProfessionalRacer" && type != "StreetRacer")
                throw new ArgumentException(Utilities.Messages.ExceptionMessages.InvalidRacerType);

            if (type == "ProfessionalRacer")
                racers.Add(new ProfessionalRacer(username, newCar));
            else
                racers.Add(new StreetRacer(username, newCar));

            return string.Format(Utilities.Messages.OutputMessages.SuccessfullyAddedRacer, username);
        }
        public string BeginRace(string racerOneUsername, string racerTwoUsername)
        {
            IRacer racerOne = racers.FindBy(racerOneUsername);
            IRacer racerTwo = racers.FindBy(racerTwoUsername);

            if (racerOne == null)
                throw new ArgumentException(string.Format(Utilities.Messages.ExceptionMessages.RacerCannotBeFound, racerOneUsername));
            else if (racerTwo == null)
                throw new ArgumentException(string.Format(Utilities.Messages.ExceptionMessages.RacerCannotBeFound, racerTwoUsername));
            else
                return map.StartRace(racerOne, racerTwo);
        }
        public string Report()
        {
            var orderedRacers = racers.Models.OrderByDescending(x => x.DrivingExperience).ThenBy(y => y.Username).ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var racer in orderedRacers)
                sb.AppendLine(racer.ToString());

            return sb.ToString().TrimEnd();
        }
    }
}