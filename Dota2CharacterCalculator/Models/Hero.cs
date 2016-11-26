using System.Windows.Media.Imaging;

namespace Dota2CharacterCalculator.Models
{
    public class Hero
    {
        public string Name { get; }
        public AttackDamage Damage { get; }

        public BitmapImage Icon { get; }

        public Hero(string name, BitmapImage icon, AttackDamage damage)
        {
            Name = name;
            Icon = icon;
            Damage = damage;
        }
    }

    public class AttackDamage
    {
        public int Min { get; }
        public int Max { get; }
        public int Average => (Min + Max) / 2;

        public BitmapImage Icon { get; }

        public AttackDamage(int min, int max, BitmapImage icon)
        {
            Min = min;
            Max = max;
            Icon = icon;
        }
    }
}