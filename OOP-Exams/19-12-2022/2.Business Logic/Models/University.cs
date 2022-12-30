using System;
using System.Collections.Generic;
using System.Text;
using UniversityCompetition.Models.Contracts;

namespace UniversityCompetition.Models
{
    public class University : IUniversity
    {
        private int id;
        private string name;
        private string category;
        private int capacity;
        private ICollection<int> requiredSubjects;

        public University(int universityId, string universityName, string category, int capacity, ICollection<int> requiredSubjects)
        {
            Id= universityId;
            Name= universityName;
            Category= category;
            Capacity= capacity;
            this.requiredSubjects = requiredSubjects;
        }

        public int Capacity 
        {
            get { return capacity; }
            private set {
                
                if (value < 0)
                    throw new ArgumentException(Utilities.Messages.ExceptionMessages.CapacityNegative);

                capacity = value; }
        }
        public string Category
        {
            get { return category; }
            private set {

                if (value != "Technical" && value != "Economical" && value != "Humanity")
                    throw new ArgumentException(string.Format(Utilities.Messages.ExceptionMessages.CategoryNotAllowed, value));

                category = value;
            }
        }
        public string Name
        {
            get { return name; }
            private set 
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException(Utilities.Messages.ExceptionMessages.NameNullOrWhitespace);

                name = value;
            }
        }
        public int Id
        {
            get { return id; }
            private set { id = value; }
        }
        public IReadOnlyCollection<int> RequiredSubjects => (IReadOnlyCollection<int>)requiredSubjects;

    }
}