using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Windows.Media.Imaging;

namespace Dota2CharacterCalculator.Models
{
    public abstract class BaseModel : INotifyPropertyChanged
    {
        public BitmapImage Icon { get; }

        protected BaseModel(BitmapImage icon)
        {
            Icon = icon;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyProperyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class Hero : BaseModel
    {
        public string Name { get; }
        public AttackDamage Damage { get; }
        public Armor Armor { get; }
        public MovementSpeed BaseMs { get; }
        public Tuple<Attribute, Attribute, Attribute> Attributes { get; }
        public AttributeType PrimaryAttribute { get; }

        public List<Item> Items { get; } = new List<Item>(6);

        private int _level;
        public int Level
        {
            get { return _level; }
            private set
            {
                if (_level == value) return;

                _level = value;
                NotifyProperyChanged(nameof(Level));
            }
        }

        public Hero(string name, BitmapImage icon, AttackDamage damage, Armor armor,
                    MovementSpeed baseMs, Tuple<Attribute, Attribute, Attribute> attributes,
                    int level, AttributeType primaryAttribute) : base(icon)
        {
            Name = name;

            // Need to do it before damage and armor calculation
            Attributes = attributes;
            PrimaryAttribute = primaryAttribute;

            Damage = damage;
            Damage.Change(GetPrimaryAttribute().Value);

            Armor = armor;
            Armor.Change(Attributes.Item2.Value);

            BaseMs = baseMs;
            Level = level;
        }

        public bool CanIncreaseLevel()
        {
            return Level < 25;
        }

        public void IncreaseLevel()
        {
            Level++;
            ChangeAttributes();
            Damage.Change(GetPrimaryAttribute().Value);
            Armor.Change(Attributes.Item2.Value);
        }

        public bool CanDecreaseLevel()
        {
            return Level > 1;
        }

        public void DecreaseLevel()
        {
            Level--;
            ChangeAttributes();
            Damage.Change(GetPrimaryAttribute().Value);
            Armor.Change(Attributes.Item2.Value);
        }

        private void ChangeAttributes()
        {
            Attributes.Item1.Change(Level);
            Attributes.Item2.Change(Level);
            Attributes.Item3.Change(Level);
        }

        private Attribute GetPrimaryAttribute()
        {
            switch (PrimaryAttribute)
            {
                case AttributeType.Strength:
                    return Attributes.Item1;
                case AttributeType.Agility:
                    return Attributes.Item2;
                case AttributeType.Intelligence:
                    return Attributes.Item3;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public class AttackDamage : BaseModel
    {
        public int BaseMin { get; }
        public int BaseMax { get; }

        public int MainMax { get; private set; }
        public int MainMin { get; private set; }

        public int Average => (MainMin + MainMax) / 2;


        public AttackDamage(int baseMin, int baseMax, BitmapImage icon) : base(icon)
        {
            BaseMin = baseMin;
            BaseMax = baseMax;
        }

        public void Change(double attributeValue)
        {
            MainMin = BaseMin + (int)attributeValue;
            MainMax = BaseMax + (int)attributeValue;
            NotifyProperyChanged(nameof(Average));
        }
    }

    public class Armor : BaseModel
    {
        public double BaseArmor { get; }

        public double MainArmor { get; private set; }

        public Armor(double baseArmor, BitmapImage icon) : base(icon)
        {
            BaseArmor = baseArmor;
        }

        public void Change(double agilityValue)
        {
            MainArmor = BaseArmor + agilityValue / 7.0;
            NotifyProperyChanged(nameof(MainArmor));
        }
    }

    public class MovementSpeed : BaseModel
    {
        public double Value { get; }

        public MovementSpeed(double value, BitmapImage icon) : base(icon)
        {
            Value = value;
        }
    }

    public class Attribute : BaseModel
    {
        public AttributeType Type { get; }
        public double Value { get; private set; }
        public double Growth { get; }

        private readonly double _startingValue;

        public Attribute(AttributeType type, double value, double growth, BitmapImage icon) : base(icon)
        {
            Type = type;
            Value = value;
            Growth = growth;
            _startingValue = value;
        }

        public void Change(int level)
        {
            Value = _startingValue;

            for (var i = 0; i < level-1; i++)
            {
                Value += Growth;
            }
            NotifyProperyChanged(nameof(Value));
        }
    }

    public enum AttributeType
    {
        Strength,
        Agility,
        Intelligence
    }
}