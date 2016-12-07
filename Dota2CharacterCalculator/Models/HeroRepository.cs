using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Windows.Media.Imaging;
using Dota2CharacterCalculator.ViewModels;
using Attribute = Dota2CharacterCalculator.ViewModels.Attribute;

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
            var attackDamageIcon = LoadIcon("AttackDamage", IconType.Stats);
            var armorIcon = LoadIcon("Armor", IconType.Stats);
            var msIcon = LoadIcon("MovementSpeed", IconType.Stats);
            var healthIcon = LoadIcon("Health", IconType.Stats);
            var manaIcon = LoadIcon("Mana", IconType.Stats);

            var strengthIcon = LoadIcon("Strength", IconType.Stats);
            var agilityIcon = LoadIcon("Agility", IconType.Stats);
            var intelligenceIcon = LoadIcon("Intelligence", IconType.Stats);

            var heroes = new List<ViewModels.Hero>();

            using (var heroContext = new HeroContext())
            {
                foreach (var heroFromDb in heroContext.Heroes)
                {
                    heroes.Add(new ViewModels.Hero
                        (
                            heroFromDb.Name,
                            ToImage(heroFromDb.Icon),
                            new AttackDamage
                            (
                                heroFromDb.BaseMinAttackDamage,
                                heroFromDb.BaseMaxAttackDamage,
                                attackDamageIcon
                            ),
                            new Armor(heroFromDb.BaseArmor, armorIcon),
                            new MovementSpeed(heroFromDb.BaseMovementSpeed, msIcon),
                            new Tuple<Attribute, Attribute, Attribute>
                            (
                                new Attribute
                                (
                                    AttributeType.Strength,
                                    heroFromDb.BaseStrength,
                                    heroFromDb.StrengthGrowth,
                                    strengthIcon
                                ),
                                new Attribute
                                (
                                    AttributeType.Agility,
                                    heroFromDb.BaseAgility,
                                    heroFromDb.AgilityGrowth,
                                    agilityIcon
                                ),
                                new Attribute
                                (
                                    AttributeType.Intelligence,
                                    heroFromDb.BaseIntelligence,
                                    heroFromDb.IntelligenceGrowth,
                                    intelligenceIcon
                                )
                            ),
                            1,
                            (AttributeType) Enum.Parse(typeof(AttributeType), heroFromDb.PrimaryAttribute),
                            new Health(healthIcon),
                            new Mana(manaIcon)
                        )
                    );
                }
            }

            return heroes;
        }

        private static BitmapImage LoadIcon(string iconName, IconType iconType)
        {
            return new BitmapImage(new Uri($"Assets/{iconType}/{iconName}.png", UriKind.Relative));
        }

        private enum IconType
        {
            Heroes,
            Stats,
            Items,
        }

        private static BitmapImage ToImage(byte[] imageBytes)
        {
            using (var memoryStream = new System.IO.MemoryStream(imageBytes))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource =  memoryStream;
                image.EndInit();
                return image;
            }
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