using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UniversityCompetition.IO.Contracts;

namespace UniversityCompetition.IO
{
    public class TextWriter : IWriter
    {
        public TextWriter()
        {
            using (StreamWriter writer = new StreamWriter("../../../output.txt", false))
            {
                writer.Write("");
            }
        }

        public void Write(string message)
        {
            using (StreamWriter writer = new StreamWriter("../../../output.txt", true))
            {
                writer.Write(message);
            }
        }

        public void WriteLine(string message)
        {
            using (StreamWriter writer = new StreamWriter("../../../output.txt", true))
            {
                writer.WriteLine(message);
            }
        }
    }
}
