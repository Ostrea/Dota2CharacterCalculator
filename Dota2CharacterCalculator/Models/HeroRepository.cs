using System.Collections.Generic;
using System.Data.Entity;

namespace Dota2CharacterCalculator.Models
{
    public class HeroRepository
    {
        public void Save(IEnumerable<Hero> heroes)
        {
            using (var heroContext = new HeroContext())
            {
                foreach (var hero in heroes)
                {
                    heroContext.Heroes.Add(hero);
                }
                heroContext.SaveChanges();
            }
        }

        public IEnumerable<ViewModels.Hero> GetHeroes()
        {
//            TODO convert to view model.
            return null;
        }

        public void DeleteAllHeroes()
        {
            using (var heroContext = new HeroContext())
            {
                heroContext.Heroes.RemoveRange(heroContext.Heroes);
                heroContext.SaveChanges();
            }
        }
    }

    public class HeroContext : DbContext
    {
        public DbSet<Hero> Heroes { get; set; }
    }
}