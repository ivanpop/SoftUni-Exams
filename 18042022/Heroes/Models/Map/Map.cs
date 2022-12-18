using Heroes.Models.Contracts;
using Heroes.Models.Heroes;
using System.Collections.Generic;
using System.Linq;

namespace Heroes.Models.Map
{
    public class Map : IMap
    {
        private List<IHero> knights = new List<IHero>();
        private List<IHero> barbarians = new List<IHero>();
        bool knightsDead = false;
        bool barbariansDead = false;

        public string Fight(ICollection<IHero> players)
        {
            knights = players.Where(x => x is Knight).ToList();
            barbarians = players.Where(x => x is Barbarian).ToList();

            while (true)
            {
                foreach (var attacker in knights.Where(x => x.IsAlive))
                {
                    foreach (var defender in barbarians.Where(x => x.IsAlive))
                    {
                        defender.TakeDamage(attacker.Weapon.DoDamage());
                        if (CheckSomeTeamDied()) break;
                    }

                    if (CheckSomeTeamDied()) break;
                }

                if (CheckSomeTeamDied()) break;

                foreach (var attacker in barbarians.Where(x => x.IsAlive))
                {
                    foreach (var defender in knights.Where(x => x.IsAlive))
                    {
                        defender.TakeDamage(attacker.Weapon.DoDamage());
                        if (CheckSomeTeamDied()) break;
                    }

                    if (CheckSomeTeamDied()) break;
                }

                if (CheckSomeTeamDied()) break;
            }

            if (barbariansDead)
                 return $"The knights took {knights.Where(x => !x.IsAlive).Count()} casualties but won the battle.";
            else
                return $"The barbarians took {barbarians.Where(x => !x.IsAlive).Count()} casualties but won the battle.";

        }

        private bool CheckSomeTeamDied()
        {
            foreach (var hero in knights)
            {
                if (hero.Health > 0)
                {
                    knightsDead = false;
                    break;
                }                    

                knightsDead = true;
            }

            foreach (var hero in barbarians)
            {
                if (hero.Health > 0)
                {
                    barbariansDead = false;
                    break;
                }                    

                barbariansDead = true;
            }

            if (knightsDead || barbariansDead)
                return true;

            return false;
        }
    }
}