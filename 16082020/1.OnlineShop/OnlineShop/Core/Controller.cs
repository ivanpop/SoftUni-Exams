using OnlineShop.Models.Products.Components;
using OnlineShop.Models.Products.Computers;
using OnlineShop.Models.Products.Peripherals;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlineShop.Core
{
    public class Controller : IController
    {
        private List<Computer> computers;
        private List<Component> components;
        private List<Peripheral> peripherals;

        public Controller()
        {
            computers = new List<Computer>();
            components = new List<Component>();
            peripherals = new List<Peripheral>();
        }

        public string AddComponent(int computerId, int id, string componentType, string manufacturer, string model, decimal price, double overallPerformance, int generation)
        {
            if (!computers.Any(x => x.Id == computerId))
                throw new ArgumentException(Common.Constants.ExceptionMessages.NotExistingComputerId);

            Component component = null;

            switch (componentType)
            {
                case "CentralProcessingUnit": component = new CentralProcessingUnit(id, manufacturer, model, price, overallPerformance, generation); break;
                case "Motherboard": component = new Motherboard(id, manufacturer, model, price, overallPerformance, generation); break;
                case "PowerSupply": component = new PowerSupply(id, manufacturer, model, price, overallPerformance, generation); break;
                case "RandomAccessMemory": component = new RandomAccessMemory(id, manufacturer, model, price, overallPerformance, generation); break;
                case "SolidStateDrive": component = new SolidStateDrive(id, manufacturer, model, price, overallPerformance, generation); break;
                case "VideoCard": component = new VideoCard(id, manufacturer, model, price, overallPerformance, generation); break;
                default: throw new ArgumentException(Common.Constants.ExceptionMessages.InvalidComponentType);
            }

            if (components.Any(x => x.Id == id))
                throw new ArgumentException(Common.Constants.ExceptionMessages.ExistingComponentId);

            var computer = computers.First(x => x.Id == computerId);

            components.Add(component);
            computer.AddComponent(component);

            return string.Format(Common.Constants.SuccessMessages.AddedComponent, componentType, id, computerId);
        }

        public string AddComputer(string computerType, int id, string manufacturer, string model, decimal price)
        {
            if (computers.Any(x => x.Id == id))
                throw new ArgumentException(Common.Constants.ExceptionMessages.ExistingComputerId);

            if (computerType != "DesktopComputer" && computerType != "Laptop")
                throw new ArgumentException(Common.Constants.ExceptionMessages.InvalidComputerType);

            if (computerType == "DesktopComputer")
                computers.Add(new DesktopComputer(id, manufacturer, model, price));
            else 
                computers.Add(new Laptop(id, manufacturer, model, price));

            return string.Format(Common.Constants.SuccessMessages.AddedComputer, id);
        }

        public string AddPeripheral(int computerId, int id, string peripheralType, string manufacturer, string model, decimal price, double overallPerformance, string connectionType)
        {
            if (!computers.Any(x => x.Id == computerId))
                throw new ArgumentException(Common.Constants.ExceptionMessages.NotExistingComputerId);

            if (peripherals.Any(x => x.Id == id))
                throw new ArgumentException(Common.Constants.ExceptionMessages.ExistingPeripheralId);

            Peripheral peripheral = null;

            switch (peripheralType)
            {
                case "Headset": peripheral = new Headset(id, manufacturer, model, price, overallPerformance, connectionType); break;
                case "Keyboard": peripheral = new Keyboard(id, manufacturer, model, price, overallPerformance, connectionType); break;
                case "Monitor": peripheral = new Monitor(id, manufacturer, model, price, overallPerformance, connectionType); break;
                case "Mouse": peripheral = new Mouse(id, manufacturer, model, price, overallPerformance, connectionType); break;
                default: throw new ArgumentException(Common.Constants.ExceptionMessages.InvalidPeripheralType);
            }

            var computer = computers.First(x => x.Id == computerId);

            computer.AddPeripheral(peripheral);
            peripherals.Add(peripheral);

            return string.Format(Common.Constants.SuccessMessages.AddedPeripheral, peripheralType, id, computerId);
        }

        public string BuyBest(decimal budget)
        {
            var computer = computers.Where(x => x.Price <= budget).OrderByDescending(y => y.OverallPerformance).FirstOrDefault();

            if (computer == default)
                throw new ArgumentException(string.Format(Common.Constants.ExceptionMessages.CanNotBuyComputer, budget));

            computers.Remove(computer);
            return computer.ToString();
        }

        public string BuyComputer(int id)
        {
            if (!computers.Any(x => x.Id == id))
                throw new ArgumentException(Common.Constants.ExceptionMessages.NotExistingComputerId);

            var computer = computers.First(x => x.Id == id);
            computers.Remove(computer);

            return computer.ToString();
        }

        public string GetComputerData(int id)
        {
            if (!computers.Any(x => x.Id == id))
                throw new ArgumentException(Common.Constants.ExceptionMessages.NotExistingComputerId);

            var computer = computers.First(x => x.Id == id);

            return computer.ToString();
        }

        public string RemoveComponent(string componentType, int computerId)
        {
            if (!computers.Any(x => x.Id == computerId))
                throw new ArgumentException(Common.Constants.ExceptionMessages.NotExistingComputerId);

            var computer = computers.First(x => x.Id == computerId);
            var component = computer.RemoveComponent(componentType);

            components = components.Where(x => x.GetType().Name != componentType).ToList();

            return string.Format(Common.Constants.SuccessMessages.RemovedComponent, componentType, component.Id);
        }

        public string RemovePeripheral(string peripheralType, int computerId)
        {
            if (!computers.Any(x => x.Id == computerId))
                throw new ArgumentException(Common.Constants.ExceptionMessages.NotExistingComputerId);

            var computer = computers.First(x => x.Id == computerId);
            var peripheral = computer.RemovePeripheral(peripheralType);

            peripherals = peripherals.Where(x => x.GetType().Name != peripheralType).ToList();

            return string.Format(Common.Constants.SuccessMessages.RemovedPeripheral, peripheralType, peripheral.Id);
        }
    }
}