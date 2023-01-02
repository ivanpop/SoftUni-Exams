using System;
using System.Collections.Generic;
using System.Linq;

namespace Renovators
{
    public class Catalog
    {
        private List<Renovator> renovators;
        public string Name { get; set; }
        public int NeededRenovators { get; set; }
        public string Project { get; set; }        

        public Catalog(string name, int neededRenovators, string project)
        {   
            Name = name;
            NeededRenovators = neededRenovators;
            Project = project;
            renovators = new List<Renovator>();
        }
        public int Count => renovators.Count;

        public string AddRenovator(Renovator renovator)
        {
            if(string.IsNullOrEmpty(renovator.Name) || string.IsNullOrEmpty(renovator.Type))
                return "Invalid renovator's information.";

            if (renovators.Count == NeededRenovators)
                return "Renovators are no more needed.";

            if(renovator.Rate > 350)
                return "Invalid renovator's rate.";

            renovators.Add(renovator);
            return $"Successfully added {renovator.Name} to the catalog.";
        }

        public bool RemoveRenovator(string name)
        {
            if (renovators.Any(x => x.Name == name))
            {
                renovators = renovators.Where(x => x.Name != name).ToList();
                return true;
            }

            return false;
        }

        public int RemoveRenovatorBySpecialty(string type)
        {
            if (renovators.Any(x => x.Type == type))
            {
                int oldCount = renovators.Count();
                renovators = renovators.Where(x => x.Type != type).ToList();
                int newCount = renovators.Count();
                return oldCount - newCount;
            }

            return 0;
        }

        public Renovator HireRenovator(string name)
        {
            if(renovators.Any(x => x.Name == name))
            {
                foreach (var renovator in renovators)
                {
                    if(renovator.Name == name)
                    {
                        renovator.Hired = true;
                        return renovator;
                    }
                }
                return null;
            }

            return null;
        }

        public List<Renovator> PayRenovators (int days) => renovators.Where(x => x.Days >= days).ToList();

        public string Report() => $"Renovators available for Project {Project}:" + Environment.NewLine + string.Join(Environment.NewLine, renovators.FindAll(r => !r.Hired));
    }
}