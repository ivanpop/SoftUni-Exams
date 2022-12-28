using OnlineShop.Models.Products.Components;
using OnlineShop.Models.Products.Peripherals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnlineShop.Models.Products.Computers
{
    public abstract class Computer : Product, IComputer
    {
        private List<IComponent> components;
        private List<IPeripheral> peripherals;

        public Computer(int id, string manufacturer, string model, decimal price, double overallPerformance) : base(id, manufacturer, model, price, overallPerformance)
        {
            components = new List<IComponent>();
            peripherals = new List<IPeripheral>();
        }

        public IReadOnlyCollection<IComponent> Components => components.AsReadOnly();
        public IReadOnlyCollection<IPeripheral> Peripherals => peripherals.AsReadOnly();
        public void AddComponent(IComponent component)
        {
            if (components.Any(x => x.GetType().Name == component.GetType().Name))
                throw new ArgumentException(string.Format(Common.Constants.ExceptionMessages.ExistingComponent, component.GetType().Name, this.GetType().Name, Id));

            components.Add(component);
        }
        public void AddPeripheral(IPeripheral peripheral)
        {
            if (peripherals.Any(x => x.GetType().Name == peripheral.GetType().Name))
                throw new ArgumentException(string.Format(Common.Constants.ExceptionMessages.ExistingPeripheral, peripheral.GetType().Name, this.GetType().Name, Id));

            peripherals.Add(peripheral);
        }
        public IComponent RemoveComponent(string componentType)
        {
            if (!components.Any() || !components.Any(x => x.GetType().Name == componentType))
                throw new ArgumentException(string.Format(Common.Constants.ExceptionMessages.NotExistingComponent, componentType, GetType().Name, Id));

            var component = components.First(x => x.GetType().Name == componentType);
            components.Remove(component);

            return component;
        }
        public IPeripheral RemovePeripheral(string peripheralType)
        {
            if (!peripherals.Any() || !peripherals.Any(x => x.GetType().Name == peripheralType))
                throw new ArgumentException(string.Format(Common.Constants.ExceptionMessages.NotExistingPeripheral, peripheralType, GetType().Name, Id));

            var peripheral = peripherals.First(x => x.GetType().Name == peripheralType);
            peripherals.Remove(peripheral);

            return peripheral;
        }
        public override double OverallPerformance 
        {
            get
            {
                if (!components.Any())
                    return base.OverallPerformance;
                else
                    return base.OverallPerformance + components.Average(x => x.OverallPerformance);
                  
            }
        }
        public override decimal Price => base.Price + components.Sum(x => x.Price) + peripherals.Sum(y => y.Price);

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(base.ToString());
            sb.AppendLine();
            sb.AppendLine($" Components ({components.Count}):");

            foreach (var component in components)
                sb.AppendLine("  " + component.ToString());

            double avgOverallPerformance = 0;

            if (peripherals.Any())
                avgOverallPerformance = peripherals.Average(x => x.OverallPerformance);

            sb.AppendLine($" Peripherals ({peripherals.Count}); Average Overall Performance ({avgOverallPerformance:f2}):");

            foreach (var peripheral in peripherals)
                sb.AppendLine("  " + peripheral.ToString());

            return sb.ToString().TrimEnd();
        }
    }
}