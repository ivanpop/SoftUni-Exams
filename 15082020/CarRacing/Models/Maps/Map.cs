using CarRacing.Models.Maps.Contracts;
using CarRacing.Models.Racers.Contracts;

namespace CarRacing.Models.Maps
{
    public class Map : IMap
    {
        public string StartRace(IRacer racerOne, IRacer racerTwo)
        {
            if (!racerOne.IsAvailable() && !racerTwo.IsAvailable())
                return Utilities.Messages.OutputMessages.RaceCannotBeCompleted;

            if (!racerOne.IsAvailable())
                return string.Format(Utilities.Messages.OutputMessages.OneRacerIsNotAvailable, racerTwo.Username, racerOne.Username);

            if (!racerTwo.IsAvailable())
                return string.Format(Utilities.Messages.OutputMessages.OneRacerIsNotAvailable, racerOne.Username, racerTwo.Username);

            racerOne.Race();
            racerTwo.Race();

            double racerOneChance = racerOne.Car.HorsePower * racerOne.DrivingExperience;

            if (racerOne.RacingBehavior == "strict")
                racerOneChance *= 1.2;
            else
                racerOneChance *= 1.1;

            double racerTwoChance = racerTwo.Car.HorsePower * racerTwo.DrivingExperience;

            if (racerTwo.RacingBehavior == "strict")
                racerTwoChance *= 1.2;
            else
                racerTwoChance *= 1.1;

            string winner = string.Empty;

            if (racerOneChance > racerTwoChance)
                winner = racerOne.Username;
            else 
                winner = racerTwo.Username;

            return string.Format(Utilities.Messages.OutputMessages.RacerWinsRace, racerOne.Username, racerTwo.Username, winner);
        }
    }
}