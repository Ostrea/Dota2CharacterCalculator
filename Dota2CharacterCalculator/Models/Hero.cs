using System.Data.Entity;

namespace Dota2CharacterCalculator.Models
{
    public class Hero
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class HeroContext : DbContext
    {
        public DbSet<Hero> Heroes { get; set; }
    }
}