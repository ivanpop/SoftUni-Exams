using System;

public class StartUp
{
    public static void Main()
    {
        int n = int.Parse(Console.ReadLine());
        string racingNumber = Console.ReadLine().Trim();
        string[,] mat = new string[n, n];

        int t1PosI = -1;
        int t2PosI = 0;
        int t1PosJ = 0;
        int t2PosJ = 0;
        int posI = 0;
        int posJ = 0;
        int newPosI = 0;
        int newPosJ = 0;
        int distance = 0;
        bool finished = false;

        for (int i = 0; i < n; i++)
        {
            string[] inp = Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries);

            for (int j = 0; j < n; j++)
            {
                mat[i, j] = inp[j].ToString();

                if (mat[i, j] == "T")
                    if (t1PosI == -1)
                    {
                        t1PosI = i;
                        t1PosJ = j;
                    }
                    else
                    {
                        t2PosI = i;
                        t2PosJ = j;
                    }
            }
        }

        mat[posI, posJ] = "C";

        while (true)
        {
            string input = Console.ReadLine();

            if (input == "End")
                break;

            newPosI = posI;
            newPosJ = posJ;

            switch (input)
            {
                case "up": newPosI--; break;
                case "down": newPosI++; break;
                case "left": newPosJ--; break;
                case "right": newPosJ++; break;
            }

            if (posI >= 0 && posI < n && posJ >= 0 && posJ < n)
            {
                if (mat[newPosI, newPosJ] == ".")
                {
                    distance += 10;
                    mat[newPosI, newPosJ] = "C";
                    mat[posI, posJ] = ".";
                    posI = newPosI;
                    posJ = newPosJ;
                }
                else if (mat[newPosI, newPosJ] == "T")
                {
                    distance += 30;
                    mat[posI, posJ] = ".";
                    mat[t1PosI, t1PosJ] = ".";
                    mat[t2PosI, t2PosJ] = ".";

                    if (newPosI == t1PosI && newPosJ == t1PosJ)
                    {
                        posI = t2PosI;
                        posJ = t2PosJ;
                    }
                    else
                    {
                        posI = t1PosI;
                        posJ = t1PosJ;
                    }

                    mat[posI, posJ] = "C";
                }
                else
                {
                    distance += 10;
                    mat[posI, posJ] = ".";
                    mat[newPosI, newPosJ] = "C";
                    finished = true;
                    break;
                }
            }
        }

        if (finished)
            Console.WriteLine($"Racing car {racingNumber} finished the stage!");
        else
            Console.WriteLine($"Racing car {racingNumber} DNF.");

        Console.WriteLine($"Distance covered {distance} km.");

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
                Console.Write(mat[i, j]);
            Console.WriteLine();
        }
    }
}