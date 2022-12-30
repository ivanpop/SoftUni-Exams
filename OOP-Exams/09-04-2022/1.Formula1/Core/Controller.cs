using Formula1.Core.Contracts;
using Formula1.Models;
using Formula1.Models.Contracts;
using Formula1.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Formula1.Core
{
    public class Controller : IController
    {
        private PilotRepository pilots;
        private RaceRepository races;
        private FormulaOneCarRepository cars;

        public Controller()
        {
            pilots = new PilotRepository();
            races = new RaceRepository();
            cars = new FormulaOneCarRepository();
        }

        public string AddCarToPilot(string pilotName, string carModel)
        {
            if (pilots.FindByName(pilotName) == default || pilots.FindByName(pilotName).Car != default)
                throw new InvalidOperationException(string.Format(Utilities.ExceptionMessages.PilotDoesNotExistOrHasCarErrorMessage, pilotName));

            if (!cars.Models.Any(x => x.Model == carModel))
                throw new NullReferenceException(string.Format(Utilities.ExceptionMessages.CarDoesNotExistErrorMessage, carModel));

            IFormulaOneCar car = cars.Models.First(x => x.Model == carModel);
            pilots.FindByName(pilotName).AddCar(car);
            cars.Remove(car);

            return string.Format(Utilities.OutputMessages.SuccessfullyPilotToCar, pilotName, car.GetType().Name, carModel);
        }

        public string AddPilotToRace(string raceName, string pilotFullName)
        {
            if (!races.Models.Any(x => x.RaceName == raceName))
                throw new NullReferenceException(string.Format(Utilities.ExceptionMessages.RaceDoesNotExistErrorMessage, raceName));

            IRace race = races.FindByName(raceName);
            IPilot pilot = pilots.FindByName(pilotFullName);

            if (pilot == default || !pilot.CanRace || race.Pilots != null && race.Pilots.Any(x => x.FullName == pilotFullName))
                throw new InvalidOperationException(string.Format(Utilities.ExceptionMessages.PilotDoesNotExistErrorMessage, pilotFullName));

            race.Pilots.Add(pilot);

            return string.Format(Utilities.OutputMessages.SuccessfullyAddPilotToRace, pilotFullName, raceName);
        }

        public string CreateCar(string type, string model, int horsepower, double engineDisplacement)
        {
            if (cars.Models.Any(x => x.Model == model))
                throw new InvalidOperationException(string.Format(Utilities.ExceptionMessages.CarExistErrorMessage, model));

            if (type != "Ferrari" && type != "Williams")
                throw new InvalidOperationException(string.Format(Utilities.ExceptionMessages.InvalidTypeCar, type));

            if (type == "Ferrari")
                cars.Add(new Ferrari(model, horsepower, engineDisplacement));
            else
                cars.Add(new Williams(model, horsepower, engineDisplacement));

            return string.Format(Utilities.OutputMessages.SuccessfullyCreateCar, type, model);
        }

        public string CreatePilot(string fullName)
        {
            if (pilots.FindByName(fullName) != default) 
                throw new InvalidOperationException(string.Format(Utilities.ExceptionMessages.PilotExistErrorMessage, fullName));

            pilots.Add(new Pilot(fullName));

            return string.Format(Utilities.OutputMessages.SuccessfullyCreatePilot, fullName);
        }

        public string CreateRace(string raceName, int numberOfLaps)
        {
            if (races.Models.Any(x => x.RaceName == raceName))
                throw new InvalidOperationException(string.Format(Utilities.ExceptionMessages.RaceExistErrorMessage, raceName));

            races.Add(new Race(raceName, numberOfLaps));

            return string.Format(Utilities.OutputMessages.SuccessfullyCreateRace, raceName);
        }

        public string PilotReport()
        {
            var orderedPilots = pilots.Models.OrderByDescending(x => x.NumberOfWins).ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var pilot in orderedPilots)
                sb.AppendLine(pilot.ToString());

            return sb.ToString().TrimEnd();
        }

        public string RaceReport()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var race in races.Models.Where(x => x.TookPlace))
                sb.AppendLine(race.RaceInfo());

            return sb.ToString().TrimEnd();
        }

        public string StartRace(string raceName)
        {
            IRace race = races.FindByName(raceName);

            if (race == default)
                throw new NullReferenceException(string.Format(Utilities.ExceptionMessages.RaceDoesNotExistErrorMessage, raceName));

            if (race.Pilots == null || race.Pilots.Count < 3)
                throw new InvalidOperationException(string.Format(Utilities.ExceptionMessages.InvalidRaceParticipants, raceName));

            if (race.TookPlace)
                throw new InvalidOperationException(string.Format(Utilities.ExceptionMessages.RaceTookPlaceErrorMessage, raceName));

            var orderedPilots = race.Pilots.OrderByDescending(x => x.Car.RaceScoreCalculator(race.NumberOfLaps));
            race.TookPlace = true;
            orderedPilots.First().WinRace();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Pilot {orderedPilots.First().FullName} wins the {raceName} race.");
            sb.AppendLine($"Pilot {orderedPilots.Skip(1).First().FullName} is second in the {raceName} race.");
            sb.AppendLine($"Pilot {orderedPilots.Skip(2).First().FullName} is third in the {raceName} race.");

            return sb.ToString().TrimEnd();
        }
    }
}