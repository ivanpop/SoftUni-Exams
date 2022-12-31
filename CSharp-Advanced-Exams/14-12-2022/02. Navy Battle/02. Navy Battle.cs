using System;

namespace _02._Navy_Battle
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int size = int.Parse(Console.ReadLine());
            string[,] arr = new string[size, size];

            int sPosI = 0;
            int sPosJ = 0;
            int destroyedShips = 0;
            int landedMines = 0;

            for (int i = 0; i < size; i++)
            {
                string input = Console.ReadLine();

                for (int j = 0; j < size; j++)
                {
                    arr[i, j] = input[j].ToString();

                    if (arr[i, j] == "S")
                    {
                        sPosI = i;
                        sPosJ = j;
                    }
                }
            }

            while (destroyedShips < 3 && landedMines < 3)
            {
                string input = Console.ReadLine();
                arr[sPosI, sPosJ] = "-";

                switch (input)
                {
                    case "up": 
                        sPosI--;
                        CheckNewPosition(sPosI, sPosJ);
                        break;

                    case "down":
                        sPosI++;
                        CheckNewPosition(sPosI, sPosJ);
                        break;

                    case "left":
                        sPosJ--;
                        CheckNewPosition(sPosI, sPosJ);
                        break;

                    case "right":
                        sPosJ++;
                        CheckNewPosition(sPosI, sPosJ);
                        break;
                }

                arr[sPosI, sPosJ] = "S";
            }

            if (destroyedShips == 3)
                Console.WriteLine("Mission accomplished, U-9 has destroyed all battle cruisers of the enemy!");
            if (landedMines == 3)
                Console.WriteLine($"Mission failed, U-9 disappeared! Last known coordinates [{sPosI}, {sPosJ}]!");

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                    Console.Write(arr[i, j]);

                Console.WriteLine();
            }

            void CheckNewPosition(int i, int j)
            {
                if (arr[i, j] == "*")
                    landedMines++;

                if (arr[i, j] == "C")
                    destroyedShips++;
            }
        }
    }
}