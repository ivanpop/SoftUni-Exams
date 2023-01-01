using System;
using System.Collections.Generic;
using System.Linq;

namespace _01._Barista_Contest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, int> coffees = new Dictionary<string, int>();
            Queue<int> coffeeBag = new Queue<int>();
            Stack<int> milkBag = new Stack<int>();

            int[] input = Console.ReadLine().Split(", ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

            foreach (var item in input)
                coffeeBag.Enqueue(item);

            input = Console.ReadLine().Split(", ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

            foreach (var item in input)
                milkBag.Push(item);

            while (coffeeBag.Count != 0 && milkBag.Count != 0)
            {
                int coffee = coffeeBag.Dequeue();
                int milk = milkBag.Pop();

                if (coffee + milk == 200)
                {
                    if (!coffees.ContainsKey("Latte"))
                        coffees.Add("Latte", 0);

                    coffees["Latte"]++;
                }
                else if (coffee + milk == 150)
                {
                    if (!coffees.ContainsKey("Americano"))
                        coffees.Add("Americano", 0);

                    coffees["Americano"]++;
                }
                else if (coffee + milk == 100)
                {
                    if (!coffees.ContainsKey("Capuccino"))
                        coffees.Add("Capuccino", 0);

                    coffees["Capuccino"]++;
                }
                else if (coffee + milk == 75)
                {
                    if (!coffees.ContainsKey("Espresso"))
                        coffees.Add("Espresso", 0);

                    coffees["Espresso"]++;
                }
                else if (coffee + milk == 50)
                {
                    if (!coffees.ContainsKey("Cortado"))
                        coffees.Add("Cortado", 0);

                    coffees["Cortado"]++;
                }
                else
                    milkBag.Push(milk - 5);
            }

            if (coffeeBag.Count == 0 && milkBag.Count == 0)
                Console.WriteLine("Nina is going to win! She used all the coffee and milk!");
            else
                Console.WriteLine("Nina needs to exercise more! She didn't use all the coffee and milk!");

            if (coffeeBag.Count == 0)
                Console.WriteLine("Coffee left: none");
            else
                Console.WriteLine($"Coffee left: " + String.Join(", ", coffeeBag));

            if (milkBag.Count == 0)
                Console.WriteLine("Milk left: none");
            else
                Console.WriteLine($"Milk left: " + String.Join(", ", milkBag));

            coffees = coffees.OrderBy(x => x.Value).ThenByDescending(x => x.Key).ToDictionary(x => x.Key, y => y.Value);

            foreach (var coffee in coffees)
                Console.WriteLine($"{coffee.Key}: {coffee.Value}");
        }
    }    
}