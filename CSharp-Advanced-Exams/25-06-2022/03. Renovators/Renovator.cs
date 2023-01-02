using System;

namespace Renovators
{
    public class Renovator
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public double Rate { get; set; }
        public int Days { get; set; }
        public bool Hired { get; set; }

        public Renovator(string name, string type, double rate, int days)
        {
            Name = name;
            Type = type;
            Rate = rate;
            Days = days;
            Hired = false;
        }

        public override string ToString() => $"-Renovator: {Name}{Environment.NewLine}--Specialty: {Type}{Environment.NewLine}--Rate per day: {Rate} BGN";
    }
}