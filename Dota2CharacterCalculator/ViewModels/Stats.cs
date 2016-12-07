using System;
using System.ComponentModel;
using System.Windows.Media.Imaging;

namespace Dota2CharacterCalculator.ViewModels
{
    public class AttackDamage : BaseModel
    {
        public int BaseMin { get; }
        public int BaseMax { get; }

        public int MainMax { get; private set; }
        public int MainMin { get; private set; }

        public int Average => (MainMin + MainMax) / 2;

        private int _bonusValue;
        public int BonusValue
        {
            get { return _bonusValue; }
            set
            {
                if (_bonusValue == value) return;

                _bonusValue = value;
                NotifyProperyChanged(nameof(BonusValue));
            }
        }

        public AttackDamage(int baseMin, int baseMax, BitmapImage icon) : base(icon)
        {
            BaseMin = baseMin;
            BaseMax = baseMax;
        }

        public void SetPrimaryAttribute(Attribute attribute)
        {
            attribute.PropertyChanged += OnAttributePropertyChange;
        }

        private void OnAttributePropertyChange(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != nameof(Attribute.TotalValue)) return;

            var attributeValue = ((Attribute) sender).TotalValue;

            MainMin = BaseMin + (int)attributeValue;
            MainMax = BaseMax + (int)attributeValue;
            NotifyProperyChanged(nameof(Average));
        }
    }

    public class Armor : BaseModel
    {
        public double BaseArmor { get; }

        public double MainArmor { get; private set; }

        private double _bonusArmor;
        public double BonusArmor
        {
            get { return _bonusArmor; }
            set
            {
                if (Math.Abs(_bonusArmor - value) < 1e-5) return;

                _bonusArmor = value;
                NotifyProperyChanged(nameof(BonusArmor));
            }
        }

        public Armor(double baseArmor, BitmapImage icon) : base(icon)
        {
            BaseArmor = baseArmor;
        }

        public void SetAgilityAttribute(Attribute agility)
        {
            agility.PropertyChanged += OnAgilityPropertyChange;
        }

        private void OnAgilityPropertyChange(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != nameof(Attribute.TotalValue)) return;

            var agilityValue = ((Attribute) sender).TotalValue;

            MainArmor = BaseArmor + agilityValue / 7.0;
            NotifyProperyChanged(nameof(MainArmor));
        }
    }

    public class MovementSpeed : BaseModel
    {
        public double BaseValue { get; }

        private double _bonusValue;
        public double BonusValue
        {
            get { return _bonusValue; }
            set
            {
                if (Math.Abs(_bonusValue - value) < 1e-5) return;

                _bonusValue = value;
                NotifyProperyChanged(nameof(TotalValue));
            }
        }

        public double TotalValue => BaseValue + BonusValue;

        public MovementSpeed(double baseValue, BitmapImage icon) : base(icon)
        {
            BaseValue = baseValue;
        }
    }

    public class Attribute : BaseModel
    {
        public AttributeType Type { get; }
        public double Value { get; private set; }
        public double Growth { get; }

        private double _bonusValue;
        public double BonusValue
        {
            get { return _bonusValue; }
            set
            {
                if (Math.Abs(_bonusValue - value) < 1e-5) return;

                _bonusValue = value;
                NotifyProperyChanged(nameof(BonusValue));
                NotifyProperyChanged(nameof(TotalValue));
            }
        }

        public double TotalValue => Value + BonusValue;

        private readonly double _startingValue;

        public Attribute(AttributeType type, double value, double growth, BitmapImage icon) : base(icon)
        {
            Type = type;
            Value = value;
            Growth = growth;
            _startingValue = value;
        }

        public void SetHero(Hero hero)
        {
            hero.PropertyChanged += OnHeroPropertyChange;
        }

        private void OnHeroPropertyChange(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != nameof(Hero.Level)) return;

            Value = _startingValue;
            var hero = (Hero) sender;

            for (var i = 0; i < hero.Level-1; i++)
            {
                Value += Growth;
            }

            NotifyProperyChanged(nameof(Value));
            NotifyProperyChanged(nameof(TotalValue));
        }
    }

    public enum AttributeType
    {
        Strength,
        Agility,
        Intelligence
    }

    public class Health : BaseModel
    {
        private const int BaseHp = 200;

        public int MaxHp { get; private set; }

        public Health(BitmapImage icon) : base(icon)
        {
        }

        public void SetStrengthAttribute(Attribute strength)
        {
            strength.PropertyChanged += OnTotalStrengthChange;
        }

        private void OnTotalStrengthChange(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != nameof(Attribute.TotalValue)) return;

            var strengthValue = (int) ((Attribute) sender).TotalValue;

            MaxHp = BaseHp + 20 * strengthValue;
            NotifyProperyChanged(nameof(MaxHp));
        }
    }

    public class Mana : BaseModel
    {
        private const int BaseMp = 50;

        public int MaxMp { get; private set; }

        public Mana(BitmapImage icon) : base(icon)
        {
        }

        public void SetIntelligenceAttribute(Attribute intelligence)
        {
            intelligence.PropertyChanged += OnTotalIntelligenceChange;
        }

        private void OnTotalIntelligenceChange(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != nameof(Attribute.TotalValue)) return;

            var intelligenceValue = (int) ((Attribute) sender).TotalValue;

            MaxMp = BaseMp + 12 * intelligenceValue;
            NotifyProperyChanged(nameof(MaxMp));
        }
    }
}