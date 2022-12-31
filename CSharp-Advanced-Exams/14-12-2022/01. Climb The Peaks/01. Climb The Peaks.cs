using System;
using System.Collections.Generic;
using System.Linq;

namespace _01._Climb_The_Peaks
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var portions = new Stack<int>(Console.ReadLine().Split(", ").Select(int.Parse).ToArray());
            var stamina = new Queue<int>(Console.ReadLine().Split(", ").Select(int.Parse).ToArray());

            Dictionary<string, int> peaks = new Dictionary<string, int>();
            peaks.Add("Vihren", 80);
            peaks.Add("Kutelo", 90);
            peaks.Add("Banski Suhodol", 100);
            peaks.Add("Polezhan", 60);
            peaks.Add("Kamenitza", 70);

            List<string> conqueredPeaks = new List<string>();

            while (portions.Count > 0 && stamina.Count > 0)
            {
                if (peaks.Count == 0)
                    break;

                int sum = portions.Pop() + stamina.Dequeue();

                if (sum >= peaks.First().Value)
                {
                    conqueredPeaks.Add(peaks.First().Key);
                    peaks.Remove(peaks.First().Key);
                }
            }

            if (conqueredPeaks.Count == 5)
                Console.WriteLine("Alex did it! He climbed all top five Pirin peaks in one week -> @FIVEinAWEEK");
            else
                Console.WriteLine("Alex failed! He has to organize his journey better next time -> @PIRINWINS");

            if (conqueredPeaks.Count > 0)
            {
                Console.WriteLine("Conquered peaks:");
                foreach (var peak in conqueredPeaks)
                    Console.WriteLine(peak);
            }
        }
    }
}