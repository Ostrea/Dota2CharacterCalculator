using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace Dota2CharacterCalculator.Models
{
    public class Item
    {
        public string Name { get; }
        public BitmapImage Icon { get; }
        public double? MovementSpeedBonus { get; set; }
        public int? AttackDamageBonus { get; set; }
        public double? StrengthBonus { get; set; }
        public double? AgilityBonus { get; set; }
        public double? IntelligenceBonus { get; set; }
        public List<string> AdditionalActiveProperties { get; set; } = new List<string>();
        public List<string> AdditionalPassiveProperties { get; set; } = new List<string>();

        public Item(string name, BitmapImage icon)
        {
            Name = name;
            Icon = icon;
        }
    }
}