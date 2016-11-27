using System.Windows.Media.Imaging;

namespace Dota2CharacterCalculator.Models
{
    public abstract class BaseModel
    {
        public BitmapImage Icon { get; }

        protected BaseModel(BitmapImage icon)
        {
            Icon = icon;
        }
    }

    public class Hero : BaseModel
    {
        public string Name { get; }
        public AttackDamage Damage { get; }

        public Hero(string name, BitmapImage icon, AttackDamage damage) : base(icon)
        {
            Name = name;
            Damage = damage;
        }
    }

    public class AttackDamage : BaseModel
    {
        public int Min { get; }
        public int Max { get; }
        public int Average => (Min + Max) / 2;


        public AttackDamage(int min, int max, BitmapImage icon) : base(icon)
        {
            Min = min;
            Max = max;
        }
    }
}