namespace Dota2CharacterCalculator.Models
{
    public class Hero
    {
        public int Id { get; set; }

        public byte[] Icon { get; set; }
        public string Name { get; set; }

        public string PrimaryAttribute { get; set; }

        public int BaseStrength { get; set; }
        public double StrengthGrowth { get; set; }

        public int BaseAgility { get; set; }
        public double AgilityGrowth { get; set; }

        public int BaseIntelligence { get; set; }
        public double IntelligenceGrowth { get; set; }

        public int BaseMovementSpeed { get; set; }

        public double BaseArmor { get; set; }

        public int BaseMinAttackDamage { get; set; }
        public int BaseMaxAttackDamage { get; set; }
    }
}