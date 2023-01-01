using System;

namespace _02._Pawn_Wars
{
    internal class Program
    {
        static void Main(string[] args)
        {
            char[,] board = new char[8, 8];

            int bPosI = 0;
            int bPosJ = 0;
            int wPosI = 0;
            int wPosJ = 0;

            for (int i = 0; i < 8; i++)
            {
                string input = Console.ReadLine();

                for (int j = 0; j < 8; j++)
                {
                    board[i, j] = input[j];

                    if (board[i, j] == 'b')
                    {
                        bPosI = i;
                        bPosJ = j;
                    }

                    if (board[i, j] == 'w')
                    {
                        wPosI = i;
                        wPosJ = j;
                    }
                }
            }

            while (true)
            {
                if (wPosI >= 1)
                {
                    if (isValidPos(wPosI - 1, wPosJ - 1) && board[wPosI - 1, wPosJ - 1] == 'b' || isValidPos(wPosI - 1, wPosJ + 1) && board[wPosI - 1, wPosJ + 1] == 'b')
                    {
                        Console.WriteLine($"Game over! White capture on {ConvertToBoard(bPosI, bPosJ)}.");
                        return;
                    }
                    else
                    {
                        board[wPosI, wPosJ] = '-';
                        wPosI--;
                        board[wPosI, wPosJ] = 'w';
                    }
                }

                if (wPosI == 0)
                {
                    Console.WriteLine($"Game over! White pawn is promoted to a queen at {ConvertToBoard(wPosI, wPosJ)}.");
                    return;
                }

                if (bPosI <= 6)
                {
                    if (isValidPos(bPosI + 1, bPosJ - 1) && board[bPosI + 1, bPosJ - 1] == 'w' || isValidPos(bPosI + 1, bPosJ + 1) && board[bPosI + 1, bPosJ + 1] == 'w')
                    {
                        Console.WriteLine($"Game over! Black capture on {ConvertToBoard(wPosI, wPosJ)}.");
                        return;
                    }
                    else
                    {
                        board[bPosI, bPosJ] = '-';
                        bPosI++;
                        board[bPosI, bPosJ] = 'b';
                    }
                }

                if (bPosI == 7)
                {
                    Console.WriteLine($"Game over! Black pawn is promoted to a queen at {ConvertToBoard(bPosI, bPosJ)}.");
                    return;
                }
            }

            bool isValidPos(int posI, int posJ) => posI >= 0 && posI < 8 && posJ >= 0 && posJ < 8;

            string ConvertToBoard(int posI, int posJ)
            {
                char boardPosJ = ' ';

                switch (posJ)
                {
                    case 0: boardPosJ = 'a'; break;
                    case 1: boardPosJ = 'b'; break;
                    case 2: boardPosJ = 'c'; break;
                    case 3: boardPosJ = 'd'; break;
                    case 4: boardPosJ = 'e'; break;
                    case 5: boardPosJ = 'f'; break;
                    case 6: boardPosJ = 'g'; break;
                    case 7: boardPosJ = 'h'; break;
                }

                switch (posI)
                {
                    case 0: posI = 8; break;
                    case 1: posI = 7; break;
                    case 2: posI = 6; break;
                    case 3: posI = 5; break;
                    case 4: posI = 4; break;
                    case 5: posI = 3; break;
                    case 6: posI = 2; break;
                    case 7: posI = 1; break;
                }

                return $"{boardPosJ}{posI}";
            }
        }
    }
}