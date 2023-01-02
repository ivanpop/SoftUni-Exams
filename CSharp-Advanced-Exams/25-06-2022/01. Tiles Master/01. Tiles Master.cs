using System;
using System.Collections.Generic;
using System.Linq;

namespace _01._Tiles_Master
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Stack<double> whites = new Stack<double>(Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(double.Parse).ToArray());
            Queue<double> greys = new Queue<double>(Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(double.Parse).ToArray());
            Dictionary<string, int> locations = new Dictionary<string, int>();

            while (whites.Count > 0 && greys.Count > 0)
            {
                double sum = whites.Peek() + greys.Peek();

                if (whites.Peek() == greys.Peek())
                {
                    if (sum == 40)
                    {
                        if (!locations.ContainsKey("Sink"))
                            locations.Add("Sink", 0);

                        locations["Sink"]++;
                    }
                    else if (sum == 50)
                    {
                        if (!locations.ContainsKey("Oven"))
                            locations.Add("Oven", 0);

                        locations["Oven"]++;
                    }
                    else if (sum == 60)
                    {
                        if (!locations.ContainsKey("Countertop"))
                            locations.Add("Countertop", 0);

                        locations["Countertop"]++;
                    }
                    else if (sum == 70)
                    {
                        if (!locations.ContainsKey("Wall"))
                            locations.Add("Wall", 0);

                        locations["Wall"]++;
                    }
                    else
                    {
                        if (!locations.ContainsKey("Floor"))
                            locations.Add("Floor", 0);

                        locations["Floor"]++;
                    }

                    whites.Pop();
                    greys.Dequeue();
                }
                else
                {
                    double temp = whites.Pop() / 2.0;
                    whites.Push(temp);

                    temp = greys.Dequeue();
                    greys.Enqueue(temp);
                }
            }

            if (whites.Count > 0)
                Console.WriteLine($"White tiles left: " + String.Join(", ", whites));
            else
                Console.WriteLine("White tiles left: none");

            if (greys.Count > 0)
                Console.WriteLine($"Grey tiles left: " + String.Join(", ", greys));
            else
                Console.WriteLine("Grey tiles left: none");

            locations = locations.OrderByDescending(x => x.Value).ThenBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);

            foreach (var location in locations)
                Console.WriteLine($"{location.Key}: {location.Value}");
        }
    }
}