using System;
using System.Collections.Generic;
using System.Linq;

namespace _01._Bakery_Shop
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, int> list = new Dictionary<string, int>();
            list.Add("Croissant", 0);
            list.Add("Muffin", 0);
            list.Add("Baguette", 0);
            list.Add("Bagel", 0);

            Queue<double> waters = new Queue<double>(Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(double.Parse));
            Stack<double> flowers = new Stack<double>(Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(double.Parse));

            while (waters.Count > 0 && flowers.Count > 0)
            {
                double water = waters.Dequeue();
                double flower = flowers.Pop();

                double waterRatio = (water * 100) / (water + flower);

                switch (waterRatio)
                {
                    case 50: list["Croissant"]++; break;
                    case 40: list["Muffin"]++; break;
                    case 30: list["Baguette"]++; break;
                    case 20: list["Bagel"]++; break;

                    default:
                        flower -= water;
                        list["Croissant"]++;
                        flowers.Push(flower);
                        break;
                }
            }

            list = list.Where(x => x.Value > 0).OrderByDescending(x => x.Value).ThenBy(x => x.Key).ToDictionary(x => x.Key, y => y.Value);

            foreach (var item in list)
                Console.WriteLine($"{item.Key}: {item.Value}");

            if (waters.Count == 0)
                Console.WriteLine("Water left: None");
            else
                Console.WriteLine($"Water left: " + String.Join(", ", waters));

            if (flowers.Count == 0)
                Console.WriteLine("Flour left: None");
            else
                Console.WriteLine($"Flour left: " + String.Join(", ", flowers));
        }
    }
}