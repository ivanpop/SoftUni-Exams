using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComputerArchitecture
{
    public class Computer
    {
        public Computer(string model, int capacity)
        {
            Model = model;
            Capacity = capacity;
            Multiprocessor = new List<CPU>();
        }

        public List<CPU> Multiprocessor { get; set; }
        public string Model  { get; set; }
        public int Capacity { get; set; }
        public int Count => Multiprocessor.Count;

        public void Add(CPU cpu)
        {
            if (Capacity > 0)
            {
                Multiprocessor.Add(cpu);
                Capacity--;
            }
        }

        public bool Remove(string brand)
        {
            var selectedCPU = Multiprocessor.FirstOrDefault(x => x.Brand == brand);

            if (selectedCPU != default)
            {
                Multiprocessor.Remove(selectedCPU);
                return true;
            }

            return false;
        }

        public CPU MostPowerful() => Multiprocessor.OrderByDescending(x => x.Frequency).FirstOrDefault();

        public CPU GetCPU(string brand) => Multiprocessor.Any(x => x.Brand == brand) ? Multiprocessor.Where(x => x.Brand == brand).FirstOrDefault() : null;

        public string Report()
        {
            StringBuilder sb = new StringBuilder($"CPUs in the Computer {Model}:" + Environment.NewLine);

            foreach (var item in Multiprocessor)
                sb.Append(item.ToString().TrimEnd() + Environment.NewLine);

            return sb.ToString().TrimEnd();
        }
    }
}