using System.Windows.Media.Imaging;

namespace Dota2CharacterCalculator.Models
{
    public class Hero
    {
        public string Name { get; }
        public BitmapImage Icon { get; }

        public Hero(string name, BitmapImage icon)
        {
            Name = name;
            Icon = icon;
        }
    }
}