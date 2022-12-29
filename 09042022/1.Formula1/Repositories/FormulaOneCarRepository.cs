using Formula1.Models.Contracts;
using Formula1.Repositories.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace Formula1.Repositories
{
    public class FormulaOneCarRepository : IRepository<IFormulaOneCar>
    {
        private List<IFormulaOneCar> formulaOneCars;

        public FormulaOneCarRepository()
        {
            formulaOneCars = new List<IFormulaOneCar>();
        }

        public IReadOnlyCollection<IFormulaOneCar> Models => formulaOneCars;
        public void Add(IFormulaOneCar model)
        {
            formulaOneCars.Add(model);
        }
        public bool Remove(IFormulaOneCar model) => formulaOneCars.Remove(model);
        public IFormulaOneCar FindByName(string name) => formulaOneCars.FirstOrDefault(x => x.GetType().Name == name);
    }
}