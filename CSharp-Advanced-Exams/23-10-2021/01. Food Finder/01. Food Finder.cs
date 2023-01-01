using System;
using System.Collections.Generic;

namespace _01._Food_Finder
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, HashSet<char>> words = new Dictionary<string, HashSet<char>>()
            {
                {"pear", new HashSet<char>()},
                {"flour", new HashSet<char>()},
                {"pork", new HashSet<char>()},
                {"olive", new HashSet<char>()},
            };

            Queue<char> vowels = new Queue<char>(string.Join("", Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries)));
            Stack<char> consonants = new Stack<char>(string.Join("", Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries)));

            while (consonants.Count > 0)
            {
                char vowel = vowels.Dequeue();
                char consonant = consonants.Pop();

                foreach (var word in words)
                {
                    if (word.Key.Contains(vowel))
                        word.Value.Add(vowel);

                    if (word.Key.Contains(consonant))
                        word.Value.Add(consonant);
                }

                vowels.Enqueue(vowel);
            }

            List<string> foundWords = new List<string>();

            foreach (var word in words)
                if (word.Key.Length == word.Value.Count)
                    foundWords.Add(word.Key);

            Console.WriteLine($"Words found: {foundWords.Count}");
            Console.WriteLine(string.Join(Environment.NewLine, foundWords));
        }
    }
}