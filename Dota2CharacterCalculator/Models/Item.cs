using System.Windows.Media.Imaging;

namespace Dota2CharacterCalculator.Models
{
    public class Item
    {
        public string Name { get; }
        public BitmapImage Icon { get; }

        public Item(string name, BitmapImage icon)
        {
            Name = name;
            Icon = icon;
        }
    }
}