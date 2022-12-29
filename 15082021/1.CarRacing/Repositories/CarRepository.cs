using CarRacing.Models.Cars.Contracts;
using CarRacing.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CarRacing.Repositories
{
    public class CarRepository : IRepository<ICar>
    {
        private List<ICar> cars;

        public CarRepository()
        {
            cars = new List<ICar>();
        }

        public IReadOnlyCollection<ICar> Models => cars.AsReadOnly();
        public void Add(ICar model)
        {
            if (model == null)
                throw new ArgumentException(Utilities.Messages.ExceptionMessages.InvalidAddCarRepository);

            cars.Add(model);
        }
        public bool Remove(ICar model) => cars.Remove(model);
        public ICar FindBy(string property) => cars.FirstOrDefault(x => x.VIN == property);
    }
}