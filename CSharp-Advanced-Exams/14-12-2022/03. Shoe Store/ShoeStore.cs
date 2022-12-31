using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoeStore
{
    public class ShoeStore
    {
        private List<Shoe> shoes;
        public string Name { get; set; }
        public int StorageCapacity { get; set; }

        public ShoeStore(string name, int storageCapacity)
        {
            Name = name;
            StorageCapacity = storageCapacity;
            shoes = new List<Shoe>();
        }

        public int Count => shoes.Count;

        public string AddShoe(Shoe shoe)
        {
            if (shoes.Count == StorageCapacity)
                return "No more space in the storage room.";

            shoes.Add(shoe);
            return $"Successfully added {shoe.Type} {shoe.Material} pair of shoes to the store.";
        }

        public int RemoveShoes(string material)
        {
            int originalCount = shoes.Count;
            shoes = shoes.Where(x => x.Material != material).ToList();
            return originalCount - shoes.Count;
        }

        public List<Shoe> GetShoesByType(string type) 
        {
            type = type.ToLower();
            return shoes.Where(x => x.Type.ToLower() == type).ToList();
        }

        public Shoe GetShoeBySize(double size) => shoes.First(x => x.Size == size);

        public string StockList(double size, string type)
        {
            var selectedShoes = shoes.Where(x => x.Size == size && x.Type == type).ToList();
            StringBuilder sb = new StringBuilder();

            if (selectedShoes.Count > 0)
            {
                sb.AppendLine($"Stock list for size {size} - {type} shoes:");

                foreach (var shoe in selectedShoes)
                    sb.AppendLine(shoe.ToString());
            }
            else
                sb.AppendLine("No matches found!");

            return sb.ToString().TrimEnd();
        }
    }
}