using System;

namespace _02._Wall_Destroyer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int size = int.Parse(Console.ReadLine());

            string[,] matrix = new string[size, size];

            int posi = 0;
            int posj = 0;
            int holes = 1;
            int countOfRods = 0;

            for (int i = 0; i < size; i++)
            {
                string s = Console.ReadLine();

                for (int j = 0; j < size; j++)
                {
                    matrix[i, j] = s[j].ToString();

                    if (s[j] == 'V')
                    {
                        posi = i;
                        posj = j;
                    }
                }
            }

            bool isHit = false;

            while (!isHit)
            {
                string input = Console.ReadLine();

                if (input == "End")
                    break;

                int newPosi = posi;
                int newPosj = posj;

                switch (input)
                {
                    case "up": newPosi--; break;
                    case "down": newPosi++; break;
                    case "left": newPosj--; break;
                    case "right": newPosj++; break;
                }

                if (newPosi >= 0 && newPosi < size && newPosj >= 0 && newPosj < size)
                {
                    if (matrix[newPosi, newPosj] == "-")
                    {
                        matrix[posi, posj] = "*";
                        matrix[newPosi, newPosj] = "V";
                        holes++;
                        posi = newPosi;
                        posj = newPosj;
                    }
                    else if (matrix[newPosi, newPosj] == "R")
                    {
                        Console.WriteLine("Vanko hit a rod!");
                        countOfRods++;
                    }
                    else if (matrix[newPosi, newPosj] == "*")
                    {
                        Console.WriteLine($"The wall is already destroyed at position [{newPosi}, {newPosj}]!");
                        matrix[newPosi, newPosj] = "V";
                        matrix[posi, posj] = "*";
                        posi = newPosi;
                        posj = newPosj;
                    }
                    else if (matrix[newPosi, newPosj] == "C")
                    {
                        holes++;
                        isHit = true;
                        matrix[newPosi, newPosj] = "E";
                        matrix[posi, posj] = "*";
                    }
                }
            }

            if (isHit)
                Console.WriteLine($"Vanko got electrocuted, but he managed to make {holes} hole(s).");
            else
                Console.WriteLine($"Vanko managed to make {holes} hole(s) and he hit only {countOfRods} rod(s).");

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                    Console.Write(matrix[i, j]);

                Console.WriteLine();
            }
        }
    }
}