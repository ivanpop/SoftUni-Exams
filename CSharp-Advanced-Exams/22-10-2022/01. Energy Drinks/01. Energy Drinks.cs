using System;
using System.Collections.Generic;
using System.Linq;

public class StartUp
{
    public static void Main()
    {
        Stack<int> caffeines = new Stack<int>(Console.ReadLine().Split(", ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray());
        Queue<int> drinks = new Queue<int>(Console.ReadLine().Split(", ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray());

        int currentCaffeine = 0;

        while (caffeines.Count > 0 && drinks.Count > 0)
        {
            int caffeine = caffeines.Pop();
            int drink = drinks.Dequeue();

            int sum = caffeine * drink;

            if (currentCaffeine + sum <= 300)
                currentCaffeine += sum;
            else
            {
                drinks.Enqueue(drink);
                currentCaffeine -= 30;

                if (currentCaffeine < 0)
                    currentCaffeine = 0;
            }
        }

        if (drinks.Count > 0)
            Console.WriteLine($"Drinks left: {string.Join(", ", drinks)}");
        else
            Console.WriteLine("At least Stamat wasn't exceeding the maximum caffeine.");

        Console.WriteLine($"Stamat is going to sleep with {currentCaffeine} mg caffeine.");
    }
}