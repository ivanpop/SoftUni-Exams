using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace _02._Beaver_at_Work
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int n = int.Parse(Console.ReadLine());
            string[,] pond = new string[n, n];

            int posI = 0;
            int posJ = 0;
            int newPosI = 0;
            int newPosJ = 0;
            int branchesLeft = 0;
            Regex regex = new Regex(@"[a-z]");
            List<string> collected = new List<string>();
            bool isF = false;

            for (int i = 0; i < n; i++)
            {
                string[] _input = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);

                for (int j = 0; j < n; j++)
                {
                    pond[i, j] = _input[j].ToString();

                    if (pond[i, j] == "B")
                    {
                        posI = i;
                        posJ = j;
                    }

                    if (regex.IsMatch(pond[i, j]))
                        branchesLeft++;
                }
            }

            string input = null;

            while (true)
            {
                if (!isF)
                {
                    input = Console.ReadLine();

                    if (input == "end")
                        break;
                }

                newPosI = posI;
                newPosJ = posJ;

                if (!isF)
                    switch (input)
                    {
                        case "up": newPosI--; break;
                        case "down": newPosI++; break;
                        case "left": newPosJ--; break;
                        case "right": newPosJ++; break;
                    }

                isF = true ? false : true;

                if (isValid(newPosI, newPosJ))
                {
                    if (pond[newPosI, newPosJ] == "-")
                    {
                        pond[posI, posJ] = "-";
                        pond[newPosI, newPosJ] = "B";
                        posI = newPosI;
                        posJ = newPosJ;
                    }
                    else if (regex.IsMatch(pond[newPosI, newPosJ]))
                    {
                        collected.Add(pond[newPosI, newPosJ]);
                        branchesLeft--;
                        pond[posI, posJ] = "-";
                        pond[newPosI, newPosJ] = "B";
                        posI = newPosI;
                        posJ = newPosJ;
                    }
                    else
                    {
                        pond[posI, posJ] = "-";
                        pond[newPosI, newPosJ] = "-";
                        posI = newPosI;
                        posJ = newPosJ;

                        if (input == "up")
                            if (posI == 0)
                                posI = n - 1;
                            else
                                posI = 0;

                        else if (input == "down")
                            if (posI == n - 1)
                                posI = 0;
                            else
                                posI = n - 1;

                        else if (input == "left")
                            if (posJ == 0)
                                posJ = n - 1;
                            else
                                posJ = 0;
                        else
                            if (posJ == n - 1)
                                posJ = 0;
                            else
                                posJ = n - 1;

                        isF = true;
                    }

                    if (branchesLeft == 0)
                        break;
                }
                else
                    if (collected.Count > 0)
                        collected.RemoveAt(collected.Count - 1);
            }

            if (branchesLeft == 0)
                Console.WriteLine($"The Beaver successfully collect {collected.Count} wood branches: {string.Join(", ", collected)}.");
            else
                Console.WriteLine($"The Beaver failed to collect every wood branch. There are {branchesLeft} branches left.");

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                    Console.Write(pond[i, j] + " ");

                Console.WriteLine();
            }

            bool isValid(int posI, int posJ) => posI >= 0 && posI < n && posJ >= 0 && posJ < n;
        }
    }
}