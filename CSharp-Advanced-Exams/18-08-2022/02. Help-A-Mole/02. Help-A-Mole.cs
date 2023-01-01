using System;

namespace _02._Help_A_Mole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int n = int.Parse(Console.ReadLine());
            string[,] field = new string[n, n];

            int posI = 0;
            int posJ = 0;
            int s1PosI = -1;
            int s1PosJ = 0;
            int s2PosI = 0;
            int s2PosJ = 0;
            int points = 0;

            for (int i = 0; i < n; i++)
            {
                string input = Console.ReadLine();

                for (int j = 0; j < n; j++)
                {
                    field[i, j] = input[j].ToString();

                    if (field[i, j] == "M")
                    {
                        posI = i;
                        posJ = j;
                    }

                    if (field[i, j] == "S")
                    {
                        if (s1PosI == -1)
                        {
                            s1PosI = i;
                            s1PosJ = j;
                        }
                        else
                        {
                            s2PosI = i;
                            s2PosJ = j;
                        }

                    }
                }
            }

            while (points < 25)
            {
                string input = Console.ReadLine();

                if (input == "End")
                    break;

                int newPosI = posI;
                int newPosJ = posJ;

                switch (input)
                {
                    case "up": newPosI--; break;
                    case "down": newPosI++; break;
                    case "left": newPosJ--; break;
                    case "right": newPosJ++; break;
                }

                if (newPosI >= 0 && newPosI < n && newPosJ >= 0 && newPosJ < n)
                {
                    if (field[newPosI, newPosJ] == "-")
                    {
                        field[posI, posJ] = "-";
                        field[newPosI, newPosJ] = "M";
                        posI = newPosI;
                        posJ = newPosJ;
                    }
                    else if (int.TryParse(field[newPosI, newPosJ], out int result))
                    {
                        field[posI, posJ] = "-";
                        field[newPosI, newPosJ] = "M";
                        posI = newPosI;
                        posJ = newPosJ;
                        points += result;
                    }
                    else
                    {
                        field[posI, posJ] = "-";
                        field[newPosI, newPosJ] = "-";
                        points -= 3;

                        if (newPosI == s1PosI && newPosJ == s1PosJ)
                        {
                            posI = s2PosI;
                            posJ = s2PosJ;
                        }
                        else
                        {
                            posI = s1PosI;
                            posJ = s1PosJ;
                        }

                        field[posI, posJ] = "M";
                    }
                }
                else
                    Console.WriteLine("Don't try to escape the playing field!");
            }

            if (points >= 25)
            {
                Console.WriteLine("Yay! The Mole survived another game!");
                Console.WriteLine($"The Mole managed to survive with a total of {points} points.");
            }
            else
            {
                Console.WriteLine("Too bad! The Mole lost this battle!");
                Console.WriteLine($"The Mole lost the game with a total of {points} points.");
            }

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                    Console.Write(field[i, j]);

                Console.WriteLine();
            }
        }
    }
}