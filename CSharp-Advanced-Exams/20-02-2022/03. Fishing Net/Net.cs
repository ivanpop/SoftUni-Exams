using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FishingNet
{
    public class Net
    {
        private List<Fish> fish;

        public string Material { get; set; }
        public int Capacity { get; set; }

        public Net(string material, int capacity)
        {
            Material = material;
            Capacity = capacity;
            fish = new List<Fish>();
        }

        public IReadOnlyCollection<Fish> Fish => fish.AsReadOnly();

        public int Count => fish.Count;

        public string AddFish(Fish fish)
        {
            if (string.IsNullOrWhiteSpace(fish.FishType) || fish.Length <= 0 || fish.Weight <= 0)
                return "Invalid fish.";

            if (Count == Capacity)
                return "Fishing net is full.";

            this.fish.Add(fish);

            return $"Successfully added {fish.FishType} to the fishing net.";
        }

        public bool ReleaseFish(double weight)
        {
            var selectedFish = fish.FirstOrDefault(x => x.Weight == weight);

            if (selectedFish == null) 
                return false;

            fish.Remove(selectedFish);
            return true;
        }

        public Fish GetFish(string fishType) => fish.First(x => x.FishType == fishType);

        public Fish GetBiggestFish() => fish.OrderByDescending(x => x.Length).First();

        public string Report()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Into the {Material}:");

            foreach (var fish in Fish.OrderByDescending(x => x.Length))
                sb.AppendLine(fish.ToString());

            return sb.ToString().TrimEnd();
        }
    }
}